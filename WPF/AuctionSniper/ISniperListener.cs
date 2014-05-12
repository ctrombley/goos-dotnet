namespace AuctionSniperApplication
{
	public interface ISniperListener
	{
		void SniperStateChanged(SniperSnapshot snapshot);
	}
}