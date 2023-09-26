using System.Security.Cryptography;
using System.Text;

namespace Holtron.Net.Network.Encryption
{
    public class NetEncryptionAES : INetEncryption
    {
        private readonly Aes aes;

        public NetEncryptionAES(string key)
        {
            aes = Aes.Create();
            var bytes = Encoding.ASCII.GetBytes(key);
            SetKey(bytes, 0, bytes.Length);
            //aes.BlockSize = 1024;
            aes.Padding = PaddingMode.PKCS7;
        }

        public NetEncryptionAES(byte[] data, int offset, int count)
        {
            aes = Aes.Create();
            SetKey(data, offset, count);
            //aes.BlockSize = 1024;
            aes.Padding = PaddingMode.PKCS7;
        }

        public bool Decrypt(NetIncomingMessage message)
        {
            int unEncLenBits = (int)message.ReadUInt32();

            using var ms = new MemoryStream(message.m_data, 4, message.LengthBytes - 4);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);

            var byteLen = NetUtility.BytesToHoldBits(unEncLenBits);
            var byteBuffer = new byte[byteLen];
            cs.Read(byteBuffer, 0, byteLen);
            //var bytesRead = cs.Read(byteBuffer, 0, byteLen);
            var streamIndex = 0;
            while(streamIndex < byteLen)
            {
                cs.Read(byteBuffer, streamIndex, 16);
                streamIndex += 16;
            }

            // TODO: recycle existing msg

            //message.m_data = byteBuffer;
            message.m_data = byteBuffer;
            message.m_bitLength = unEncLenBits;
            message.m_readPosition = 0;

            return true;
        }

        public void Encrypt(NetOutgoingMessage message)
        {
            int unEncLenBits = message.LengthBits;

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(message.m_data, 0, message.LengthBytes);
            cs.Flush();

            // get results
            var arr = ms.ToArray();

            message.EnsureBufferSize((arr.Length + 4) * 8);
            message.LengthBits = 0; // reset write pointer
            message.Write((uint)unEncLenBits);
            message.Write(arr);
            message.LengthBits = (arr.Length + 4) * 8;
        }

        public void SetKey(byte[] data, int offset, int length)
        {
            int len = aes.Key.Length;
            var key = new byte[len];
            for (int i = 0; i < len; i++)
                key[i] = data[offset + (i % length)];
            aes.Key = key;

            len = aes.IV.Length;
            key = new byte[len];
            for (int i = 0; i < len; i++)
                key[len - 1 - i] = data[offset + (i % length)];
            aes.IV = key;
        }
    }
}
