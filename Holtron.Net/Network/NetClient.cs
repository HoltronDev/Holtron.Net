#if !__NOIPENDPOINT__
using NetEndPoint = System.Net.IPEndPoint;
#endif

namespace Holtron.Net.Network
{
    /// <summary>
    /// Specialized version of NetPeer used for a "client" connection. It does not accept any incoming connections and maintains a ServerConnection property
    /// </summary>
    public class NetClient : NetPeer
	{
		/// <summary>
		/// Gets the connection to the server, if any
		/// </summary>
		public NetConnection ServerConnection
		{
			get
			{
				NetConnection retval = null;
				if (m_connections.Count > 0)
				{
					try
					{
						retval = m_connections[0];
					}
					catch
					{
						// preempted!
						return null;
					}
				}
				return retval;
			}
		}

		/// <summary>
		/// Gets the connection status of the server connection (or NetConnectionStatus.Disconnected if no connection)
		/// </summary>
		public NetConnectionStatus ConnectionStatus
		{
			get
			{
				var conn = ServerConnection;
				if (conn == null)
					return NetConnectionStatus.Disconnected;
				return conn.Status;
			}
		}

		/// <summary>
		/// NetClient constructor
		/// </summary>
		/// <param name="config"></param>
		public NetClient(NetPeerConfiguration config)
			: base(config)
		{
			config.AcceptIncomingConnections = false;
		}

		/// <summary>
		/// Connect to a remote server
		/// </summary>
		/// <param name="remoteEndPoint">The remote endpoint to connect to</param>
		/// <param name="hailMessage">The hail message to pass</param>
		/// <returns>server connection, or null if already connected</returns>
		public override NetConnection Connect(NetEndPoint remoteEndPoint, NetOutgoingMessage hailMessage)
		{
			lock (m_connections)
			{
				if (m_connections.Count > 0)
				{
					LogWarning("Connect attempt failed; Already connected");
					return null;
				}
			}

			lock (m_handshakes)
			{
				if (m_handshakes.Count > 0)
				{
					LogWarning("Connect attempt failed; Handshake already in progress");
					return null;
				}
			}

			return base.Connect(remoteEndPoint, hailMessage);
		}

		/// <summary>
		/// Disconnect from server
		/// </summary>
		/// <param name="byeMessage">reason for disconnect</param>
		public void Disconnect(string byeMessage)
		{
			NetConnection serverConnection = ServerConnection;
			if (serverConnection == null)
			{
				lock (m_handshakes)
				{
					if (m_handshakes.Count > 0)
					{
						LogVerbose("Aborting connection attempt");
						foreach(var hs in m_handshakes)
							hs.Value.Disconnect(byeMessage);
						return;
					}
				}

				LogWarning("Disconnect requested when not connected!");
				return;
			}
			serverConnection.Disconnect(byeMessage);
		}

		/// <summary>
		/// Sends message to server
		/// </summary>
		public NetSendResult SendMessage(NetOutgoingMessage msg, NetDeliveryMethod method)
		{
			NetConnection serverConnection = ServerConnection;
			if (serverConnection == null)
			{
				LogWarning("Cannot send message, no server connection!");
				return NetSendResult.FailedNotConnected;
			}

			return serverConnection.SendMessage(msg, method, 0);
		}

		/// <summary>
		/// Sends message to server
		/// </summary>
		public NetSendResult SendMessage(NetOutgoingMessage msg, NetDeliveryMethod method, int sequenceChannel)
		{
			NetConnection serverConnection = ServerConnection;
			if (serverConnection == null)
			{
				LogWarning("Cannot send message, no server connection!");
				Recycle(msg);
				return NetSendResult.FailedNotConnected;
			}

			return serverConnection.SendMessage(msg, method, sequenceChannel);
		}

		/// <summary>
		/// Returns a string that represents this object
		/// </summary>
		public override string ToString()
		{
			return "[NetClient " + ServerConnection + "]";
		}

	}
}
