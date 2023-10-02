namespace Holtron.Net
{
    public partial class NetBuffer2
    {
        public class BufferReader
        {
            private readonly NetBuffer2 _buffer;

            private readonly Memory<byte> _readBuffer = new(new byte[8 * DataSize.KILOBYTE]);

            public BufferReader(NetBuffer2 buffer)
            {
                _buffer = buffer;
            }

            public byte ReadByte()
            {
                _ = _buffer.Format.DecodeByte(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public sbyte ReadSByte()
            {
                _ = _buffer.Format.DecodeSByte(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public ushort ReadUInt16()
            {
                _ = _buffer.Format.DecodeUInt16(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public short ReadInt16()
            {
                _ = _buffer.Format.DecodeInt16(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public uint ReadUInt32()
            {
                _ = _buffer.Format.DecodeUInt32(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public int ReadInt32()
            {
                _ = _buffer.Format.DecodeInt32(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public ulong ReadUInt64()
            {
                _ = _buffer.Format.DecodeUInt64(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public long ReadInt64()
            {
                _ = _buffer.Format.DecodeInt64(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public Half ReadHalf()
            {
                _ = _buffer.Format.DecodeHalf(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public float ReadSingle()
            {
                _ = _buffer.Format.DecodeSingle(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public double ReadDouble()
            {
                _ = _buffer.Format.DecodeDouble(_buffer._buffer, _readBuffer.Span, out var value);
                return value;
            }

            public int ReadBytes(byte[] buffer, int size)
            {
                var bytesRead = _buffer._buffer.Read(buffer, 0, size);
                return bytesRead;
            }

            public int ReadBytes(Span<byte> buffer)
            {
                var bytesRead = _buffer._buffer.Read(buffer);
                return bytesRead;
            }
        }
    }
}
