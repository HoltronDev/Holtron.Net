namespace Holtron.Net.Tests.UnitTests
{
    public class NetBuffer2Tests
    {
        [Theory]
        [InlineData(typeof(PacketFormat.None), 200)]
        public void BufferCanReadByte(Type packetFormat, byte value)
        {
            using var sut = CreateTestBuffer(packetFormat, new[] { value });
            var result = sut.Reader.ReadByte();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), -120)]
        public void BufferCanReadSByte(Type packetFormat, sbyte value)
        {
            using var sut = CreateTestBuffer(packetFormat, new[] { unchecked((byte)value), });
            var result = sut.Reader.ReadSByte();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 60_000)]
        public void BufferCanReadUInt16(Type packetFormat, ushort value)
        {
            var data = BitConverter.GetBytes(value);

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = sut.Reader.ReadUInt16();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 32_000)]
        public void BufferCanReadInt16(Type packetFormat, short value)
        {
            var data = BitConverter.GetBytes(value);

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = sut.Reader.ReadInt16();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 4_000_000_000)]
        public void BufferCanReadUInt32(Type packetFormat, uint value)
        {
            var data = BitConverter.GetBytes(value);

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = sut.Reader.ReadUInt32();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 2_000_000_000)]
        public void BufferCanReadInt32(Type packetFormat, int value)
        {
            var data = BitConverter.GetBytes(value);

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = sut.Reader.ReadInt32();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 16_000_000_000_000)]
        public void BufferCanReadUInt64(Type packetFormat, ulong value)
        {
            var data = BitConverter.GetBytes(value);

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = sut.Reader.ReadUInt64();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), -16_000_000_000_000)]
        public void BufferCanReadInt64(Type packetFormat, long value)
        {
            var data = BitConverter.GetBytes(value);

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = sut.Reader.ReadInt64();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 12.34)]
        public void BufferCanReadHalf(Type packetFormat, float value)
        {
            // xUnit has some trouble passing in a decimal value
            // to a test that expects a Half value. So, we'll
            // use float for the parameter then just cast it
            // before actually running the test.
            var actualValue = (Half)value;

            var data = BitConverter.GetBytes(actualValue);

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = sut.Reader.ReadHalf();
            Assert.Equal(actualValue, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 111.2233f)]
        public void BufferCanReadSingle(Type packetFormat, float value)
        {
            var data = BitConverter.GetBytes(value);

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = sut.Reader.ReadSingle();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 432.111d)]
        public void BufferCanReadDouble(Type packetFormat, double value)
        {
            var data = BitConverter.GetBytes(value);

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = sut.Reader.ReadDouble();
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 255, sizeof(byte))]
        public void BufferCanWriteByte(Type packetFormat, byte value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), -100, sizeof(sbyte))]
        public void BufferCanWriteSByte(Type packetFormat, sbyte value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 60_000, sizeof(ushort))]
        public void BufferCanWriteUInt16(Type packetFormat, ushort value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);

            var actualValue = BitConverter.ToUInt16(output);
            Assert.Equal(value, actualValue);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), -32_000, sizeof(ushort))]
        public void BufferCanWriteInt16(Type packetFormat, short value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 4_000_000_000, sizeof(uint))]
        public void BufferCanWriteUInt32(Type packetFormat, uint value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 1_000_000_000, sizeof(int))]
        public void BufferCanWriteInt32(Type packetFormat, int value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 900_000_000, sizeof(ulong))]
        public void BufferCanWriteUInt64(Type packetFormat, ulong value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), -900_000_000, sizeof(long))]
        public void BufferCanWriteInt64(Type packetFormat, long value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 12.34f, 2)]
        public void BufferCanWriteHalf(Type packetFormat, float value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write((Half)value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 12.34f, sizeof(float))]
        public void BufferCanWriteFloat(Type packetFormat, float value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write(value);
            Assert.Equal(expectedSizeWritten, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(expectedSizeWritten, output.Length);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None), 12.34d, sizeof(double))]
        public void BufferCanWriteDouble(Type packetFormat, double value, int expectedSizeWritten)
        {
            using var sut = CreateTestBuffer(packetFormat);
            var sizeWritten = sut.Writer.Write(value);
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
            var sizeWritten = sut.Writer.Write(testData);
            Assert.Equal(testData.Length, sizeWritten);

            var output = sut.ToArray();
            Assert.Equal(testData.Length, output.Length);
        }

        private NetBuffer2 CreateTestBuffer() => CreateTestBuffer(PacketFormat.None.Instance);

        private NetBuffer2 CreateTestBuffer(Type packetFormat) => packetFormat switch
        {
            _ => CreateTestBuffer(PacketFormat.None.Instance),
        };

        private NetBuffer2 CreateTestBuffer(IPacketFormat format) => new(format);

        private NetBuffer2 CreateTestBuffer(IPacketFormat format, byte[] data) => new(format, data);

        private NetBuffer2 CreateTestBuffer(Type packetFormat, byte[] data) => packetFormat switch
        {
            _ => CreateTestBuffer(PacketFormat.None.Instance, data),
        };
    }
}
