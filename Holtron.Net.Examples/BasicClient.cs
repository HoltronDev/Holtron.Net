using Holtron.Net.Network;

namespace Holtron.Net.Examples
{
    public class BasicClient
    {
        public void RunClient()
        {
            var config = new NetPeerConfiguration("basic");
            var client = new NetClient(config);
            client.Start();

            var msg = client.CreateMessage("Howdy!");
            client.Connect("localhost", 1234, msg);

            Task.Run(RunMessageQueue(client));

            while (true)
            {
                var clientMessage = Console.ReadLine();
                var outgoingClientMsg = client.CreateMessage($"{clientMessage}");
                client.SendMessage(outgoingClientMsg, NetDeliveryMethod.ReliableOrdered);
            }
        }

        private Action RunMessageQueue(NetClient client)
        {
            NetIncomingMessage incomingMsg;
            while (true)
            {
                while ((incomingMsg = client.ReadMessage()) != null)
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

                            if (status == NetConnectionStatus.Connected)
                                Console.WriteLine("Connected!");

                            if (status == NetConnectionStatus.Disconnected)
                                Console.WriteLine("Disconnected. :(");

                            string reason = incomingMsg.ReadString();
                            Console.WriteLine(status.ToString() + ": " + reason);

                            break;
                        case NetIncomingMessageType.Data:
                            string chat = incomingMsg.ReadString();
                            Console.WriteLine(chat);
                            break;
                        default:
                            Console.WriteLine("Unhandled type: " + incomingMsg.MessageType + " " + incomingMsg.LengthBytes + " bytes");
                            break;
                    }
                    client.Recycle(incomingMsg);
                }
            }
        }
    }
}
