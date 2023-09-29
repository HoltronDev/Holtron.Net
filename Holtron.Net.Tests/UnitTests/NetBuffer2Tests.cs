using Newtonsoft.Json.Linq;

namespace Holtron.Net.Tests.UnitTests
{
    public class NetBuffer2Tests
    {
        [Theory]
        [InlineData(typeof(PacketFormat.None), 255, sizeof(byte))]
        public void BufferCanWriteByte(Type packetFormat, byte value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), -100, sizeof(sbyte))]
        public void BufferCanWriteSByte(Type packetFormat, sbyte value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 60_000, sizeof(ushort))]
        public void BufferCanWriteUInt16(Type packetFormat, ushort value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), -32_000, sizeof(ushort))]
        public void BufferCanWriteInt16(Type packetFormat, short value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 4_000_000_000, sizeof(uint))]
        public void BufferCanWriteUInt32(Type packetFormat, uint value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 1_000_000_000, sizeof(int))]
        public void BufferCanWriteInt32(Type packetFormat, int value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 900_000_000, sizeof(ulong))]
        public void BufferCanWriteUInt64(Type packetFormat, ulong value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), -900_000_000, sizeof(long))]
        public void BufferCanWriteInt64(Type packetFormat, long value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 12.34f, 2)]
        public void BufferCanWriteHalf(Type packetFormat, float value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write((Half)value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 12.34f, sizeof(float))]
        public void BufferCanWriteFloat(Type packetFormat, float value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 12.34d, sizeof(double))]
        public void BufferCanWriteDouble(Type packetFormat, double value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None))]
        public void BufferCanWriteByteArray(Type packetFormat)
        {
            var testData = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };

            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Write(testData);
            Assert.Equal(testData.Length, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(testData.Length, output.Length);
        }

        private NetBufferWriter CreateTestBuffer() => CreateTestBuffer(PacketFormat.None.Instance);

        private NetBufferWriter CreateTestBuffer(Type packetFormat) => packetFormat switch
        {
            _ => CreateTestBuffer(PacketFormat.None.Instance),
        };

        private NetBufferWriter CreateTestBuffer(IPacketFormat format) => new(format);
    }
}
