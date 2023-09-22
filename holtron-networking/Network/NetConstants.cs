namespace HoltronNetworking.Network
{
    /// <summary>
    /// All the constants used when compiling the library
    /// </summary>
    internal static class NetConstants
	{
		internal const int NumTotalChannels = 99;

		internal const int NetChannelsPerDeliveryMethod = 32;

		internal const int NumSequenceNumbers = 1024;

		internal const int HeaderByteSize = 5;

		internal const int UnreliableWindowSize = 128;
		internal const int ReliableOrderedWindowSize = 64;
		internal const int ReliableSequencedWindowSize = 64;
		internal const int DefaultWindowSize = 64;

		internal const int MaxFragmentationGroups = ushort.MaxValue - 1;

		internal const int UnfragmentedMessageHeaderSize = 5;

		/// <summary>
		/// Number of channels which needs a sequence number to work
		/// </summary>
		internal const int NumSequencedChannels = ((int)NetMessageType.UserReliableOrdered1 + NetConstants.NetChannelsPerDeliveryMethod) - (int)NetMessageType.UserSequenced1;

		/// <summary>
		/// Number of reliable channels
		/// </summary>
		internal const int NumReliableChannels = ((int)NetMessageType.UserReliableOrdered1 + NetConstants.NetChannelsPerDeliveryMethod) - (int)NetMessageType.UserReliableUnordered;
		
		internal const string ConnResetMessage = "Connection was reset by remote host";
	}
}
