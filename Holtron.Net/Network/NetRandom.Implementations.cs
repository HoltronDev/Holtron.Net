using System.Numerics;
using System.Security.Cryptography;

namespace Holtron.Net.Network
{
    /// <summary>
    /// Multiply With Carry random
    /// </summary>
    public class MultiplyWithCarryRandom : NetRandom
    {
        /// <summary>
        /// Get global instance of MultiplyWithCarryRandom
        /// </summary>
        public static MultiplyWithCarryRandom Instance => LazyInstance.Value;

        private static readonly Lazy<MultiplyWithCarryRandom> LazyInstance = new(CreateInstance);

        private static MultiplyWithCarryRandom CreateInstance() => new();

        private uint m_w, m_z;

        /// <summary>
        /// Constructor with randomized seed
        /// </summary>
        public MultiplyWithCarryRandom()
        {
            var seed = NetRandomSeed.GetUInt64();
            m_w = (uint)seed;
            m_z = (uint)(seed >> 32);
        }

        /// <summary>
        /// Generates a random value from UInt32.MinValue to UInt32.MaxValue, inclusively
        /// </summary>
        public override uint NextUInt32()
        {
            m_z = 36969 * (m_z & 65535) + (m_z >> 16);
            m_w = 18000 * (m_w & 65535) + (m_w >> 16);
            return ((m_z << 16) + m_w);
        }
    }

    /// <summary>
    /// Xor Shift based random
    /// </summary>
    public sealed class XorShiftRandom : NetRandom
    {
        private const uint CX = 123456789;
        private const uint CY = 362436069;
        private const uint CZ = 521288629;
        private const uint CW = 88675123;

        /// <summary>
        /// Get global instance of XorShiftRandom
        /// </summary>
        public static XorShiftRandom Instance => LazyInstance.Value;

        private static readonly Lazy<XorShiftRandom> LazyInstance = new(CreateInstance);

        private static XorShiftRandom CreateInstance() => new();

        private uint m_x, m_y, m_z, m_w;

        /// <summary>
        /// Constructor with randomized seed
        /// </summary>
        public XorShiftRandom()
        {
            var seed = NetRandomSeed.GetUInt64();
            m_x = (uint)seed;
            m_y = CY;
            m_z = CZ;
            m_w = CW;
        }

        /// <summary>
        /// Constructor with provided 64 bit seed
        /// </summary>
        public XorShiftRandom(ulong seed)
        {
            m_x = (uint)seed;
            m_y = CY;
            m_z = (uint)(seed << 32);
            m_w = CW;
        }

        /// <summary>
        /// Generates a random value from UInt32.MinValue to UInt32.MaxValue, inclusively
        /// </summary>
        public override uint NextUInt32()
        {
            uint t = (m_x ^ (m_x << 11));
            m_x = m_y; m_y = m_z; m_z = m_w;
            return (m_w = (m_w ^ (m_w >> 19)) ^ (t ^ (t >> 8)));
        }
    }

    /// <summary>
    /// Mersenne Twister based random
    /// </summary>
    public sealed class MersenneTwisterRandom : NetRandom
    {
        private const int N = 624;
        private const int M = 397;
        private const uint MATRIX_A = 0x9908b0dfU;
        private const uint UPPER_MASK = 0x80000000U;
        private const uint LOWER_MASK = 0x7fffffffU;
        private const uint TEMPER1 = 0x9d2c5680U;
        private const uint TEMPER2 = 0xefc60000U;
        private const int TEMPER3 = 11;
        private const int TEMPER4 = 7;
        private const int TEMPER5 = 15;
        private const int TEMPER6 = 18;

        /// <summary>
        /// Get global instance of MersenneTwisterRandom
        /// </summary>
        public new static MersenneTwisterRandom Instance => LazyInstance.Value;

        private static readonly Lazy<MersenneTwisterRandom> LazyInstance = new(CreateInstance);

        private static MersenneTwisterRandom CreateInstance() => new();

        private uint[] mt;
        private int mti;
        private uint[] mag01;

        /// <summary>
        /// Constructor with randomized seed
        /// </summary>
        public MersenneTwisterRandom()
            : this(NetRandomSeed.GetUInt32())
        {
        }

        /// <summary>
        /// Constructor with provided 32 bit seed
        /// </summary>
        public MersenneTwisterRandom(uint seed)
        {
            mt = new uint[N];
            mti = N + 1;
            mag01 = new uint[] { 0x0U, MATRIX_A };
            mt[0] = seed;
            for (int i = 1; i < N; i++)
                mt[i] = (uint)(1812433253 * (mt[i - 1] ^ (mt[i - 1] >> 30)) + i);
        }

        /// <summary>
        /// Generates a random value from UInt32.MinValue to UInt32.MaxValue, inclusively
        /// </summary>
        public override uint NextUInt32()
        {
            if (mti >= N)
            {
                GenRandAll();
                mti = 0;
            }
            var y = mt[mti++];
            y ^= (y >> TEMPER3);
            y ^= (y << TEMPER4) & TEMPER1;
            y ^= (y << TEMPER5) & TEMPER2;
            y ^= (y >> TEMPER6);
            return y;
        }

        private void GenRandAll()
        {
            int kk = 1;
            uint p;
            var y = mt[0] & UPPER_MASK;
            do
            {
                p = mt[kk];
                mt[kk - 1] = mt[kk + (M - 1)] ^ ((y | (p & LOWER_MASK)) >> 1) ^ mag01[p & 1];
                y = p & UPPER_MASK;
            } while (++kk < N - M + 1);
            do
            {
                p = mt[kk];
                mt[kk - 1] = mt[kk + (M - N - 1)] ^ ((y | (p & LOWER_MASK)) >> 1) ^ mag01[p & 1];
                y = p & UPPER_MASK;
            } while (++kk < N);
            p = mt[0];
            mt[N - 1] = mt[M - 1] ^ ((y | (p & LOWER_MASK)) >> 1) ^ mag01[p & 1];
        }
    }

    /// <summary>
    /// RNGCryptoServiceProvider based random; very slow but cryptographically safe
    /// </summary>
    public class CryptoRandom : NetRandom
    {
        /// <summary>
        /// Global instance of CryptoRandom
        /// </summary>
        public static CryptoRandom Instance => LazyInstance.Value;

        private static readonly Lazy<CryptoRandom> LazyInstance = new(CreateInstance);

        private static CryptoRandom CreateInstance() => new();

        public CryptoRandom()
            : this(NetRandomSeed.GetUInt32())
        {
        }

        public CryptoRandom(uint seed)
        {
            RandomNumberGenerator.GetBytes((int)(seed % 16));
        }

        /// <summary>
        /// Generates a random value from UInt32.MinValue to UInt32.MaxValue, inclusively
        /// </summary>
        [CLSCompliant(false)]
        public override uint NextUInt32()
        {
            var bytes = RandomNumberGenerator.GetBytes(sizeof(uint));
            return (uint)bytes[0] | (((uint)bytes[1]) << 8) | (((uint)bytes[2]) << 16) | (((uint)bytes[3]) << 24);
        }

        /// <summary>
        /// Fill the specified buffer with random values
        /// </summary>
        public override void NextBytes(byte[] buffer)
        {
            // We need to use a temporary buffer and
            // Buffer.BlockCopy in order to make this
            // work as expected.
            //
            // RandomNumberGenerator won't directly byte[].
            var tmp = RandomNumberGenerator.GetBytes(buffer.Length);
            Buffer.BlockCopy(tmp, 0, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Fills all bytes from offset to offset + length in buffer with random values
        /// </summary>
        public override void NextBytes(byte[] buffer, int offset, int length)
        {
            var bytes = RandomNumberGenerator.GetBytes(length);
            Buffer.BlockCopy(bytes, 0, buffer, offset, length);
        }
    }
}
