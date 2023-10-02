namespace Holtron.Net
{
    public interface INetBuffer : IDisposable
    {
        IPacketFormat Format { get; }

        long Length { get; }

        byte[] ToArray();
    }

    public partial class NetBuffer2 : INetBuffer
    {
        public long Length => _buffer.Length;

        public IPacketFormat Format { get; }

        public BufferReader Reader { get; }

        public BufferWriter Writer { get; }

        private readonly MemoryStream _buffer;

        public NetBuffer2()
            : this(new PacketFormat.None())
        {
        }

        public NetBuffer2(IPacketFormat packetFormat)
        {
            _buffer = new MemoryStream();
            Format = packetFormat;
            Reader = new BufferReader(this);
            Writer = new BufferWriter(this);
        }

        public NetBuffer2(IPacketFormat packetFormat, byte[] data)
        {
            _buffer = new MemoryStream(data);
            Format = packetFormat;
            Reader = new BufferReader(this);
            Writer = new BufferWriter(this);
        }

        ~NetBuffer2()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public byte[] ToArray() => _buffer.ToArray();

        protected bool Commit(Memory<byte> pending, ref int length)
        {
            var tmp = pending.Span[..length];
            _buffer.Write(tmp);
            tmp.Fill(0);
            length = 0;
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _buffer.Dispose();
            }
        }

        /// <summary>
        /// Write an array of bytes directly to the underlying stream.
        /// </summary>
        protected void WriteToStream(byte[] data) => _buffer.Write(data, 0, data.Length);
    }
}
