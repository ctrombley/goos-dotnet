using System;
using System.Diagnostics;
using TestStack.White;

namespace AuctionSniperApplication.Tests
{
	public class ApplicationRunner
	{
		public const string AppPath = @"..\..\..\AuctionSniper\bin\Debug\AuctionSniperApplication.exe";
		public const string XmppServer = "localhost";
		public const string SniperId = "sniper";
		public const string SniperPassword = "sniper";
		public const string AuctionResource = "Auction";

		private AuctionSniperDriver _driver;

		public void StartBiddingIn(FakeAuctionServer auction)
		{
			var app = Application.Launch(new ProcessStartInfo(AppPath, String.Format("{0} {1} {2} {3}", XmppServer, SniperId, SniperPassword, auction.ItemId)));
			_driver = new AuctionSniperDriver(app);
			_driver.ShowsSniperStatus(AuctionSniperConstants.StatusJoining);
		}

		public void ShowsSniperHasLostAuction()
		{
			_driver.ShowsSniperStatus(AuctionSniperConstants.StatusLost);
		}

		public void Stop()
		{
			if (_driver != null)
			{
				_driver.Dispose();
			}
		}

		public static string SniperXmppId
		{
			get { return String.Format("{0}@{1}/{2}", SniperId, XmppServer, AuctionResource); }
		}

		public void HasShownSniperIsBidding()
		{
			_driver.ShowsSniperStatus(AuctionSniperConstants.StatusBidding);
		}
	}
}