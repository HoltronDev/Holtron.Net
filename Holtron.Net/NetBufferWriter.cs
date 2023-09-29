using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holtron.Net
{
    public class NetBufferWriter : NetBuffer2
    {
        public int PendingLength => _writeOffset;

        private Memory<byte> _writeBuffer = new(new byte[8 * 1024]);
        private int _writeOffset = 0;

        public NetBufferWriter()
        {
        }

        public NetBufferWriter(IPacketFormat packetFormat)
            : base(packetFormat)
        {
        }

        public NetBufferWriter(IPacketFormat packetFormat, byte[] data)
            : base(packetFormat, data)
        {
        }

        public int Write(byte value)
        {
            var bytesWritten = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, bytesWritten);
            Commit(_writeBuffer, ref _writeOffset);
            return bytesWritten;
        }

        public int Write(sbyte value) => Write(unchecked((byte)value));

        public int Write(ushort value)
        {
            var bytesWritten = Format.Encode(value, _writeBuffer.Span.Slice(_writeOffset));
            Interlocked.Add(ref _writeOffset, bytesWritten);
            Commit(_writeBuffer, ref _writeOffset);
            return bytesWritten;
        }

        public int Write(short value) => Write(unchecked((ushort)value));
    }
}
