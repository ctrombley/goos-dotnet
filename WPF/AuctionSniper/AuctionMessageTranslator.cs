using System;
using System.Diagnostics;
using agsXMPP;
using agsXMPP.protocol.client;

namespace AuctionSniperApplication
{
	public class AuctionMessageTranslator : IMessageListener
	{
		private readonly Jid _sniperId;
		private readonly IAuctionEventListener _listener;

		public AuctionMessageTranslator(Jid sniperId, IAuctionEventListener listener)
		{
			_sniperId = sniperId;
			_listener = listener;
		}

		public void ProcessMessage(Message message)
		{
			var auctionEvent = AuctionEventCreator.From(message.Body);

			Trace.WriteLine(String.Format("Event type: {0}", auctionEvent.Type));

			if ("CLOSE".Equals(auctionEvent.Type))
			{
				_listener.AuctionClosed();	
			}
			else if ("PRICE".Equals(auctionEvent.Type)) 
			{
				_listener.CurrentPrice(auctionEvent.CurrentPrice, 
					auctionEvent.Increment, auctionEvent.IsFrom(_sniperId));
			}
		}

	}
}