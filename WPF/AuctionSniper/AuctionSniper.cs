using System;

namespace AuctionSniperApplication
{
	public class AuctionSniper : IAuctionEventListener
	{
		private readonly ISniperListener _listener;
		private readonly IAuction _auction;
		private bool _isWinning;

		public AuctionSniper(IAuction auction, ISniperListener listener)
		{
			_auction = auction;
			_listener = listener;
		}

		public void AuctionClosed()
		{
			if (_isWinning)
			{
				_listener.SniperWon();
			}
			else
			{
				_listener.SniperLost();
			}
		}

		public void CurrentPrice(int price, int increment, PriceSource source)
		{
			_isWinning = source == PriceSource.FromSniper;

			switch (source)
			{
				case PriceSource.FromOtherBidder:
					_auction.Bid(price + increment);	
					_listener.SniperBidding();
					break;
				case PriceSource.FromSniper:
					_listener.SniperWinning();
					break;
			}
		}
	}
}