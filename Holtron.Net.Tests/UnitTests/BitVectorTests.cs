using Holtron.Net.Network;

namespace Holtron.Net.Tests.UnitTests
{
    public class BitVectorTests
    {
        [Fact]
        public void BitVectorTest()
        {
            var bitVector = new NetBitVector(256);
            for (int i = 0; i < 256; i++)
            {
                bitVector.Clear();
                if (i > 42 && i < 65)
                {
                    bitVector = new NetBitVector(256);
                }

                Assert.True(bitVector.IsEmpty());

                bitVector.Set(i, true);

                Assert.True(bitVector.Get(i));
                Assert.False(bitVector.IsEmpty());
                if (i != 79)
                {
                    Assert.False(bitVector.Get(79));
                }

                var indexFromBitVector = bitVector.GetFirstSetIndex();
                Assert.True(indexFromBitVector == i);
            }
        }
    }
}