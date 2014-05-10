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

		private string _sniperState;

		[SetUp]
		public void SetUp()
		{
			_listener = MockRepository.GenerateMock<ISniperListener>();
			_auction = MockRepository.GenerateMock<IAuction>();
			_sniper = new AuctionSniper(_auction, _listener);

			_sniperState = null;
		}

		[Test]
		public void ReportsLostWhenAuctionClosesImmediately()
		{
			_sniper.AuctionClosed();
	
			_listener.AssertWasCalled(l => l.SniperLost(), options => options.Repeat.Once());
		}

		[Test]
		public void ReportsLostIfAuctionClosesWhenBidding()
		{
			_listener.Expect(l => l.SniperBidding()).WhenCalled(l => _sniperState = "bidding");
			_listener.Expect(l => l.SniperLost()).WhenCalled(l => Assert.That(_sniperState, Is.EqualTo("bidding")));

			_sniper.CurrentPrice(123, 45, PriceSource.FromOtherBidder);

			_sniper.AuctionClosed();

			_listener.AssertWasCalled(l => l.SniperLost(), options => options.Repeat.Once());
		}

		[Test]
		public void ResportsWonIfAuctionClosesWhenWinning()
		{
			_listener.Expect(l => l.SniperWinning()).WhenCalled(l => _sniperState = "winning");
			_listener.Expect(l => l.SniperWon()).WhenCalled(l => Assert.That(_sniperState, Is.EqualTo("winning")));

			_sniper.CurrentPrice(123, 45, PriceSource.FromSniper);

			_sniper.AuctionClosed();

			_listener.AssertWasCalled(l => l.SniperWon(), options => options.Repeat.Once());
		}

		[Test]
		public void BidsHigherAndReportsBiddingWhenNewPriceArrives()
		{
			const int price = 1001;
			const int increment = 25;

			_sniper.CurrentPrice(price, increment, PriceSource.FromOtherBidder);

			_auction.AssertWasCalled(l => l.Bid(price + increment), options => options.Repeat.Once());
			_listener.AssertWasCalled(l => l.SniperBidding(), options => options.Repeat.Once());
		}

		[Test]
		public void ReportsIsWinningWhenCurrentPriceComesFromSniper()
		{
			_sniper.CurrentPrice(123, 45, PriceSource.FromSniper);	

			_listener.AssertWasCalled(l => l.SniperWinning(), options => options.Repeat.AtLeastOnce());
		}
	}
}
