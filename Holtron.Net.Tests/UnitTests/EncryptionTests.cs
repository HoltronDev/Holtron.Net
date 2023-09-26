using Holtron.Net.Network;
using Holtron.Net.Network.Encryption;

namespace Holtron.Net.Tests.UnitTests
{
    public class EncryptionTests
    {
        private static NetPeer _peer = new NetPeer(new NetPeerConfiguration("test"));

        [Fact]
        public void NetEncryptionAesGcmTest()
        {
            RunAlgorithmTest(new NetEncryptionAESGCM("TopSecret"));
        }

        private static void RunAlgorithmTest(INetEncryption encryption)
        {
            var outgoingMessage = _peer.CreateMessage();
            outgoingMessage.Write("Well hello there!");
            outgoingMessage.Write(42);
            outgoingMessage.Write(5, 5);
            outgoingMessage.Write(true);
            outgoingMessage.Write("General Kenobi!");
            var unencryptedLength = outgoingMessage.LengthBits;

            var outgoingMessageData = new byte[outgoingMessage.Data.Length];
            Array.Copy(outgoingMessage.Data, outgoingMessageData, outgoingMessage.Data.Length);
            encryption.Encrypt(outgoingMessage);

            // Convert to incoming message
            var incomingMessage = HelperMethods.CreateIncomingMessage(outgoingMessage.PeekDataBuffer(), outgoingMessage.LengthBits);

            Assert.NotNull(incomingMessage);
            Assert.NotEmpty(incomingMessage.Data);

            encryption.Decrypt(incomingMessage);
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
