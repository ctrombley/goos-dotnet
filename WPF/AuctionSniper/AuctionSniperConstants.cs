﻿namespace AuctionSniperApplication
{
	public static class AuctionSniperConstants
	{
		public const string MainWindowName = "Auction Sniper Main";
		public const string AuctionResource = "Auction";

		public const string StatusJoining = "Joining";
		public const string StatusLost = "Lost";
		public const string StatusBidding = "Bidding";

		public const string SniperStatusName = "StatusLabel";

		public const string JoinCommandFormat = "SOLVersion: 1.1; Command: JOIN;";
		public const string BidCommandFormat = "SOLVersion: 1.1; Command: BID; Price: {0};";
	}
}