using Holtron.Net.Network;
using System.Reflection;

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

            var dataField = typeof(NetIncomingMessage).GetField("m_data", BindingFlags.NonPublic | BindingFlags.Instance);
            dataField?.SetValue(inc, fromData);

            var bitLengthField = typeof(NetIncomingMessage).GetField("m_bitLength", BindingFlags.NonPublic | BindingFlags.Instance);
            bitLengthField?.SetValue(inc, bitLength);

            return inc;
        }
    }
}
