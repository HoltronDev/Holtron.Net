using Holtron.Net.Network;

namespace Holtron.Net.Tests
{
    public static class HelperMethods
    {
        public static NetIncomingMessage? CreateIncomingMessage(byte[] fromData, int bitLength)
        {
            NetIncomingMessage? inc = (NetIncomingMessage?)Activator.CreateInstance(typeof(NetIncomingMessage), true);
            if (inc == null)
            {
                return null;
            }

            inc.Data = fromData;
            inc.LengthBits = bitLength;

            return inc;
        }
    }
}
