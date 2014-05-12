namespace AuctionSniperApplication
{
	public class AuctionSniper : IAuctionEventListener
	{
		private readonly ISniperListener _listener;
		private readonly IAuction _auction;
		private bool _isWinning;
		private readonly string _itemId;
		private SniperSnapshot _snapshot;

		public AuctionSniper(string itemId, IAuction auction, ISniperListener listener)
		{
			_itemId = itemId;
			_auction = auction;
			_listener = listener;
			_snapshot = SniperSnapshot.Joining(_itemId);
		}

		public void AuctionClosed()
		{
			_snapshot = SniperSnapshot.Closed(_snapshot);
			NotifyChange();
		}

		public void CurrentPrice(int price, int increment, PriceSource source)
		{
			switch (source) {
				case PriceSource.FromSniper:
					_snapshot = SniperSnapshot.Winning(_itemId, price);
					break;
				case PriceSource.FromOtherBidder:
					int bid = price + increment;
					_auction.Bid(bid);
					_snapshot = SniperSnapshot.Bidding(_itemId, price, bid);
					break;
			}

			NotifyChange();
		}

		private void NotifyChange()
		{
			_listener.SniperStateChanged(_snapshot);
		}
	}
}