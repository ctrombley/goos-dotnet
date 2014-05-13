using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Is = NUnit.Framework.Is;

namespace AuctionSniperApplication.Tests.Unit
{
	[TestFixture]
	public class AuctionSniperTests
	{
		[SetUp]
		public void SetUp()
		{
			_listener = MockRepository.GenerateMock<ISniperListener>();
			_auction = MockRepository.GenerateMock<IAuction>();
			_sniper = new AuctionSniper(ItemId, _auction, _listener);

			_sniperState = null;
		}

		private ISniperListener _listener;
		private IAuction _auction;
		private AuctionSniper _sniper;
		private const string ItemId = "item-54321";

		private string _sniperState;

		[Test]
		public void BidsHigherAndReportsBiddingWhenNewPriceArrives()
		{
			const int price = 1001;
			const int increment = 25;
			const int bid = price + increment;

			_sniper.CurrentPrice(price, increment, PriceSource.FromOtherBidder);

			_auction.AssertWasCalled(l => l.Bid(bid), options => options.Repeat.Once());
			_listener.AssertWasCalled(l => l.SniperStateChanged(new SniperSnapshot(ItemId, price, bid, SniperState.Bidding)),
				options => options.Repeat.Once());
		}

		[Test]
		public void ReportsIsWinningWhenCurrentPriceComesFromSniper()
		{
			_listener.Expect(l => l.SniperStateChanged(null))
				.IgnoreArguments()
				.Constraints(Property.Value("Status", SniperState.Bidding))
				.WhenCalled(l => _sniperState = "bidding");

			_listener.Expect(l => l.SniperStateChanged(new SniperSnapshot(ItemId, 135, 135, SniperState.Winning)))
				.Repeat.AtLeastOnce()
				.WhenCalled(l => _sniperState = "bidding");

			_sniper.CurrentPrice(123, 12, PriceSource.FromOtherBidder);
			_sniper.CurrentPrice(123, 45, PriceSource.FromSniper);
		}

		[Test]
		public void ReportsLostIfAuctionClosesWhenBidding()
		{
			_listener.Expect(l => l.SniperStateChanged(null))
				.IgnoreArguments()
				.Constraints(Property.Value("Status", SniperState.Bidding))
				.WhenCalled(l => _sniperState = "bidding");

			_listener.Expect(l => l.SniperStateChanged(new SniperSnapshot(ItemId, 123, 123, SniperState.Bidding)))
				.WhenCalled(l => Assert.That(_sniperState, Is.EqualTo("bidding")));

			_sniper.CurrentPrice(123, 45, PriceSource.FromOtherBidder);

			_sniper.AuctionClosed();

			_listener.AssertWasCalled(l => l.SniperStateChanged(new SniperSnapshot(ItemId, 123, 168, SniperState.Lost)), options => options.Repeat.AtLeastOnce());
		}

		[Test]
		public void ReportsLostWhenAuctionClosesImmediately()
		{
			_sniper.AuctionClosed();

			_listener.AssertWasCalled(l => l.SniperStateChanged(new SniperSnapshot(ItemId, 0, 0, SniperState.Lost)), options => options.Repeat.Once());
		}

		[Test]
		public void ReportsWonIfAuctionClosesWhenWinning()
		{
			_listener.Expect(l => l.SniperStateChanged(null))
				.IgnoreArguments()
				.Constraints(Property.Value("Status", SniperState.Winning))
				.WhenCalled(l => _sniperState = "winning");

			_listener.Expect(l => l.SniperStateChanged(new SniperSnapshot(ItemId, 123, 123, SniperState.Won)))
				.WhenCalled(l => Assert.That(_sniperState, Is.EqualTo("winning")));

			_sniper.CurrentPrice(123, 12, PriceSource.FromOtherBidder);
			_sniper.CurrentPrice(135, 45, PriceSource.FromSniper);

			_sniper.AuctionClosed();

			_listener.AssertWasCalled(l => l.SniperStateChanged(new SniperSnapshot(ItemId, 135, 135, SniperState.Won)), 
				options => options.Repeat.AtLeastOnce());
		}
	}
}