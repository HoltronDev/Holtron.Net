using System.Text;

namespace Holtron.Net
{
    public interface IPacketFormat
    {
        /// <summary>
        /// The encoding format to use for strings when they're converted directly to bytes for transport.
        /// </summary>
        Encoding StringEncoding { get; }

        int DecodeByte(MemoryStream buffer, out byte value);

        int DecodeSByte(MemoryStream buffer, out sbyte value);

        int DecodeUInt16(MemoryStream buffer, out ushort value);

        int DecodeInt16(MemoryStream buffer, out short value);

        int DecodeUInt32(MemoryStream buffer, out uint value);

        int DecodeInt32(MemoryStream buffer, out int value);

        int DecodeUInt64(MemoryStream buffer, out ulong value);

        int DecodeInt64(MemoryStream buffer, out long value);

        int DecodeHalf(MemoryStream buffer, out Half value);

        int DecodeSingle(MemoryStream buffer, out float value);

        int DecodeDouble(MemoryStream buffer, out double value);

        int DecodeString(MemoryStream buffer, out string value);

        int Encode(byte value, Span<byte> buffer, int offset = 0);

        int Encode(sbyte value, Span<byte> buffer, int offset = 0);

        int Encode(ushort value, Span<byte> buffer, int offset = 0);

        int Encode(short value, Span<byte> buffer, int offset = 0);

        int Encode(uint value, Span<byte> buffer, int offset = 0);

        int Encode(int value, Span<byte> buffer, int offset = 0);

        int Encode(ulong value, Span<byte> buffer, int offset = 0);

        int Encode(long value, Span<byte> buffer, int offset = 0);

        int Encode(Half value, Span<byte> buffer, int offset = 0);

        int Encode(float value, Span<byte> buffer, int offset = 0);

        int Encode(double value, Span<byte> buffer, int offset = 0);

        int Encode(string str, MemoryStream stream, bool includeSize = true);

        int GetStringByteSize(string str, bool includeSize = false);

        int GetEncodedSize<T>(T value);
    }

    public static partial class PacketFormat
    {
    }
}
