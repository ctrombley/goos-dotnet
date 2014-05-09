using agsXMPP.protocol.client;

namespace AuctionSniperApplication
{
	public interface IMessageListener
	{
		void ProcessMessage(Message message);
	}
}