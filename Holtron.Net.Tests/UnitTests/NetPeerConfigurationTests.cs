using Holtron.Net.Network;

namespace Holtron.Net.Tests.UnitTests
{
    public class NetPeerConfigurationTests
    {
        [Theory]
        [InlineData(NetIncomingMessageType.ConnectionApproval)]
        [InlineData(NetIncomingMessageType.ConnectionLatencyUpdated)]
        [InlineData(NetIncomingMessageType.Data)]
        [InlineData(NetIncomingMessageType.DebugMessage)]
        [InlineData(NetIncomingMessageType.DiscoveryRequest)]
        [InlineData(NetIncomingMessageType.DiscoveryResponse)]
        //[InlineData(NetIncomingMessageType.Error)] While this is part of it, apparently it shouldn't ever occur?
        [InlineData(NetIncomingMessageType.ErrorMessage)]
        [InlineData(NetIncomingMessageType.NatIntroductionSuccess)]
        [InlineData(NetIncomingMessageType.Receipt)]
        [InlineData(NetIncomingMessageType.StatusChanged)]
        [InlineData(NetIncomingMessageType.UnconnectedData)]
        [InlineData(NetIncomingMessageType.VerboseDebugMessage)]
        [InlineData(NetIncomingMessageType.WarningMessage)]
        public void EnableAndDisableMessageTypes(NetIncomingMessageType msgType)
        {
            var config = new NetPeerConfiguration("test");

            config.SetMessageTypeEnabled(msgType, false);

            Assert.False(config.IsMessageTypeEnabled(msgType));

            config.SetMessageTypeEnabled(msgType, true);

            Assert.True(config.IsMessageTypeEnabled(msgType));

            config.EnableMessageType(msgType);

            Assert.True(config.IsMessageTypeEnabled(msgType));

            config.DisableMessageType(msgType);

            Assert.False(config.IsMessageTypeEnabled(msgType));
        }
    }
}
