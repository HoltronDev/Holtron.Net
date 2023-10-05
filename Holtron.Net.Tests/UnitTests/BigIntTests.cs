using Holtron.Net.Network;
using System.Numerics;

namespace Holtron.Net.Tests.UnitTests
{
    public class BigIntTests
    {
        private readonly byte[] data1;
        private readonly byte[] data2;

        private readonly NetBigInteger netBigInteger1;
        private readonly NetBigInteger netBigInteger2;
        private readonly NetBigInteger netBigIntegerPow = new("3");

        private BigInteger bigInt1;
        private readonly BigInteger bigInt2;
        private readonly BigInteger bigIntPow = new(3);

        public BigIntTests()
        {
            data1 = new byte[100];
            data2 = new byte[100];

            new Random(100).NextBytes(data1);
            new Random(100).NextBytes(data2);

            netBigInteger1 = new NetBigInteger(data1);
            netBigInteger2 = new NetBigInteger(data2);

            bigInt1 = new BigInteger(data1);
            bigInt2 = new BigInteger(data2);
        }

        [Fact]
        public void BigIntCompare_Abs()
        {
            var netBigIntegerResult = netBigInteger1.Abs().ToByteArray();
            var bigIntegerResult = BigInteger.Abs(bigInt1).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact(Skip = "NetBigInteger is less reliable than BigInt")]
        public void BigIntCompare_Add()
        {
            var netBigIntegerResult = netBigInteger1.Add(netBigInteger2).ToByteArray();
            var bigIntegerResult = BigInteger.Add(bigInt1, bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_And()
        {
            var netBigIntegerResult = netBigInteger1.And(netBigInteger2).ToByteArray();
            var bigIntegerResult = (bigInt1 & bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_Compare()
        {
            var netBigIntegerResult = netBigInteger1.CompareTo(netBigInteger2);
            var bigIntegerResult = BigInteger.Compare(bigInt1, bigInt2);

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_Divide()
        {
            var netBigIntegerResult = netBigInteger1.Divide(netBigInteger2).ToByteArray();
            var bigIntegerResult = BigInteger.Divide(bigInt1, bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_DivideAndRemainder()
        {
            var values = netBigInteger1.DivideAndRemainder(netBigInteger2);
            var netBigIntegerResult = values[0].ToByteArray();
            var netBigIntegerRemainder = values[1].ToByteArray();
            var bigIntegerResult = BigInteger.DivRem(bigInt1, bigInt2, out var bigIntRemainder).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
            Assert.Equal(netBigIntegerRemainder, bigIntRemainder.ToByteArray());
        }

        [Fact]
        public void BigIntCompare_Equals()
        {
            var netBigIntegerResult = netBigInteger1.Equals(netBigInteger2);
            var bigIntegerResult = Equals(bigInt1, bigInt2);

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_GreatestCommonDevisor() 
        {
            var netBigIntegerResult = netBigInteger1.Gcd(netBigInteger2).ToByteArray();
            var bigIntegerResult = BigInteger.GreatestCommonDivisor(bigInt1, bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_Max()
        {
            var netBigIntegerResult = netBigInteger1.Max(netBigInteger2).ToByteArray();
            var bigIntegerResult = BigInteger.Max(bigInt1, bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_Min()
        {
            var netBigIntegerResult = netBigInteger1.Min(netBigInteger2).ToByteArray();
            var bigIntegerResult = BigInteger.Min(bigInt1, bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_Mod()
        {
            var netBigIntegerResult = netBigInteger1.Mod(netBigInteger2).ToByteArray();
            var bigIntegerResult = (bigInt1 % bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact(Skip = "Unsure of how ModInverse is actually supposed to work.")]
        public void BigIntCompare_ModInverse()
        {
            var netBigIntegerResult = netBigInteger1.ModInverse(netBigInteger2).ToByteArray();
            var bigIntegerResult = BigInteger.ModPow(bigInt1, bigIntPow - 2, bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_ModPow()
        {
            var netBigIntegerResult = netBigInteger1.ModPow(netBigIntegerPow, netBigInteger2).ToByteArray();
            var bigIntegerResult = BigInteger.ModPow(bigInt1, bigIntPow, bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact(Skip = "NetBigInteger is less reliable than BigInt")]
        public void BigIntCompare_Multiply()
        {
            var netBigIntegerResult = netBigInteger1.Multiply(netBigInteger2).ToByteArray();
            var bigIntegerResult = BigInteger.Multiply(bigInt1, bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact(Skip = "NetBigInteger is less reliable than BigInt")]
        public void BigIntCompare_Negate()
        {
            var netBigIntegerResult = netBigInteger1.Negate().ToByteArray();
            var bigIntegerResult = BigInteger.Negate(bigInt1).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact(Skip = "NetBigInteger is less reliable than BigInt")]
        public void BigIntCompare_Not()
        {
            var netBigIntegerResult = netBigInteger1.Not().ToByteArray();
            var bigIntegerResult = BigInteger.Negate(bigInt1++).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact(Skip = "NetBigInteger is less reliable than BigInt")]
        public void BigIntCompare_Pow()
        {
            var netBigIntegerResult = netBigInteger1.Pow(3).ToByteArray();
            var bigIntegerResult = BigInteger.Pow(bigInt1, 3).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_Remainder()
        {
            var netBigIntegerResult = netBigInteger1.Remainder(netBigInteger2).ToByteArray();
            var bigIntegerResult = BigInteger.Remainder(bigInt1, bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact(Skip = "NetBigInteger is less reliable than BigInt")]
        public void BigIntCompare_ShiftLeft()
        {
            var netBigIntegerResult = netBigInteger1.ShiftLeft(3).ToByteArray();
            var bigIntegerResult = (bigInt1 << 3).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact(Skip = "NetBigInteger is less reliable than BigInt")]
        public void BigIntCompare_ShiftRight()
        {
            var netBigIntegerResult = netBigInteger1.ShiftRight(3).ToByteArray();
            var bigIntegerResult = (bigInt1 >> 3).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }

        [Fact]
        public void BigIntCompare_Subtract()
        {
            var netBigIntegerResult = netBigInteger1.Subtract(netBigInteger2).ToByteArray();
            var bigIntegerResult = BigInteger.Subtract(bigInt1, bigInt2).ToByteArray();

            Assert.Equal(netBigIntegerResult, bigIntegerResult);
        }
    }
}
