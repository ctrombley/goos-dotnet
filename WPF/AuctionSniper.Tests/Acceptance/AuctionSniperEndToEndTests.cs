using NUnit.Framework;

namespace AuctionSniperApplication.Tests.Acceptance
{
	[TestFixture]
	public class AuctionSniperEndToEndTests
	{
		private readonly FakeAuctionServer _auction1 = new FakeAuctionServer("item-54321");
		private readonly FakeAuctionServer _auction2 = new FakeAuctionServer("item-65432");
		private readonly ApplicationRunner _application = new ApplicationRunner();

		[Test]
		public void SniperJoinsAuctionUntilAuctionCloses()
		{
			_auction1.StartSellingItem();

			_application.StartBiddingIn(_auction1);
			_auction1.HasReceivedJoinRequestFrom(ApplicationRunner.SniperJid);

			_auction1.AnnounceClosed();
			_application.HasShownSniperHasLostAuction(_auction1, 0, 0);
		}

		[Test]
		public void SniperMakesAHigherBidButLoses()
		{
			_auction1.StartSellingItem();	

			_application.StartBiddingIn(_auction1);
			_auction1.HasReceivedJoinRequestFrom(ApplicationRunner.SniperJid);

			_auction1.ReportPrice(1000, 98, "other bidder");
			_auction1.HasReceivedBid(1098, ApplicationRunner.SniperJid);
			_application.HasShownSniperIsBidding(_auction1, 1000, 1098);

			_auction1.AnnounceClosed();
			_application.HasShownSniperHasLostAuction(_auction1, 1000, 1098);
		}

		[Test]
		public void SniperWinsAnAuctionByBiddingHigher()
		{
			_auction1.StartSellingItem();

			_application.StartBiddingIn(_auction1);
			_auction1.HasReceivedJoinRequestFrom(ApplicationRunner.SniperJid);

			_auction1.ReportPrice(1000, 98, "other bidder");
			_auction1.HasReceivedBid(1098, ApplicationRunner.SniperJid);
			_application.HasShownSniperIsBidding(_auction1, 1000, 1098);

			_auction1.ReportPrice(1098, 97, ApplicationRunner.SniperJid);
			_application.HasShownSniperIsWinning(_auction1, 1098);

			_auction1.AnnounceClosed();
			_application.ShowsSniperHasWonAuction(_auction1, 1098);
		}

		[Test]
		public void SniperBidsForMultipleItems()
		{
			_auction1.StartSellingItem();
			_auction2.StartSellingItem();

			_application.StartBiddingIn(_auction1, _auction2);
			_auction1.HasReceivedJoinRequestFrom(ApplicationRunner.SniperJid);
			_auction2.HasReceivedJoinRequestFrom(ApplicationRunner.SniperJid);

			_auction1.ReportPrice(1000, 98, "other bidder");
			_auction1.HasReceivedBid(1098, ApplicationRunner.SniperJid);
			_application.HasShownSniperIsBidding(_auction1, 1000, 1098);

			_auction2.ReportPrice(500, 21, "other bidder");
			_auction2.HasReceivedBid(521, ApplicationRunner.SniperJid);
			_application.HasShownSniperIsBidding(_auction2, 500, 521);

			_auction1.ReportPrice(1098, 97, ApplicationRunner.SniperJid);
			_auction2.ReportPrice(521, 22, ApplicationRunner.SniperJid);

			_application.HasShownSniperIsWinning(_auction1, 1098);
			_application.HasShownSniperIsWinning(_auction2, 521);

			_auction1.AnnounceClosed();
			_auction2.AnnounceClosed();

			_application.ShowsSniperHasWonAuction(_auction1, 1098);
			_application.ShowsSniperHasWonAuction(_auction2, 521);
		}

		[SetUp]
		public void SetUp()
		{
			
		}

		[TearDown]
		public void StopAuction()
		{
			_auction1.Stop();
			_auction2.Stop();
		}

		[TearDown]
		public void StopApplication()
		{
			_application.Stop();
		}
	}

}
