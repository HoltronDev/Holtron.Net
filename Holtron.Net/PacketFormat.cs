using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holtron.Net
{
    public interface IPacketFormat
    {
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
    }

    public static partial class PacketFormat
    {
    }
}
