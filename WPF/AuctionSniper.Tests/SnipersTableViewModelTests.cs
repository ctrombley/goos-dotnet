using System.Linq;
using AuctionSniperApplication.UI;
using NUnit.Framework;

namespace AuctionSniperApplication.Tests
{
	[TestFixture]
	public class SnipersTableViewModelTests
	{
		private SnipersTableViewModel _model;

		[SetUp]
		public void SetUp()
		{
			_model = new SnipersTableViewModel();	
		}

		[Test]
		[TestCase(SniperState.Bidding, AuctionSniperConstants.StatusBidding)]
		[TestCase(SniperState.Winning, AuctionSniperConstants.StatusWinning)]
		[TestCase(SniperState.Lost, AuctionSniperConstants.StatusLost)]
		[TestCase(SniperState.Won, AuctionSniperConstants.StatusWon)]
		public void SetsSniperValuesInCollection(SniperState sniperState, string expectedStatus)
		{
			_model.SniperStateChanged(new SniperSnapshot("item id", 555, 777, sniperState));

			Assert.That(_model.First().ItemId, Is.EqualTo("item id"));
			Assert.That(_model.First().LastPrice, Is.EqualTo(555));
			Assert.That(_model.First().LastBid, Is.EqualTo(777));
			Assert.That(_model.First().Status, Is.EqualTo(expectedStatus));
		}
	}
}
