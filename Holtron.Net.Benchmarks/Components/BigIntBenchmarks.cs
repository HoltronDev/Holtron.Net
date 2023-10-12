using BenchmarkDotNet.Attributes;
using Holtron.Net.Network;
using System.Numerics;

namespace Holtron.Net.Benchmarks.Components
{
    public class BigIntBenchmarks
    {
        private readonly byte[] data1;
        private readonly byte[] data2;

        private readonly NetBigInteger netBigInteger1;
        private readonly NetBigInteger netBigInteger2;
        private readonly NetBigInteger netBigIntegerPow = new NetBigInteger("3");

        private readonly BigInteger bigInt1;
        private readonly BigInteger bigInt2;
        private readonly BigInteger bigIntPow = new BigInteger(3);

        public BigIntBenchmarks()
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

        [Benchmark]
        public void NetBigInteger_Abs() => netBigInteger1.Abs();

        [Benchmark]
        public void BigInt_Abs() => BigInteger.Abs(bigInt1);

        [Benchmark]
        public void NetBigInteger_Add() => netBigInteger1.Add(netBigInteger2);

        [Benchmark]
        public void BigInt_Add() => BigInteger.Add(bigInt1, bigInt2);

        [Benchmark]
        public void NetBigInteger_And() => netBigInteger1.And(netBigInteger2);

        [Benchmark]
        public void BigInt_And() => _ = bigInt1 & bigInt2;

        [Benchmark]
        public void NetBigInteger_Compare() => netBigInteger1.CompareTo(netBigInteger2);

        [Benchmark]
        public void BigInt_Compare() => BigInteger.Compare(bigInt1, bigInt2);

        [Benchmark]
        public void NetBigInteger_Divide() => netBigInteger1.Divide(netBigInteger2);

        [Benchmark]
        public void BigInt_Divide() => BigInteger.Divide(bigInt1, bigInt2);

        [Benchmark]
        public void NetBigInteger_DivideAndRemainder() => netBigInteger1.DivideAndRemainder(netBigInteger2);

        [Benchmark]
        public void BigInt_DivideAndRemainder() => BigInteger.DivRem(bigInt1, bigInt2, out _);

        [Benchmark]
        public void NetBigInteger_Equals() => netBigInteger1.Equals(netBigInteger2);

        [Benchmark]
        public void BigInt_Equals() => Equals(bigInt1, bigInt2);

        [Benchmark]
        public void NetBigInteger_Gcd() => netBigInteger1.Gcd(netBigInteger2);

        [Benchmark]
        public void BigInt_Gcd() => BigInteger.GreatestCommonDivisor(bigInt1, bigInt2);

        [Benchmark]
        public void NetBigInteger_GetHashCode() => netBigInteger1.GetHashCode();

        [Benchmark]
        public void BigInt_GetHashCode() => bigInt1.GetHashCode();

        [Benchmark]
        public void NetBigInteger_Max() => netBigInteger1.Max(netBigInteger2);

        [Benchmark]
        public void BigInt_Max() => BigInteger.Max(bigInt1, bigInt2);

        [Benchmark]
        public void NetBigInteger_Min() => netBigInteger1.Min(netBigInteger2);

        [Benchmark]
        public void BigInt_Min() => BigInteger.Min(bigInt1, bigInt2);

        [Benchmark]
        public void NetBigInteger_Mod() => netBigInteger1.Mod(netBigInteger2);

        [Benchmark]
        public void BigInt_Mod() => _ = bigInt1 % bigInt2;

        [Benchmark]
        public void NetBigInteger_ModInverse() => netBigInteger1.ModInverse(netBigIntegerPow);

        // May be wrong, pulled from https://stackoverflow.com/a/15768873
        [Benchmark]
        public void BigInt_ModInverse() => BigInteger.ModPow(bigInt1, bigIntPow - 2, bigInt2);

        [Benchmark]
        public void NetBigInteger_ModPow() => netBigInteger1.ModPow(netBigIntegerPow, netBigInteger2);

        [Benchmark]
        public void BigInt_ModPow() => BigInteger.ModPow(bigInt1, bigIntPow, bigInt2);

        [Benchmark]
        public void NetBigInteger_Multiply() => netBigInteger1.Multiply(netBigInteger2);

        [Benchmark]
        public void BigInteger_Multiply() => BigInteger.Multiply(bigInt1, bigInt2);

        [Benchmark]
        public void NetBigInteger_Negate() => netBigInteger1.Negate();

        [Benchmark]
        public void BigInteger_Negate() => BigInteger.Negate(bigInt1);

        [Benchmark]
        public void NetBigInteger_Not() => netBigInteger1.Not();

        [Benchmark]
        public void BigInteger_Not() 
        {
            var incBigInt = bigInt1 + 1;
            BigInteger.Negate(incBigInt);
        }

        [Benchmark]
        public void NetBigInteger_Pow() => netBigInteger1.Pow(3);

        [Benchmark]
        public void BigInteger_Pow() => BigInteger.Pow(bigInt1, 3);

        [Benchmark]
        public void NetBigInteger_Remainder() => netBigInteger1.Remainder(netBigInteger2);

        [Benchmark]
        public void BigInteger_Remainder() => BigInteger.Remainder(bigInt1, bigInt2);

        [Benchmark]
        public void NetBigInteger_ShiftLeft() => netBigInteger1.ShiftLeft(3);

        [Benchmark]
        public void BigInteger_ShiftLeft() => _ = bigInt1 << 3;

        [Benchmark]
        public void NetBigInteger_ShiftRight() => netBigInteger1.ShiftRight(3);

        [Benchmark]
        public void BigInteger_ShiftRight() => _ = bigInt1 >> 3;

        [Benchmark]
        public void NetBigInteger_Subtract() => netBigInteger1.Subtract(netBigInteger2);

        [Benchmark]
        public void BigInteger_Subtract() => BigInteger.Subtract(bigInt1, bigInt2);

        [Benchmark]
        public void NetBigInteger_ToByteArray() => netBigInteger1.ToByteArray();

        [Benchmark]
        public void BigInteger_ToByteArray() => bigInt1.ToByteArray();

        [Benchmark]
        public void NetBigInteger_ToString() => netBigInteger1.ToString();

        [Benchmark]
        public void BigInteger_ToString() => bigInt1.ToString();
    }
}
