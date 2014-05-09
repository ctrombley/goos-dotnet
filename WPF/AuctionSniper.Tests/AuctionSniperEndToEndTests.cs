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
			_auction.HasReceivedJoinRequestFrom(ApplicationRunner.SniperXmppId);

			_auction.AnnounceClosed();
			_application.ShowsSniperHasLostAuction();
		}

		[Test]
		public void SniperMakesAHigherBidButLoses()
		{
			_auction.StartSellingItem();	

			_application.StartBiddingIn(_auction);
			_auction.HasReceivedJoinRequestFrom(ApplicationRunner.SniperXmppId);

			_auction.ReportPrice(1000, 98, "other bidder");
			_application.HasShownSniperIsBidding();

			_auction.HasReceivedBid(1098, ApplicationRunner.SniperXmppId);

			_auction.AnnounceClosed();
			_application.ShowsSniperHasLostAuction();
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
