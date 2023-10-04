namespace Holtron.Net
{
    public interface INetBuffer : IDisposable
    {
        IPacketFormat Format { get; }

        long Length { get; }

        /// <summary>
        /// Provides a way to call Seek on the underlying stream.
        /// This method will return 0 and have no effect if Seek cannot
        /// be used with the underlying stream.
        /// </summary>
        long Seek(long offset, SeekOrigin origin);

        byte[] ToArray();
    }

    public partial class NetBuffer2 : INetBuffer
    {
        public long Length => _buffer.Length;

        public IPacketFormat Format { get; }

        public IBufferReader Reader { get; }

        public IBufferWriter Writer { get; }

        private readonly MemoryStream _buffer;

        public NetBuffer2()
            : this(new PacketFormat.None())
        {
        }

        public NetBuffer2(IPacketFormat packetFormat)
            : this(packetFormat, reader: default, writer: default)
        {
        }

        public NetBuffer2(IPacketFormat packetFormat, byte[] data)
            : this(packetFormat, data, reader: default,  writer: default)
        {
        }

        protected NetBuffer2(IPacketFormat packetFormat,
            IBufferReader? reader = default,
            IBufferWriter? writer = default)
        {
            _buffer = new MemoryStream();
            Format = packetFormat;
            Reader = reader ?? new BufferReader(this);
            Writer = writer ?? new BufferWriter(this);
        }

        protected NetBuffer2(IPacketFormat packetFormat, byte[] data,
            IBufferReader? reader = default,
            IBufferWriter? writer = default)
        {
            _buffer = new MemoryStream(data);
            Format = packetFormat;
            Reader = reader ?? new BufferReader(this);
            Writer = writer ?? new BufferWriter(this);
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

        /// <inheritdoc cref="INetBuffer.Seek"/>
        public long Seek(long offset, SeekOrigin origin)
        {
            if (!_buffer.CanRead)
                return 0L;

            return _buffer.Seek(offset, origin);
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
