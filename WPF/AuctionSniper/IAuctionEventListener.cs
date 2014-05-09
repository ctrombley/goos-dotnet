namespace AuctionSniperApplication
{
	public interface IAuctionEventListener
	{
		void AuctionClosed();
		void CurrentPrice(int price, int increment);
	}
}