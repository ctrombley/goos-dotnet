using System;

namespace AuctionSniperApplication.UI
{
	public class SniperSnapshotViewModel : BindableBase
	{
		private static readonly string[] StatusText =
		{
			AuctionSniperConstants.StatusJoining, 
			AuctionSniperConstants.StatusBidding, 
			AuctionSniperConstants.StatusWinning,
			AuctionSniperConstants.StatusLost,
			AuctionSniperConstants.StatusWon
		};

		public SniperSnapshotViewModel(SniperSnapshot snapshot)
		{
			Update(snapshot);
		}

		public void Update(SniperSnapshot newSnapshot)
		{
			ItemId = newSnapshot.ItemId;
			LastBid = newSnapshot.LastBid;
			LastPrice = newSnapshot.LastPrice;
			Status = StatusText[(int) newSnapshot.Status];
		}

		private String _itemId;
		private int _lastBid;
		private int _lastPrice;
		private	string _status;

		public string Status
		{
			get { return _status; }
			set { SetProperty(ref _status, value); }
		}

		public String ItemId
		{
			get { return _itemId; }
			set { SetProperty(ref _itemId, value); }
		}

		public int LastPrice
		{
			get { return _lastPrice; }
			set { SetProperty(ref _lastPrice, value); }
		}

		public int LastBid
		{
			get { return _lastBid; }
			set { SetProperty(ref _lastBid, value); }
		}
	}
}