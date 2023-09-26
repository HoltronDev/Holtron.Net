using System.Security.Cryptography;

namespace Holtron.Net.Network.Encryption
{
    public class NetEncryptionAESGCM : INetEncryption
    {
        // This implementation is from
        // https://stackoverflow.com/questions/60889345/using-the-aesgcm-class
        private readonly AesGcm aesGcm;

        public NetEncryptionAESGCM(byte[] key)
        {
            var derivedKey = new Rfc2898DeriveBytes(key, new byte[8], 1000).GetBytes(16);
            aesGcm = new AesGcm(derivedKey);
        }

        public NetEncryptionAESGCM(string key)
        {
            var derivedKey = new Rfc2898DeriveBytes(key, new byte[8], 1000).GetBytes(16);
            aesGcm = new AesGcm(derivedKey);
        }

        public NetEncryptionAESGCM(Span<byte> key)
        {
            var derivedKey = new Rfc2898DeriveBytes(key.ToArray(), new byte[8], 1000).GetBytes(16);
            aesGcm = new AesGcm(derivedKey);
        }

        public bool Decrypt(NetIncomingMessage message)
        {
            var unencryptedLengthBits = (int)message.ReadUInt32();
            var encryptedDataLength = message.ReadInt32();
            var encryptedData = message.ReadBytes(encryptedDataLength).AsSpan();
            var nonce = encryptedData.Slice(0, 12);
            var tag = encryptedData.Slice(12, 12);
            var cipherText = encryptedData.Slice(24, encryptedData.Length - 24);
            var plainTextMessage = new byte[cipherText.Length];

            aesGcm.Decrypt(nonce, cipherText, tag, plainTextMessage);

            message.Data = plainTextMessage.ToArray();
            message.m_data = plainTextMessage.ToArray();
            message.m_bitLength = unencryptedLengthBits;
            message.m_readPosition = 0;

            return true;
        }

        public void Encrypt(NetOutgoingMessage message)
        {
            var bytesToEncrypt = new byte[message.Data.Length];
            message.Data.CopyTo(bytesToEncrypt, 0);
            var unencryptedLengthBits = message.LengthBits;

            var encryptedDataLength = 24 + bytesToEncrypt.Length;

            Span<byte> encryptedData = encryptedDataLength < 1024
                ? stackalloc byte[encryptedDataLength]
                : new byte[encryptedDataLength].AsSpan();

            var nonce = encryptedData.Slice(0, 12);
            var tag = encryptedData.Slice(12, 12);
            var cipherBytes = encryptedData.Slice(24, bytesToEncrypt.Length);

            RandomNumberGenerator.Fill(nonce);

            aesGcm.Encrypt(nonce, message.Data, cipherBytes, tag);

            message.EnsureBufferSize((encryptedData.Length + 8) * 8);
            message.LengthBits = 0; // reset write pointer
            message.Write((uint)unencryptedLengthBits);
            message.Write(encryptedData.Length);
            message.Write(encryptedData.ToArray());
            message.LengthBits = (encryptedData.Length + 8) * 8;
        }

        public void SetKey(byte[] data, int offset, int length)
        {
            
        }
    }
}
