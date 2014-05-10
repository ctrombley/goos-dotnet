using System;
using System.Collections.Generic;
using agsXMPP;

namespace AuctionSniperApplication
{
	public class AuctionEvent : Dictionary<string, string>
	{
		public string Type
		{
			get { return this["Event"]; }
		}

		public int CurrentPrice
		{
			get { return Int32.Parse(this["CurrentPrice"]); }
		}

		public int Increment
		{
			get { return Int32.Parse(this["Increment"]); }
		}

		public string Bidder
		{
			get { return this["Bidder"]; }
		}

		public PriceSource IsFrom(Jid sniperId)
		{
			return sniperId.Equals(Bidder) ? PriceSource.FromSniper : PriceSource.FromOtherBidder;
		}
	}
}