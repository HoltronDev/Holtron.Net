namespace Holtron.Net
{
    public interface IBufferWriter
    {
        bool HasPendingData { get; }

        int PendingBufferSize { get; }

        bool CommitPending();

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

        int Write<T>(T value);
    }

    public partial class NetBuffer2
    {
        public class BufferWriter : IBufferWriter
        {
            public bool HasPendingData => _writeOffset != 0;
            
            public int PendingBufferSize => _writeBuffer.Length;

            private readonly NetBuffer2 _buffer;

            private readonly Memory<byte> _writeBuffer = new(new byte[8 * DataSize.KILOBYTE]);

            private int _writeOffset;

            public BufferWriter(NetBuffer2 buffer)
            {
                _buffer = buffer;
            }

            public bool CommitPending()
            {
                // Nothing to actually commit.
                if (!HasPendingData)
                    return true;

                return _buffer.Commit(_writeBuffer, ref _writeOffset);
            }

            public int Write(byte value) => Write<byte>(value);

            public int Write(sbyte value) => Write<sbyte>(value);

            public int Write(ushort value) => Write<ushort>(value);

            public int Write(short value) => Write<short>(value);

            public int Write(uint value) => Write<uint>(value);

            public int Write(int value) => Write<int>(value);

            public int Write(ulong value) => Write<ulong>(value);

            public int Write(long value) => Write<long>(value);

            public int Write(Half value) => Write<Half>(value);

            public int Write(float value) => Write<float>(value);

            public int Write(double value) => Write<double>(value);

            public int Write(byte[] data) => Write<byte[]>(data);

            public int Write(string str) => Write<string>(str);

            public int Write<T>(T value)
            {
                var expectedSize = _buffer.Format.GetEncodedSize(value);
                if (expectedSize == 0)
                {
                    // Unrecognized type will not be written to the pending buffer.
                    return 0;
                }

                if (_writeOffset + expectedSize > _writeBuffer.Length)
                    CommitPending();

                if (value is byte[] bytes)
                {
                    _buffer.WriteToStream(bytes);
                    return bytes.Length;
                }

                if (value is string str)
                    return WriteString(str);

                if (_writeOffset > _writeBuffer.Length)
                {
                    throw new InvalidOperationException("Cannot generate buffer destination for write.");
                }

                var buffer = _writeBuffer.Span.Slice(_writeOffset);
                var formattedSize = value switch
                {
                    byte val => _buffer.Format.Encode(val, buffer),
                    sbyte val => _buffer.Format.Encode(val, buffer),
                    ushort val => _buffer.Format.Encode(val, buffer),
                    short val => _buffer.Format.Encode(val, buffer),
                    uint val => _buffer.Format.Encode(val, buffer),
                    int val => _buffer.Format.Encode(val, buffer),
                    ulong val => _buffer.Format.Encode(val, buffer),
                    long val => _buffer.Format.Encode(val, buffer),
                    Half val => _buffer.Format.Encode(val, buffer),
                    float val => _buffer.Format.Encode(val, buffer),
                    double val => _buffer.Format.Encode(val, buffer),
                    _ => throw new NotSupportedException($"Encoding {typeof(T).Name} is not supported."),
                };

                Interlocked.Add(ref _writeOffset, formattedSize);
                return formattedSize;
            }

            private int WriteString(string str)
            {
                if (string.IsNullOrEmpty(str))
                    return 0;

                return _buffer.Format.Encode(str, _buffer._buffer);
            }
        }
    }
}
