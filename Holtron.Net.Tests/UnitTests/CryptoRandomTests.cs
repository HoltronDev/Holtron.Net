using Holtron.Net.Network;

namespace Holtron.Net.Tests.UnitTests
{
    public class CryptoRandomTests
    {
        [Fact]
        public void NextUInt32GeneratesANumber()
        {
            var actual = CryptoRandom.Instance.NextUInt32();
            Assert.NotEqual((uint)0, actual);
        }

        [Fact]
        public void NextBytesReturnsFilledArray()
        {
            var buffer = new byte[8];
            CryptoRandom.Instance.NextBytes(buffer);
            Assert.All(buffer, n => Assert.NotEqual(0, n));
        }

        [Fact]
        public void NextUInt64GeneratesNumber()
        {
            var actual = CryptoRandom.Instance.NextUInt64();
            Assert.NotEqual((ulong)0, actual);
        }

        [Fact]
        public void NextUInt64FillsAbove32Bits()
        {
            const ulong threshold = 0x00000001_00000000;
            const int tryCount = 1_000;

            var results = new List<bool>();
            // We want to run this a good number of times to make sure
            // that we're not just seeing a one-off situation where the
            // test is passing.
            for (var idx = 0; idx < tryCount; idx++)
            {
                var actual = CryptoRandom.Instance.NextUInt64();
                results.Add(actual > threshold);
            }

            Assert.All(results, Assert.True);
        }
    }
}
