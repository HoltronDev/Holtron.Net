using System.Text;

namespace Holtron.Net.Network.Encryption
{
    public class NetEncryptionXtea : INetEncryption
    {
        private readonly int blockSize = 8;
        private readonly int numberOfRounds;
        private readonly uint[] sum0;
        private readonly uint[] sum1;

        private byte[] tempBlock;

        public NetEncryptionXtea(byte[] key, int rounds)
        {
            if(key.Length < 16)
            {
                throw new NetException("Key is too short!");
            }

            numberOfRounds = rounds;
            sum0 = new uint[numberOfRounds];
            sum1 = new uint[numberOfRounds];
            var tmp = new uint[8];

            var num2 = 0;
            for(int i = 0; i < 4; i++)
            {
                tmp[i] = BitConverter.ToUInt32(key, num2);
                num2 += 4;
            }

            num2 = 0;
            for(int i = 0; i < 32; i++)
            {
                sum0[i] = ((uint)num2) + tmp[num2 & 3];
                num2 -= 1640531527;
                sum1[i] = ((uint)num2) + tmp[(num2 >> 11) & 3];
            }

            tempBlock = new byte[blockSize];
        }

        public NetEncryptionXtea(byte[] key) : this(key, 32)
        { }

        public NetEncryptionXtea(string key) : this(NetUtility.ComputeSHAHash(Encoding.UTF8.GetBytes(key)), 32)
        { }

        public bool Decrypt(NetIncomingMessage message)
        {
            int numEncryptedBytes = message.LengthBytes - 4; // last 4 bytes is true bit length
            int numBlocks = numEncryptedBytes / blockSize;
            if (numBlocks * blockSize != numEncryptedBytes)
                return false;

            for (int i = 0; i < numBlocks; i++)
            {
                // Pack bytes into integers
                uint v0 = BytesToUInt(message.m_data, i * blockSize);
                uint v1 = BytesToUInt(message.m_data, i * blockSize + 4);

                for (int j = numberOfRounds - 1; j >= 0; j--)
                {
                    v1 -= (((v0 << 4) ^ (v0 >> 5)) + v0) ^ sum1[j];
                    v0 -= (((v1 << 4) ^ (v1 >> 5)) + v1) ^ sum0[j];
                }

                UIntToBytes(v0, tempBlock, 0);
                UIntToBytes(v1, tempBlock, 0 + 4);
                Buffer.BlockCopy(tempBlock, 0, message.m_data, (i * blockSize), tempBlock.Length);
            }

            // read 32 bits of true payload length
            uint realSize = NetBitWriter.ReadUInt32(message.m_data, 32, (numEncryptedBytes * 8));
            message.m_bitLength = (int)realSize;

            return true;
        }

        public void Encrypt(NetOutgoingMessage message)
        {
            int payloadBitLength = message.LengthBits;
            int numBytes = message.LengthBytes;
            int numBlocks = (int)Math.Ceiling((double)numBytes / (double)blockSize);
            int dstSize = numBlocks * blockSize;

            message.EnsureBufferSize(dstSize * 8 + (4 * 8)); // add 4 bytes for payload length at end
            message.LengthBits = dstSize * 8; // length will automatically adjust +4 bytes when payload length is written

            for (int i = 0; i < numBlocks; i++)
            {
                var v0 = BytesToUInt(message.m_data, i * blockSize);
                var v1 = BytesToUInt(message.m_data, i * blockSize + 4);

                for (int j = 0; j != numberOfRounds; j++)
                {
                    v0 += (((v1 << 4) ^ (v1 >> 5)) + v1) ^ sum0[j];
                    v1 += (((v0 << 4) ^ (v0 >> 5)) + v0) ^ sum1[j];
                }

                UIntToBytes(v0, tempBlock, 0);
                UIntToBytes(v1, tempBlock, 0 + 4);
                Buffer.BlockCopy(tempBlock, 0, message.m_data, (i * blockSize), tempBlock.Length);
            }

            // add true payload length last
            message.Write((UInt32)payloadBitLength);
        }

        public void SetKey(byte[] data, int offset, int length)
        {
            var key = NetUtility.ComputeSHAHash(data, offset, length);
            NetException.Assert(key.Length >= 16);
            SetKey(key, 0, 16);
        }

        private static uint BytesToUInt(byte[] bytes, int offset)
        {
            uint retval = (uint)(bytes[offset] << 24);
            retval |= (uint)(bytes[++offset] << 16);
            retval |= (uint)(bytes[++offset] << 8);
            return (retval | bytes[++offset]);
        }

        private static void UIntToBytes(uint value, byte[] destination, int destinationOffset)
        {
            destination[destinationOffset++] = (byte)(value >> 24);
            destination[destinationOffset++] = (byte)(value >> 16);
            destination[destinationOffset++] = (byte)(value >> 8);
            destination[destinationOffset++] = (byte)value;
        }
    }
}
