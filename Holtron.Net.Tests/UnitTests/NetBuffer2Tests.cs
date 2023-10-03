using System.Security.Cryptography;
using System.Text;
using Xunit.Abstractions;

namespace Holtron.Net.Tests.UnitTests
{
    public class NetBuffer2Tests
    {
        private ITestOutputHelper Output { get; }

        public NetBuffer2Tests(ITestOutputHelper output)
        {
            Output = output;
        }

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
        [InlineData(typeof(PacketFormat.None))]
        public void BufferCanReadByteArray(Type packetFormat)
        {
            var data = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = new byte[data.Length];
            sut.Reader.ReadBytes(result, 4);

            Assert.Equal(data, result);
        }

        [Theory]
        [InlineData(typeof(PacketFormat.None))]
        public void BufferCanReadByteArrayToSpan(Type packetFormat)
        {
            var data = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };

            using var sut = CreateTestBuffer(packetFormat, data);
            var result = new Memory<byte>(new byte[8]);
            sut.Reader.ReadBytes(result.Span);

            Assert.Equal(result.Slice(0, 4).ToArray(), data);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("The quick brown fox jumped over the lazy dog.")]
        public void BufferCanReadBasicString(string str)
        {
            using var sut = CreateTestBuffer(PacketFormat.None.Instance);
            sut.Writer.Write(str);
            sut.Seek(0, SeekOrigin.Begin);

            var actual = sut.Reader.ReadString();
            Assert.Equal(str, actual);
        }

        [Fact]
        public void BufferCanReadLargeString()
        {
            // Generate a string that should be large enough to fill the underlying read buffer.
            var str = GenerateRandomString(10 * DataSize.KILOBYTE);

            using var sut = CreateTestBuffer(PacketFormat.None.Instance);
            sut.Writer.Write(str);
            sut.Seek(0, SeekOrigin.Begin);

            var actual = sut.Reader.ReadString();
            Assert.Equal(str, actual);
        }

        [Theory]
        [InlineData("これは日本語です。")]
        [InlineData("اللغة العربية هي لغة جيدة للاختبار من اليمين إلى اليسار.")]
        [InlineData("Быстрая, коричневая лиса, перепрыгнула через ленивого пса.")]
        public void BufferCanReadStringWithUnicode(string str)
        {
            using var sut = CreateTestBuffer(PacketFormat.None.Instance);
            sut.Writer.Write(str);
            sut.Seek(0, SeekOrigin.Begin);

            var actual = sut.Reader.ReadString();
            Assert.Equal(str, actual);
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

        [Theory]
        [InlineData("test")]
        [InlineData("The quick brown fox jumped over the lazy dog.")]
        public void BufferCanWriteBasicString(string str)
        {
            using var sut = CreateTestBuffer(PacketFormat.None.Instance);
            var expectedSize = sut.Format.GetStringByteSize(str, includeSize: true);
            var sizeWritten = sut.Writer.Write(str);
            Assert.Equal(expectedSize, sizeWritten);

            var output = sut.ToArray().AsSpan();
            var outStr = sut.Format.StringEncoding.GetString(output.Slice(sizeof(uint)));
            Assert.Equal(str, outStr);
        }

        [Fact]
        public void BufferCanWriteLargeString()
        {
            // Generate a string that should be large enough to fill the underlying write buffer.
            var str = GenerateRandomString(10 * DataSize.KILOBYTE);

            using var sut = CreateTestBuffer(PacketFormat.None.Instance);
            var expectedSize = sut.Format.GetStringByteSize(str, includeSize: true);
            var sizeWritten = sut.Writer.Write(str);
            Assert.Equal(expectedSize, sizeWritten);

            var output = sut.ToArray().AsSpan();
            var outStr = sut.Format.StringEncoding.GetString(output.Slice(sizeof(uint)));
            Assert.Equal(str, outStr);
        }

        [Theory]
        [InlineData("これは日本語です。")]
        [InlineData("اللغة العربية هي لغة جيدة للاختبار من اليمين إلى اليسار.")]
        [InlineData("Быстрая, коричневая лиса, перепрыгнула через ленивого пса.")]
        public void BufferCanWriteStringWithUnicode(string str)
        {
            using var sut = CreateTestBuffer(PacketFormat.None.Instance);
            var expectedSize = sut.Format.GetStringByteSize(str, includeSize: true);
            var sizeWritten = sut.Writer.Write(str);
            Assert.Equal(expectedSize, sizeWritten);

            var output = sut.ToArray().AsSpan();
            var outSizeBytes = output.Slice(0, sizeof(uint));
            var outSize = BitConverter.ToUInt32(outSizeBytes);
            Assert.Equal((uint)(expectedSize - sizeof(uint)), outSize);

            var outStr = sut.Format.StringEncoding.GetString(output.Slice(sizeof(uint)));
            Assert.Equal(str, outStr);
        }

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

        private string GenerateRandomString(int size)
        {
            var str = new StringBuilder(size);
            for (var c = 0; c < size; c++)
                str.Append((char)RandomNumberGenerator.GetInt32(97, 123));
            return str.ToString();
        }
    }
}
