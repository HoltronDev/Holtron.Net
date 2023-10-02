namespace Holtron.Net
{
    public partial class NetBuffer2
    {
        public class BufferWriter
        {
            private readonly NetBuffer2 _buffer;

            private readonly Memory<byte> _writeBuffer = new(new byte[8 * DataSize.KILOBYTE]);

            private int _writeOffset;

            public BufferWriter(NetBuffer2 buffer)
            {
                _buffer = buffer;
            }

            public int Write(byte value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(sbyte value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(ushort value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(short value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(uint value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(int value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(ulong value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(long value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(Half value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(float value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(double value)
            {
                var formattedSize = _buffer.Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
                Interlocked.Add(ref _writeOffset, formattedSize);
                CommitPending();
                return formattedSize;
            }

            public int Write(byte[] data)
            {
                _buffer.WriteToStream(data);
                return data.Length;
            }

            private bool CommitPending() => _buffer.Commit(_writeBuffer, ref _writeOffset);
        }
    }
}
