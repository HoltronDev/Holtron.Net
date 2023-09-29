using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Holtron.Net
{
    public static partial class PacketFormat
    {
        public class None : IPacketFormat
        {
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

            public int Encode(ushort value, Span<byte> buffer, int offset = 0) =>
                CopyBytes(BitConverter.GetBytes(value), buffer, offset);

            public int Encode(short value, Span<byte> buffer, int offset = 0) =>
                CopyBytes(BitConverter.GetBytes(value), buffer, offset);

            public int Encode(uint value, Span<byte> buffer, int offset = 0) =>
                CopyBytes(BitConverter.GetBytes(value), buffer, offset);

            public int Encode(int value, Span<byte> buffer, int offset = 0) =>
                CopyBytes(BitConverter.GetBytes(value), buffer, offset);

            public int Encode(ulong value, Span<byte> buffer, int offset = 0) =>
                CopyBytes(BitConverter.GetBytes(value), buffer, offset);

            public int Encode(long value, Span<byte> buffer, int offset = 0) =>
                CopyBytes(BitConverter.GetBytes(value), buffer, offset);

            public int Encode(Half value, Span<byte> buffer, int offset= 0) =>
                CopyBytes(BitConverter.GetBytes(value), buffer, offset);

            public int Encode(float value, Span<byte> buffer, int offset = 0) =>
                CopyBytes(BitConverter.GetBytes(value), buffer, offset);

            public int Encode(double value, Span<byte> buffer, int offset = 0) =>
                CopyBytes(BitConverter.GetBytes(value), buffer, offset);

            private int CopyBytes(byte[] data, Span<byte> buffer, int offset)
            {
                data.CopyTo(buffer.Slice(offset));
                return data.Length;
            }
        }
    }
}
