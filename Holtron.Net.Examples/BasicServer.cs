using Holtron.Net.Network;

namespace Holtron.Net.Examples
{
    public class BasicServer
    {
        public void RunServer()
        {
            var config = new NetPeerConfiguration("basic");
            var server = new NetServer(config);
            config.Port = 1234;
            config.MaximumConnections = 100;
            server.Start();

            while (true)
            {
                NetIncomingMessage incomingMsg;
                while((incomingMsg = server.ReadMessage()) != null)
                {
                    switch (incomingMsg.MessageType)
                    {
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            string text = incomingMsg.ReadString();
                            Console.WriteLine(text);
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)incomingMsg.ReadByte();

                            string reason = incomingMsg.ReadString();
                            Console.WriteLine(NetUtility.ToHexString(incomingMsg.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

                            if (status == NetConnectionStatus.Connected)
                                Console.WriteLine("Remote hail: " + incomingMsg.SenderConnection.RemoteHailMessage.ReadString());
                            break;
                        case NetIncomingMessageType.Data:
                            // incoming chat message from a client
                            string chat = incomingMsg.ReadString();

                            Console.WriteLine("Broadcasting '" + chat + "'");

                            // broadcast this to all connections, except sender
                            List<NetConnection> all = server.Connections; // get copy
                            all.Remove(incomingMsg.SenderConnection);

                            if (all.Count > 0)
                            {
                                NetOutgoingMessage om = server.CreateMessage();
                                om.Write(NetUtility.ToHexString(incomingMsg.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
                                server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                            }
                            break;
                        default:
                            Console.WriteLine("Unhandled type: " + incomingMsg.MessageType + " " + incomingMsg.LengthBytes + " bytes " + incomingMsg.DeliveryMethod + "|" + incomingMsg.SequenceChannel);
                            break;
                    }
                    server.Recycle(incomingMsg);
                }
            }
        }
    }
}
