using System.Numerics;

namespace Holtron.Net
{
    public class NetBufferWriter : NetBuffer2
    {
        public int PendingLength => _writeOffset;

        private Memory<byte> _writeBuffer = new(new byte[8 * 1024]);
        private int _writeOffset = 0;

        public NetBufferWriter()
        {
        }

        public NetBufferWriter(IPacketFormat packetFormat)
            : base(packetFormat)
        {
        }

        public NetBufferWriter(IPacketFormat packetFormat, byte[] data)
            : base(packetFormat, data)
        {
        }

        public int Write(byte value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(sbyte value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(ushort value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(short value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(uint value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(int value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(ulong value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(long value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(Half value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(float value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(double value)
        {
            var formattedSize = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, formattedSize);
            CommitPending();
            return formattedSize;
        }

        public int Write(byte[] data)
        {
            WriteToStream(data);
            return data.Length;
        }

        private bool CommitPending() => Commit(_writeBuffer, ref _writeOffset);
    }
}
