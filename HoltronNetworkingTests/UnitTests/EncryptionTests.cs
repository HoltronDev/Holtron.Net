using HoltronNetworking.Network;

namespace HoltronNetworkingTests.UnitTests
{
    public class EncryptionTests
    {
        private static NetPeer _peer = new NetPeer(new NetPeerConfiguration("test"));

        [Fact]
        public void NetXorEncryptionTest()
        {
            RunAlgorithmTest(new NetXorEncryption(_peer, "TopSecret"));
        }

        [Fact]
        public void NetXteaTest()
        {
            RunAlgorithmTest(new NetXtea(_peer, "TopSecret"));
        }

        [Fact]
        public void NetAESEncryptionTest()
        {
            RunAlgorithmTest(new NetAESEncryption(_peer, "TopSecret"));
        }

        [Fact]
        public void NetRC2EncryptionTest()
        {
            RunAlgorithmTest(new NetRC2Encryption(_peer, "TopSecret"));
        }

        [Fact]
        public void NetDESEncryptionTest()
        {
            RunAlgorithmTest(new NetDESEncryption(_peer, "TopSecret"));
        }

        [Fact]
        public void NetTripleDESEncryptionTest()
        {
            RunAlgorithmTest(new NetTripleDESEncryption(_peer, "TopSecret"));
        }

        [Fact]
        public void NetSRPTest()
        {
            for (int i = 0; i < 100; i++)
            {
                var salt = NetSRP.CreateRandomSalt();
                var x = NetSRP.ComputePrivateKey("user", "password", salt);

                var v = NetSRP.ComputeServerVerifier(x);
                //Console.WriteLine("v = " + NetUtility.ToHexString(v));

                var a = NetSRP.CreateRandomEphemeral(); //  NetUtility.ToByteArray("393ed364924a71ba7258633cc4854d655ca4ec4e8ba833eceaad2511e80db2b5");
                var A = NetSRP.ComputeClientEphemeral(a);
                //Console.WriteLine("A = " + NetUtility.ToHexString(A));

                var b = NetSRP.CreateRandomEphemeral(); // NetUtility.ToByteArray("cc4d87a90db91067d52e2778b802ca6f7d362490c4be294b21b4a57c71cf55a9");
                var B = NetSRP.ComputeServerEphemeral(b, v);
                //Console.WriteLine("B = " + NetUtility.ToHexString(B));

                var u = NetSRP.ComputeU(A, B);
                //Console.WriteLine("u = " + NetUtility.ToHexString(u));

                var Ss = NetSRP.ComputeServerSessionValue(A, v, u, b);
                //Console.WriteLine("Ss = " + NetUtility.ToHexString(Ss));

                var Sc = NetSRP.ComputeClientSessionValue(B, x, u, a);
                //Console.WriteLine("Sc = " + NetUtility.ToHexString(Sc));

                Assert.Equal(Ss.Length, Sc.Length);

                for (int j = 0; j < Ss.Length; j++)
                {
                    Assert.Equal(Ss[j], Sc[j]);
                }

                var netSRPTest = NetSRP.CreateEncryption(_peer, Ss);
                Assert.NotNull(netSRPTest);
            }
        }

        private static void RunAlgorithmTest(NetEncryption encryption)
        {
            var outgoingMessage = _peer.CreateMessage();
            outgoingMessage.Write("Well hello there!");
            outgoingMessage.Write(42);
            outgoingMessage.Write(5, 5);
            outgoingMessage.Write(true);
            outgoingMessage.Write("General Kenobi!");
            var unencryptedLength = outgoingMessage.LengthBits;

            var outgoingMessageData = outgoingMessage.Data;
            outgoingMessage.Encrypt(encryption);

            // Convert to incoming message
            var incomingMessage = HelperMethods.CreateIncomingMessage(outgoingMessage.PeekDataBuffer(), outgoingMessage.LengthBits);

            Assert.NotNull(incomingMessage);
            Assert.NotEmpty(incomingMessage.Data);

            incomingMessage.Decrypt(encryption);
            var incomingMessageData = incomingMessage.Data;

            Assert.Equal(outgoingMessageData, incomingMessageData);

            Assert.NotNull(incomingMessage.Data);
            Assert.NotEmpty(incomingMessage.Data);
            Assert.Equal(unencryptedLength, incomingMessage.LengthBits);

            var msgFirstString = incomingMessage.ReadString();
            var msgFirstInt = incomingMessage.ReadInt32();
            var msgSecondInt = incomingMessage.ReadInt32(5);
            var msgBool = incomingMessage.ReadBoolean();
            var msgSecondString = incomingMessage.ReadString();

            Assert.Equal("Well hello there!", msgFirstString);
            Assert.Equal(42, msgFirstInt);
            Assert.Equal(5, msgSecondInt);
            Assert.True(msgBool);
            Assert.Equal("General Kenobi!", msgSecondString);
        }
    }
}
