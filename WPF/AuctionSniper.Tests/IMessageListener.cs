using agsXMPP.protocol.client;

namespace AuctionSniperApplication.Tests
{
	public interface IMessageListener
	{
		void ProcessMessage(Message message);
	}
}