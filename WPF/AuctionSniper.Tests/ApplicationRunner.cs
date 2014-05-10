using System;
using System.Diagnostics;
using agsXMPP;
using TestStack.White;

namespace AuctionSniperApplication.Tests
{
	public class ApplicationRunner
	{
		public const string AppPath = @"..\..\..\AuctionSniper\bin\Debug\AuctionSniperApplication.exe";
		public const string XmppServer = "localhost";
		public const string Username = "sniper";
		public const string Password = "sniper";
		public const string AuctionResource = "Auction";

		private AuctionSniperDriver _driver;

		public static Jid SniperJid = new Jid(Username, XmppServer, AuctionResource);

		public void StartBiddingIn(FakeAuctionServer auction)
		{
			var app = Application.Launch(new ProcessStartInfo(AppPath, String.Format("{0} {1} {2} {3}", XmppServer, Username, Password, auction.ItemId)));
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

		public void HasShownSniperIsBidding()
		{
			_driver.ShowsSniperStatus(AuctionSniperConstants.StatusBidding);
		}

		public void HasShownSniperIsWinning()
		{
			_driver.ShowsSniperStatus(AuctionSniperConstants.StatusWinning);
		}

		public void ShowsSniperHasWonAuction()
		{
			_driver.ShowsSniperStatus(AuctionSniperConstants.StatusWon);
		}
	}
}