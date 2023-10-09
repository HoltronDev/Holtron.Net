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
            private const int SIZE_HALF = sizeof(float) / 2;

            public static None Instance => LazyInstance.Value;

            private static readonly Lazy<None> LazyInstance = new(() => new None());

            public Encoding StringEncoding { get; } = new UTF8Encoding(false);

            private readonly Memory<byte> _readBuffer = new(new byte[8 * DataSize.KILOBYTE]);

            public int DecodeByte(MemoryStream buffer, out byte value)
            {
                value = (byte)buffer.ReadByte();
                return sizeof(byte);
            }

            public int DecodeSByte(MemoryStream buffer, out sbyte value)
            {
                value = (sbyte)buffer.ReadByte();
                return sizeof(sbyte);
            }

            public int DecodeUInt16(MemoryStream buffer, out ushort value)
            {
                var tmp = _readBuffer.Span[..sizeof(ushort)];

                var bytesRead = buffer.Read(tmp);
                value = (ushort)((tmp[1] << 8) | tmp[0]);
                return bytesRead;
            }

            public int DecodeInt16(MemoryStream buffer, out short value)
            {
                var tmp = _readBuffer.Span[..sizeof(ushort)];

                var bytesRead = buffer.Read(tmp);
                value = (short)((tmp[1] << 8) | tmp[0]);
                return bytesRead;
            }

            public int DecodeUInt32(MemoryStream buffer, out uint value)
            {
                var tmp = _readBuffer.Span[..sizeof(uint)];

                var bytesRead = buffer.Read(tmp);
                value = (uint)((tmp[3] << 24) | (tmp[2]) << 16 | (tmp[1] << 8) | tmp[0]);
                return bytesRead;
            }

            public int DecodeInt32(MemoryStream buffer, out int value)
            {
                var tmp = _readBuffer.Span[..sizeof(int)];

                var bytesRead = buffer.Read(tmp);
                value = (tmp[3] << 24) | (tmp[2]) << 16 | (tmp[1] << 8) | tmp[0];
                return bytesRead;
            }

            public int DecodeUInt64(MemoryStream buffer, out ulong value)
            {
                var tmp = _readBuffer.Span[..sizeof(ulong)];

                var bytesRead = buffer.Read(tmp);
                value = BinaryPrimitives.ReadUInt64LittleEndian(tmp);
                return bytesRead;
            }

            public int DecodeInt64(MemoryStream buffer, out long value)
            {
                var tmp = _readBuffer.Span[..sizeof(long)];

                var bytesRead = buffer.Read(tmp);
                value = BinaryPrimitives.ReadInt64LittleEndian(tmp);
                return bytesRead;
            }

            public int DecodeHalf(MemoryStream buffer, out Half value)
            {
                var tmp = _readBuffer.Span[..SIZE_HALF];

                var bytesRead = buffer.Read(tmp);
                value = BinaryPrimitives.ReadHalfLittleEndian(tmp);
                return bytesRead;
            }

            public int DecodeSingle(MemoryStream buffer, out float value)
            {
                var tmp = _readBuffer.Span[..sizeof(float)];

                var bytesRead = buffer.Read(tmp);
                value = BinaryPrimitives.ReadSingleLittleEndian(tmp);
                return bytesRead;
            }

            public int DecodeDouble(MemoryStream buffer, out double value)
            {
                var tmp = _readBuffer.Span[..sizeof(double)];
                
                var bytesRead = buffer.Read(tmp);
                value = BinaryPrimitives.ReadDoubleLittleEndian(tmp);
                return bytesRead;
            }

            public int DecodeString(MemoryStream buffer, out string value)
            {
                // First 4 bytes should contain the size of the string.
                var strSizeBytes = new byte[sizeof(uint)];
                _ = buffer.Read(strSizeBytes, 0, strSizeBytes.Length);

                var strSize = BinaryPrimitives.ReadUInt32LittleEndian(strSizeBytes.AsSpan());
                // If the local read buffer is large enough to hold the incoming string, we'll
                // just want to use that rather than creating a temporary buffer just for reading
                // this one string.
                int bytesRead;
                if (strSize < _readBuffer.Length)
                {
                    bytesRead = buffer.Read(_readBuffer.Span);
                    value = StringEncoding.GetString(_readBuffer.Span[..bytesRead]);
                }
                else
                {
                    var tmp = new byte[strSize].AsSpan();
                    bytesRead = buffer.Read(tmp);
                    value = StringEncoding.GetString(tmp);
                }

                return (int)(strSize + bytesRead);
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

            /// <summary>
            /// Strings will be encoded directly into the output stream without
            /// using an intermediate buffer.
            /// </summary>
            public int Encode(string str, MemoryStream stream, bool includeSize = true)
            {
                var sizeBytes = GetStringByteSize(str, includeSize: false);
                if (includeSize)
                {
                    // This (should) be a bit faster than using BitConverter
                    // to get convert the size of the string into a byte array
                    var dataLen = new[]
                    {
                        (byte)(sizeBytes),
                        (byte)(sizeBytes >> 8),
                        (byte)(sizeBytes >> 16),
                        (byte)(sizeBytes >> 24),
                    };
                    sizeBytes += dataLen.Length;
                    stream.Write(dataLen);
                }

                stream.Write(StringEncoding.GetBytes(str));
                return sizeBytes;
            }

            public int Encode<T>(T value, Span<byte> buffer, int offset = 0)
            {
                var encodedSize = GetEncodedSize(value);
                if (buffer.Length < encodedSize)
                    return 0;

                switch (value)
                {
                    case byte val:
                        buffer[offset] = val;
                        return sizeof(byte);
                    case sbyte val:
                        buffer[offset] = unchecked((byte)val);
                        return sizeof(sbyte);
                    case ushort val:
                        BinaryPrimitives.WriteUInt16LittleEndian(buffer, val);
                        return sizeof(ushort);
                    case short val:
                        BinaryPrimitives.WriteInt16LittleEndian(buffer, val);
                        return sizeof(short);
                    case uint val:
                        BinaryPrimitives.WriteUInt32LittleEndian(buffer, val);
                        return sizeof(uint);
                    case int val:
                        BinaryPrimitives.WriteInt32LittleEndian(buffer, val);
                        return sizeof(int);
                    case ulong val:
                        BinaryPrimitives.WriteUInt64LittleEndian(buffer, val);
                        return sizeof(ulong);
                    case long val:
                        BinaryPrimitives.WriteInt64LittleEndian(buffer, val);
                        return sizeof(long);
                    case Half val:
                        BinaryPrimitives.WriteHalfLittleEndian(buffer, val);
                        return SIZE_HALF;
                    case float val:
                        BinaryPrimitives.WriteSingleLittleEndian(buffer, val);
                        return sizeof(float);
                    case double val:
                        BinaryPrimitives.WriteDoubleLittleEndian(buffer, val);
                        return sizeof(double);
                    case string:
                        throw new InvalidOperationException(
                            "Encode<T> cannot be used to encode strings. Use Encode(string, MemoryStream, ...) instead.");
                    default:
                        throw new NotSupportedException($"Encoding {typeof(T).Name} is not supported.");
                }

                return 0;
            }

            public int GetStringByteSize(string str, bool includeSize = false) =>
                StringEncoding.GetByteCount(str) + (includeSize ? sizeof(uint) : 0);

            public int GetEncodedSize<T>(T value) => value switch
            {
                byte => sizeof(byte),
                sbyte => sizeof(sbyte),
                ushort => sizeof(ushort),
                short => sizeof(short),
                uint => sizeof(uint),
                int => sizeof(int),
                ulong => sizeof(ulong),
                long => sizeof(long),
                Half => sizeof(float) / 2,
                float => sizeof(float),
                double => sizeof(double),
                byte[] v => v.Length,
                string str => GetStringByteSize(str),
                _ => 0, // Unrecognized type.
            };
        }
    }
}
