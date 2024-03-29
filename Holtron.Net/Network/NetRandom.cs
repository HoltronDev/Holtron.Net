namespace Holtron.Net.Network
{
    /// <summary>
    /// NetRandom base class
    /// </summary>
    public abstract class NetRandom : Random
    {
        protected const double REAL_UNIT_INT = 1.0 / ((double)int.MaxValue + 1.0);

        /// <summary>
        /// Same value (usually) as <see cref="int.MaxValue"/>
        /// </summary>
        private const uint INT_OFFSET = int.MaxValue;

        private uint m_boolValues;
        private int m_nextBoolIndex;

        /// <summary>
        /// Generates a random value from UInt32.MinValue to UInt32.MaxValue, inclusively
        /// </summary>
        public abstract uint NextUInt32();

        /// <summary>
        /// Generates a random value that is greater or equal than 0 and less than Int32.MaxValue
        /// </summary>
        public override int Next()
        {
            var ret = (int)(INT_OFFSET & NextUInt32());
            if (ret == INT_OFFSET)
                return NextInt32();
            return ret;
        }

        /// <summary>
        /// Generates a random value greater or equal than 0 and less or equal than Int32.MaxValue (inclusively)
        /// </summary>
        public int NextInt32()
        {
            return (int)(INT_OFFSET & NextUInt32());
        }

        /// <summary>
        /// Returns random value larger or equal to 0.0 and less than 1.0
        /// </summary>
        public override double NextDouble()
        {
            return REAL_UNIT_INT * NextInt32();
        }

        /// <summary>
        /// Returns random value is greater or equal than 0.0 and less than 1.0
        /// </summary>
        protected override double Sample()
        {
            return REAL_UNIT_INT * NextInt32();
        }

        /// <summary>
        /// Returns random value is greater or equal than 0.0f and less than 1.0f
        /// </summary>
        public override float NextSingle()
        {
            var ret = (float)(REAL_UNIT_INT * NextInt32());
            if (ret == 1.0f)
                return NextSingle();
            return ret;
        }

        /// <summary>
        /// Returns a random value is greater or equal than 0 and less than maxValue
        /// </summary>
        public override int Next(int maxValue)
        {
            return (int)(NextDouble() * maxValue);
        }

        /// <summary>
        /// Returns a random value is greater or equal than minValue and less than maxValue
        /// </summary>
        public override int Next(int minValue, int maxValue)
        {
            return minValue + (int)(NextDouble() * (double)(maxValue - minValue));
        }
        
        /// <summary>
        /// Generates a random value between UInt64.MinValue to UInt64.MaxValue
        /// </summary>
        public virtual ulong NextUInt64()
        {
            ulong ret = NextUInt32();
            ret <<= 32;
            ret |= NextUInt32();
            return ret;
        }

        /// <summary>
        /// Returns true or false, randomly
        /// </summary>
        public virtual bool NextBool()
        {
            if (m_nextBoolIndex >= 32)
            {
                m_boolValues = NextUInt32();
                m_nextBoolIndex = 1;
            }

            m_nextBoolIndex++;
            return ((m_boolValues >> m_nextBoolIndex) & 1) == 1;
        }
        

        /// <summary>
        /// Fills all bytes from offset to offset + length in buffer with random values
        /// </summary>
        public virtual void NextBytes(byte[] buffer, int offset, int length)
        {
            int full = length / 4;
            int ptr = offset;
            for (int i = 0; i < full; i++)
            {
                uint r = NextUInt32();
                buffer[ptr++] = (byte)r;
                buffer[ptr++] = (byte)(r >> 8);
                buffer[ptr++] = (byte)(r >> 16);
                buffer[ptr++] = (byte)(r >> 24);
            }

            int rest = length - (full * 4);
            for (int i = 0; i < rest; i++)
                buffer[ptr++] = (byte)NextUInt32();
        }

        /// <summary>
        /// Fill the specified buffer with random values
        /// </summary>
        public override void NextBytes(byte[] buffer)
        {
            NextBytes(buffer, 0, buffer.Length);
        }
    }
}
