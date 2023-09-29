namespace Holtron.Net.Network.Encryption
{
    public interface INetEncryption : IDisposable
    {
        /// <summary>
        /// Decrypt a message after recieving it.
        /// </summary>
        /// <param name="message"></param>
        public bool Decrypt(NetIncomingMessage message);

        /// <summary>
        /// Encrypt a message prior to it being transmitted over the wire.
        /// </summary>
        /// <param name="message"></param>
        public void Encrypt(NetOutgoingMessage message);

        /// <summary>
        /// Sets the key for encryption used by this Net Encryption
        /// </summary>
        /// <param name="data">Encryption Key</param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void SetKey(byte[] data, int offset, int length);
    }
}