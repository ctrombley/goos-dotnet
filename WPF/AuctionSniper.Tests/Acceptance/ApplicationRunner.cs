using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.Base;
using TestStack.White;

namespace AuctionSniperApplication.Tests.Acceptance
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

		public void StartBiddingIn(params FakeAuctionServer[] auctions)
		{
			var app = Application.Launch(new ProcessStartInfo(AppPath, String.Format("{0} {1} {2}", XmppServer, Username, Password)));
			_driver = new AuctionSniperDriver(app);
			_driver.HasTitle(AuctionSniperConstants.MainWindowName);
			_driver.HasColumnTitles();

			foreach (var auction in auctions)
			{
				var itemId = auction.ItemId;
				_driver.StartBiddingFor(itemId);	
				_driver.ShowsSniperStatus(itemId, 0, 0, AuctionSniperConstants.StatusJoining);
			}
		}

		public void Stop()
		{
			if (_driver != null)
			{
				_driver.Dispose();
			}
		}

		public void HasShownSniperHasLostAuction(FakeAuctionServer auction, int lastPrice, int lastBid)
		{
			_driver.ShowsSniperStatus(auction.ItemId, lastPrice, lastBid, AuctionSniperConstants.StatusLost);
		}

		public void HasShownSniperIsBidding(FakeAuctionServer auction, int lastPrice, int lastBid)
		{
			_driver.ShowsSniperStatus(auction.ItemId, lastPrice, lastBid, AuctionSniperConstants.StatusBidding);
		}

		public void HasShownSniperIsWinning(FakeAuctionServer auction, int winningBid)
		{
			_driver.ShowsSniperStatus(auction.ItemId, winningBid, winningBid, AuctionSniperConstants.StatusWinning);
		}

		public void ShowsSniperHasWonAuction(FakeAuctionServer auction, int lastPrice)
		{
			_driver.ShowsSniperStatus(auction.ItemId, lastPrice, lastPrice, AuctionSniperConstants.StatusWon);
		}
	}
}