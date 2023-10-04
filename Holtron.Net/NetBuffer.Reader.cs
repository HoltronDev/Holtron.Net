namespace Holtron.Net
{
    public interface IBufferReader
    {
        byte ReadByte();

        int ReadByte(out byte value);

        sbyte ReadSByte();

        int ReadSByte(out sbyte value);

        ushort ReadUInt16();

        int ReadUInt16(out ushort value);

        short ReadInt16();

        int ReadInt16(out short value);

        uint ReadUInt32();

        int ReadUInt32(out uint value);

        int ReadInt32();

        int ReadInt32(out int value);

        ulong ReadUInt64();

        int ReadUInt64(out ulong value);

        long ReadInt64();

        int ReadInt64(out long value);

        Half ReadHalf();

        int ReadHalf(out Half value);

        float ReadSingle();

        int ReadSingle(out float value);

        double ReadDouble();

        int ReadDouble(out double value);

        int ReadBytes(byte[] buffer, int size);

        int ReadBytes(Span<byte> buffer);

        string ReadString();
    }

    public partial class NetBuffer2
    {
        public class BufferReader : IBufferReader
        {
            private readonly NetBuffer2 _buffer;

            public BufferReader(NetBuffer2 buffer)
            {
                _buffer = buffer;
            }

            public byte ReadByte()
            {
                _ = ReadByte(out var value);
                return value;
            }

            public int ReadByte(out byte value) =>
                _buffer.Format.DecodeByte(_buffer._buffer, out value);

            public sbyte ReadSByte()
            {
                _ = ReadSByte(out var value);
                return value;
            }

            public int ReadSByte(out sbyte value) =>
                _buffer.Format.DecodeSByte(_buffer._buffer, out value);

            public ushort ReadUInt16()
            {
                _ = ReadUInt16(out var value);
                return value;
            }

            public int ReadUInt16(out ushort value) =>
                _buffer.Format.DecodeUInt16(_buffer._buffer, out value);

            public short ReadInt16()
            {
                _ = ReadInt16(out var value);
                return value;
            }

            public int ReadInt16(out short value) =>
                _buffer.Format.DecodeInt16(_buffer._buffer, out value);

            public uint ReadUInt32()
            {
                _ = ReadUInt32(out var value);
                return value;
            }

            public int ReadUInt32(out uint value) =>
                _buffer.Format.DecodeUInt32(_buffer._buffer, out value);

            public int ReadInt32()
            {
                _ = ReadInt32(out var value);
                return value;
            }

            public int ReadInt32(out int value) =>
                _buffer.Format.DecodeInt32(_buffer._buffer, out value);

            public ulong ReadUInt64()
            {
                _ = ReadUInt64(out var value);
                return value;
            }

            public int ReadUInt64(out ulong value) =>
                _buffer.Format.DecodeUInt64(_buffer._buffer, out value);

            public long ReadInt64()
            {
                _ = ReadInt64(out var value);
                return value;
            }

            public int ReadInt64(out long value) =>
                _buffer.Format.DecodeInt64(_buffer._buffer, out value);

            public Half ReadHalf()
            {
                _ = ReadHalf(out var value);
                return value;
            }

            public int ReadHalf(out Half value) =>
                _buffer.Format.DecodeHalf(_buffer._buffer, out value);


            public float ReadSingle()
            {
                _ = ReadSingle(out var value);
                return value;
            }

            public int ReadSingle(out float value) =>
                _buffer.Format.DecodeSingle(_buffer._buffer, out value);

            public double ReadDouble()
            {
                _ = ReadDouble(out var value);
                return value;
            }

            public int ReadDouble(out double value) =>
                _buffer.Format.DecodeDouble(_buffer._buffer, out value);

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

            public string ReadString()
            {
                _ = _buffer.Format.DecodeString(_buffer._buffer, out var str);
                return str;
            }
        }
    }
}
