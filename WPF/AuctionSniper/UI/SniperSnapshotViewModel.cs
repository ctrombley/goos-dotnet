using System;

namespace AuctionSniperApplication.UI
{
	public class SniperSnapshotViewModel : BindableBase
	{
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