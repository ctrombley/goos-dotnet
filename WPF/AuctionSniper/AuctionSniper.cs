namespace AuctionSniperApplication
{
	public class AuctionSniper : IAuctionEventListener
	{
		private readonly ISniperListener _listener;
		private readonly IAuction _auction;

		public AuctionSniper(IAuction auction, ISniperListener listener)
		{
			_auction = auction;
			_listener = listener;
		}

		public void AuctionClosed()
		{
			_listener.SniperLost();
		}

		public void CurrentPrice(int price, int increment)
		{
			_auction.Bid(price + increment);	
			_listener.SniperBidding();
		}
	}
}