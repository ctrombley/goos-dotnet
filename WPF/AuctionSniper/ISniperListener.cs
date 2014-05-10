namespace AuctionSniperApplication
{
	public interface ISniperListener
	{
		void SniperLost();
		void SniperBidding();
		void SniperWinning();
		void SniperWon();
	}
}