using System.Collections.ObjectModel;
using System.Linq;

namespace AuctionSniperApplication.UI
{
	public class SnipersTableViewModel : ObservableCollection<SniperSnapshotViewModel>, ISniperListener
	{
		private static readonly string[] StatusText =
		{
			AuctionSniperConstants.StatusJoining, 
			AuctionSniperConstants.StatusBidding, 
			AuctionSniperConstants.StatusWinning,
			AuctionSniperConstants.StatusLost,
			AuctionSniperConstants.StatusWon
		};

		private static readonly SniperSnapshotViewModel StartingUp = new SniperSnapshotViewModel
		{
			Status = AuctionSniperConstants.StatusJoining
		};

		public SnipersTableViewModel()
		{
			Add(StartingUp);
		}

		public void SniperStateChanged(SniperSnapshot newSnapshot)
		{
			var snapshot = this.First();

			snapshot.Status = StatusText[(int) newSnapshot.Status];
			snapshot.LastBid = newSnapshot.LastBid;
			snapshot.LastPrice = newSnapshot.LastPrice;
			snapshot.ItemId = newSnapshot.ItemId;
		}
	}
}