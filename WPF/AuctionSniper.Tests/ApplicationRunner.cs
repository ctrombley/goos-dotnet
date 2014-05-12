using System;
using System.Diagnostics;
using System.Text;
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
		private string _itemId;

		public static Jid SniperJid = new Jid(Username, XmppServer, AuctionResource);

		public void StartBiddingIn(FakeAuctionServer auction)
		{
			_itemId = auction.ItemId;

			var app = Application.Launch(new ProcessStartInfo(AppPath, String.Format("{0} {1} {2} {3}", XmppServer, Username, Password, auction.ItemId)));
			_driver = new AuctionSniperDriver(app);
			_driver.HasTitle(AuctionSniperConstants.MainWindowName);
			_driver.HasColumnTitles();
			_driver.ShowsSniperStatus("", 0, 0, AuctionSniperConstants.StatusJoining);

		}

		public void Stop()
		{
			if (_driver != null)
			{
				_driver.Dispose();
			}
		}

		public void ShowsSniperHasLostAuction()
		{
			_driver.ShowsSniperStatus(AuctionSniperConstants.StatusLost);
		}

		public void HasShownSniperIsBidding(int lastPrice, int lastBid)
		{
			_driver.ShowsSniperStatus(_itemId, lastPrice, lastBid, AuctionSniperConstants.StatusBidding);
		}

		public void HasShownSniperIsWinning(int winningBid)
		{
			_driver.ShowsSniperStatus(_itemId, winningBid, winningBid, AuctionSniperConstants.StatusWinning);
		}

		public void ShowsSniperHasWonAuction(int lastPrice)
		{
			_driver.ShowsSniperStatus(_itemId, lastPrice, lastPrice, AuctionSniperConstants.StatusWon);
		}
	}
}