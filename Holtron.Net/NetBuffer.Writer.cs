namespace Holtron.Net
{
    public interface IBufferWriter
    {
        int Write(byte value);

        int Write(sbyte value);

        int Write(ushort value);

        int Write(short value);

        int Write(uint value);

        int Write(int value);

        int Write(ulong value);

        int Write(long value);

        int Write(Half value);

        int Write(float value);

        int Write(double value);

        int Write(byte[] data);

        int Write(string str);
    }

    public partial class NetBuffer2
    {
        public class BufferWriter : IBufferWriter
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

            public int Write(string str)
            {
                if (string.IsNullOrEmpty(str))
                    return 0;

                return _buffer.Format.Encode(str, _buffer._buffer);
            }

            private bool CommitPending() => _buffer.Commit(_writeBuffer, ref _writeOffset);
        }
    }
}
