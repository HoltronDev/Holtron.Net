using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holtron.Net.Tests.UnitTests
{
    public class NetBuffer2Tests
    {
        [Fact]
        public void WriteTest()
        {
            using var sut = new NetBufferWriter();
            var bytesWritten = sut.Write(255);
            Assert.Equal(1, bytesWritten);

            bytesWritten = sut.Write(-127);
            Assert.Equal(1, bytesWritten);

            bytesWritten = sut.Write(60_000);
            Assert.Equal(2, bytesWritten);

            bytesWritten = sut.Write(-30_000);
            Assert.Equal(2, bytesWritten);

            var output = sut.ToArray();
            Assert.Equal(6L, output.Length);
        }
    }
}
