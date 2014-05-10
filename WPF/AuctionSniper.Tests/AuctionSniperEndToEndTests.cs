using NUnit.Framework;

namespace AuctionSniperApplication.Tests
{
	[TestFixture]
	public class AuctionSniperEndToEndTests
	{
		private readonly FakeAuctionServer _auction = new FakeAuctionServer("item-54321");
		private readonly ApplicationRunner _application = new ApplicationRunner();

		[Test]
		public void SniperJoinsAuctionUntilAuctionCloses()
		{
			_auction.StartSellingItem();

			_application.StartBiddingIn(_auction);
			_auction.HasReceivedJoinRequestFrom(ApplicationRunner.SniperJid);

			_auction.AnnounceClosed();
			_application.ShowsSniperHasLostAuction();
		}

		[Test]
		public void SniperMakesAHigherBidButLoses()
		{
			_auction.StartSellingItem();	

			_application.StartBiddingIn(_auction);
			_auction.HasReceivedJoinRequestFrom(ApplicationRunner.SniperJid);

			_auction.ReportPrice(1000, 98, "other bidder");
			_application.HasShownSniperIsBidding();

			_auction.HasReceivedBid(1098, ApplicationRunner.SniperJid);

			_auction.AnnounceClosed();
			_application.ShowsSniperHasLostAuction();
		}

		[Test]
		public void SniperWinsAnAuctionByBiddingHigher()
		{
			_auction.StartSellingItem();

			_application.StartBiddingIn(_auction);
			_auction.HasReceivedJoinRequestFrom(ApplicationRunner.SniperJid);

			_auction.ReportPrice(1000, 98, "other bidder");
			_application.HasShownSniperIsBidding();

			_auction.ReportPrice(1098, 97, ApplicationRunner.SniperJid);
			_application.HasShownSniperIsWinning();

			_auction.AnnounceClosed();
			_application.ShowsSniperHasWonAuction();
		}

		[SetUp]
		public void SetUp()
		{
			
		}

		[TearDown]
		public void StopAuction()
		{
			_auction.Stop();
		}

		[TearDown]
		public void StopApplication()
		{
			_application.Stop();
		}
	}

}
