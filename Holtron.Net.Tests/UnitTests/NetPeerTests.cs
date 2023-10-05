using Holtron.Net.Network;

namespace Holtron.Net.Tests.UnitTests
{
    public class NetPeerTests
    {
        [Fact]
        public void Start_InitializesNetPeer_WhenCalled()
        {
            var config = new NetPeerConfiguration("test");
            var peer = new NetPeer(config);
            peer.Start();

            Assert.True(peer.Status == NetPeerStatus.Running);
        }

        [Fact]
        public void ReadMessage_ReturnsAMessage_WhenMessageInQueue()
        {
            var config = new NetPeerConfiguration("test");
            var peer = new NetPeer(config);
            peer.Start();

            var message = peer.ReadMessage();

            Assert.NotNull(message);
        }

        [Fact]
        public void ReadMessageOutNetIncomingMessage_ReturnsMessageToOutParam_WhenCalled()
        {
            var config = new NetPeerConfiguration("test");
            var peer = new NetPeer(config);
            peer.Start();

            peer.ReadMessage(out var message);

            Assert.NotNull(message);
        }

        [Fact]
        public void ReadMessages_ReadsAllQueuedMessage_WhenCalled()
        {
            var config = new NetPeerConfiguration("test");
            var peer = new NetPeer(config);
            peer.Start();

            var messages = new List<NetIncomingMessage>();
            peer.ReadMessages(messages);

            Assert.NotNull(messages);
            Assert.NotEmpty(messages);
        }
    }
}
