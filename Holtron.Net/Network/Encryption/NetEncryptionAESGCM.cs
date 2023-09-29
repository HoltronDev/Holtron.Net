using System.Security.Cryptography;
using System.Text;

namespace Holtron.Net.Network.Encryption
{
    public class NetEncryptionAESGCM : INetEncryption
    {
        // This implementation is from
        // https://stackoverflow.com/questions/60889345/using-the-aesgcm-class
        private readonly AesGcm aesGcm;
        private const int KEY_ROUNDS = 200000;

        public NetEncryptionAESGCM(string key) :
            this(Encoding.UTF8.GetBytes(key))
        { }

        public NetEncryptionAESGCM(Span<byte> key)
            : this(key.ToArray())
        { }

        public NetEncryptionAESGCM(byte[] key)
        {
            var salt = new byte[8];
            RandomNumberGenerator.Fill(salt);
            using var deriveBytes = new Rfc2898DeriveBytes(key, salt, KEY_ROUNDS);
            var derivedKey = deriveBytes.GetBytes(16);
            aesGcm = new AesGcm(derivedKey);
        }

        public bool Decrypt(NetIncomingMessage message)
        {
            var unencryptedLengthBits = (int)message.ReadUInt32();
            var encryptedDataLength = message.ReadInt32();
            var encryptedData = message.ReadBytes(encryptedDataLength).AsSpan();
            var nonce = encryptedData[..AesGcm.NonceByteSizes.MinSize];
            var tag = encryptedData.Slice(AesGcm.NonceByteSizes.MinSize, AesGcm.TagByteSizes.MinSize);
            var cipherText = encryptedData[(AesGcm.NonceByteSizes.MinSize + AesGcm.TagByteSizes.MinSize)..];
            var plainTextMessage = new byte[cipherText.Length];

            aesGcm.Decrypt(nonce, cipherText, tag, plainTextMessage);

            message.m_data = plainTextMessage.ToArray();
            message.m_bitLength = unencryptedLengthBits;
            message.m_readPosition = 0;

            return true;
        }

        public void Dispose()
        {
            aesGcm.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Encrypt(NetOutgoingMessage message)
        {
            var bytesToEncrypt = new byte[message.m_data.Length];
            message.m_data.CopyTo(bytesToEncrypt, 0);
            var unencryptedLengthBits = message.LengthBits;

            var encryptedDataLength = AesGcm.NonceByteSizes.MinSize + AesGcm.TagByteSizes.MinSize + bytesToEncrypt.Length;

            Span<byte> encryptedData = encryptedDataLength < 1024
                ? stackalloc byte[encryptedDataLength]
                : new byte[encryptedDataLength].AsSpan();

            var nonce = encryptedData[..AesGcm.NonceByteSizes.MinSize];
            var tag = encryptedData.Slice(AesGcm.NonceByteSizes.MinSize, AesGcm.TagByteSizes.MinSize);
            var cipherBytes = encryptedData.Slice(AesGcm.NonceByteSizes.MinSize + AesGcm.TagByteSizes.MinSize, bytesToEncrypt.Length);

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
            throw new NotImplementedException();
        }
    }
}
