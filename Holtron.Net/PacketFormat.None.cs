using System.Buffers.Binary;
using System.Text;

namespace Holtron.Net
{
    public static partial class PacketFormat
    {
        public class None : IPacketFormat
        {
            /// <summary>
            /// Byte size of <see cref="Half"/>.
            /// </summary>
            private const int SIZE_HALF = 2;

            public static None Instance => LazyInstance.Value;

            private static readonly Lazy<None> LazyInstance = new(() => new None());

            public Encoding StringEncoding { get; } = new ASCIIEncoding();

            public int DecodeByte(MemoryStream buffer, Span<byte> readBuffer, out byte value)
            {
                value = (byte)buffer.ReadByte();
                return sizeof(byte);
            }

            public int DecodeSByte(MemoryStream buffer, Span<byte> readBuffer, out sbyte value)
            {
                value = (sbyte)buffer.ReadByte();
                return sizeof(sbyte);
            }

            public int DecodeUInt16(MemoryStream buffer, Span<byte> readBuffer, out ushort value)
            {
                var tmp = readBuffer[..sizeof(ushort)];

                var bytesRead = buffer.Read(tmp);
                value = (ushort)((tmp[1] << 8) | tmp[0]);
                return bytesRead;
            }

            public int DecodeInt16(MemoryStream buffer, Span<byte> readBuffer, out short value)
            {
                var tmp = readBuffer[..sizeof(ushort)];

                var bytesRead = buffer.Read(tmp);
                value = (short)((tmp[1] << 8) | tmp[0]);
                return bytesRead;
            }

            public int DecodeUInt32(MemoryStream buffer, Span<byte> readBuffer, out uint value)
            {
                var tmp = readBuffer[..sizeof(uint)];

                var bytesRead = buffer.Read(tmp);
                value = (uint)((tmp[3] << 24) | (tmp[2]) << 16 | (tmp[1] << 8) | tmp[0]);
                return bytesRead;
            }

            public int DecodeInt32(MemoryStream buffer, Span<byte> readBuffer, out int value)
            {
                var tmp = readBuffer[..sizeof(int)];

                var bytesRead = buffer.Read(tmp);
                value = (tmp[3] << 24) | (tmp[2]) << 16 | (tmp[1] << 8) | tmp[0];
                return bytesRead;
            }

            public int DecodeUInt64(MemoryStream buffer, Span<byte> readBuffer, out ulong value)
            {
                var tmp = readBuffer[..sizeof(ulong)];

                var bytesRead = buffer.Read(tmp);
                value = BinaryPrimitives.ReadUInt64LittleEndian(tmp);
                return bytesRead;
            }

            public int DecodeInt64(MemoryStream buffer, Span<byte> readBuffer, out long value)
            {
                var tmp = readBuffer[..sizeof(long)];

                var bytesRead = buffer.Read(tmp);
                value = BinaryPrimitives.ReadInt64LittleEndian(tmp);
                return bytesRead;
            }

            public int DecodeHalf(MemoryStream buffer, Span<byte> readBuffer, out Half value)
            {
                var tmp = readBuffer[..SIZE_HALF];

                var bytesRead = buffer.Read(tmp);
                value = BinaryPrimitives.ReadHalfLittleEndian(tmp);
                return bytesRead;
            }

            public int DecodeSingle(MemoryStream buffer, Span<byte> readBuffer, out float value)
            {
                var tmp = readBuffer[..sizeof(float)];

                var bytesRead = buffer.Read(tmp);
                value = BinaryPrimitives.ReadSingleLittleEndian(tmp);
                return bytesRead;
            }

            public int DecodeDouble(MemoryStream buffer, Span<byte> readBuffer, out double value)
            {
                var tmp = readBuffer[..sizeof(double)];
                
                var bytesRead = buffer.Read(tmp);
                value = BinaryPrimitives.ReadDoubleLittleEndian(tmp);
                return bytesRead;
            }

            public int Encode(byte value, Span<byte> buffer, int offset = 0)
            {
                buffer[offset] = value;
                return sizeof(byte);
            }

            public int Encode(sbyte value, Span<byte> buffer, int offset = 0)
            {
                
                buffer[offset] = unchecked((byte)value);
                return sizeof(sbyte);
            }

            public int Encode(ushort value, Span<byte> buffer, int offset = 0)
            {
                if (buffer.Length < sizeof(ushort))
                    return 0;

                BinaryPrimitives.WriteUInt16LittleEndian(buffer, value);
                return sizeof(ushort);
            }

            public int Encode(short value, Span<byte> buffer, int offset = 0)
            {
                if (buffer.Length < sizeof(short))
                    return 0;

                BinaryPrimitives.WriteInt16LittleEndian(buffer, value);
                return sizeof(short);
            }

            public int Encode(uint value, Span<byte> buffer, int offset = 0)
            {
                if (buffer.Length < sizeof(int))
                    return 0;

                BinaryPrimitives.WriteUInt32LittleEndian(buffer, value);
                return sizeof(uint);
            }

            public int Encode(int value, Span<byte> buffer, int offset = 0)
            {
                if (buffer.Length < sizeof(uint))
                    return 0;

                BinaryPrimitives.WriteInt32LittleEndian(buffer, value);
                return sizeof(uint);
            }

            public int Encode(ulong value, Span<byte> buffer, int offset = 0)
            {
                if (buffer.Length < sizeof(ulong))
                    return 0;

                BinaryPrimitives.WriteUInt64LittleEndian(buffer, value);
                return sizeof(ulong);
            }

            public int Encode(long value, Span<byte> buffer, int offset = 0)
            {
                if (buffer.Length < sizeof(long))
                    return 0;

                BinaryPrimitives.WriteInt64LittleEndian(buffer, value);
                return sizeof(long);
            }

            public int Encode(Half value, Span<byte> buffer, int offset = 0)
            {
                if (buffer.Length < SIZE_HALF)
                    return 0;

                BinaryPrimitives.WriteHalfLittleEndian(buffer, value);
                return SIZE_HALF;
            }

            public int Encode(float value, Span<byte> buffer, int offset = 0)
            {
                if (buffer.Length < sizeof(float))
                    return 0;

                BinaryPrimitives.WriteSingleLittleEndian(buffer, value);
                return sizeof(float);
            }

            public int Encode(double value, Span<byte> buffer, int offset = 0)
            {
                if (buffer.Length < sizeof(double))
                    return 0;

                BinaryPrimitives.WriteDoubleLittleEndian(buffer, value);
                return sizeof(double);
            }

            public int Encode(string str, Span<byte> buffer, int offset = 0)
            {
                if (buffer.Length < str.Length)
                    return 0;

                return StringEncoding.GetBytes(str, buffer.Slice(offset));
            }
        }
    }
}
