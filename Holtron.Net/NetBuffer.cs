using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holtron.Net
{
    public interface INetBuffer : IDisposable
    {
        long Length { get; }
    }

    public class NetBuffer2 : INetBuffer
    {
        public long Length => _buffer.Length;

        protected IPacketFormat Format { get; }

        private MemoryStream _buffer;

        public NetBuffer2()
            : this(new PacketFormat.None())
        {
        }

        public NetBuffer2(IPacketFormat packetFormat)
        {
            _buffer = new MemoryStream();
            Format = packetFormat;
        }

        public NetBuffer2(IPacketFormat packetFormat, byte[] data)
        {
            _buffer = new MemoryStream(data);
            Format = packetFormat;
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
    }
}
