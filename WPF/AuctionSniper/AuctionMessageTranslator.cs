using System;
using System.Collections.Generic;
using System.Diagnostics;
using agsXMPP.protocol.client;

namespace AuctionSniperApplication
{
	public class AuctionMessageTranslator : IMessageListener
	{
		private readonly IAuctionEventListener _listener;

		public AuctionMessageTranslator(IAuctionEventListener listener)
		{
			_listener = listener;
		}

		public void ProcessMessage(Message message)
		{
			var auctionEvent = UnpackEventFrom(message);
			var type = auctionEvent["Event"];

			Trace.WriteLine(String.Format("Event type: {0}", type));

			if ("CLOSE".Equals(type))
			{
				_listener.AuctionClosed();	
			}
			else if ("PRICE".Equals(type)) 
			{
				_listener.CurrentPrice(Int32.Parse(auctionEvent["CurrentPrice"]), 
					Int32.Parse(auctionEvent["Increment"]));
			}
		}

		private Dictionary<string, string> UnpackEventFrom(Message message)
		{
			var auctionEvent = new Dictionary<string, string>();
			foreach (var element in message.Body.Split(';'))
			{
				var pair = element.Split(':');

				if (pair.Length == 2)
				{
					auctionEvent.Add(pair[0].Trim(), pair[1].Trim());
				}
			}

			return auctionEvent;
		}
	}
}