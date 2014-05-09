using NUnit.Framework;
using Rhino.Mocks;

namespace AuctionSniperApplication.Tests
{
	[TestFixture]
	public class AuctionSniperTests
	{
		private ISniperListener _listener;
		private IAuction _auction;
		private AuctionSniper _sniper;

		[SetUp]
		public void SetUp()
		{
			_listener = MockRepository.GenerateMock<ISniperListener>();
			_auction = MockRepository.GenerateMock<IAuction>();
			_sniper = new AuctionSniper(_auction, _listener);
		}

		[Test]
		public void ReportsLostWhenAuctionCloses()
		{
			_sniper.AuctionClosed();
	
			_listener.AssertWasCalled(l => l.SniperLost(), options => options.Repeat.Once());
		}

		[Test]
		public void BidsHigherAndReportsBiddingWhenNewPriceArrives()
		{
			const int price = 1001;
			const int increment = 25;

			_sniper.CurrentPrice(price, increment);

			_auction.AssertWasCalled(l => l.Bid(price + increment), options => options.Repeat.Once());
			_listener.AssertWasCalled(l => l.SniperBidding(), options => options.Repeat.Once());
		}
	}
}
