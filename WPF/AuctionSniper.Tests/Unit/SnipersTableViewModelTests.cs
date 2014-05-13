using AuctionSniperApplication.UI;
using NUnit.Framework;

namespace AuctionSniperApplication.Tests.Unit
{
	[TestFixture]
	public class SnipersTableViewModelTests
	{
		private SnipersTableViewModel _model;

		private static readonly string[] StatusText =
		{
			AuctionSniperConstants.StatusJoining, 
			AuctionSniperConstants.StatusBidding, 
			AuctionSniperConstants.StatusWinning,
			AuctionSniperConstants.StatusLost,
			AuctionSniperConstants.StatusWon
		};

		[SetUp]
		public void SetUp()
		{
			_model = new SnipersTableViewModel();	
		}

		[Test]
		public void SetsSniperValuesInCollection()
		{
			var joining = SniperSnapshot.Joining("Item1");
			var bidding = joining.Bidding(555, 777);

			_model.Add(joining);

			_model.SniperStateChanged(bidding);

			AssertSniperAtIndexMatchesSnapshot(0, bidding);
		}

		[Test]
		public void HoldsSnipersInAdditionOrder()
		{
			_model.Add(SniperSnapshot.Joining("Item1"));
			_model.Add(SniperSnapshot.Joining("Item2"));

			Assert.That(_model.Snipers[0].ItemId, Is.EqualTo("Item1"));
			Assert.That(_model.Snipers[1].ItemId, Is.EqualTo("Item2"));
		}

		private void AssertSniperAtIndexMatchesSnapshot(int index, SniperSnapshot snapshot)
		{
			var sniper = _model.Snipers[index];

			Assert.That(sniper.ItemId, Is.EqualTo("Item1"));
			Assert.That(sniper.LastPrice, Is.EqualTo(555));
			Assert.That(sniper.LastBid, Is.EqualTo(777));
			Assert.That(sniper.Status, Is.EqualTo(StatusText[(int)snapshot.Status]));
		}
	}
}
