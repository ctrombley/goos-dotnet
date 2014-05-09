using System;
using agsXMPP;
using agsXMPP.protocol.client;

namespace AuctionSniperApplication
{
	public class XmppAuction : IAuction
	{
		private const string AuctionIdFormat = ItemIdAsLogin + "@{1}/" + AuctionSniperConstants.AuctionResource;
		private const string ItemIdAsLogin = "auction-{0}";

		private readonly XmppConnection _conn;
		private readonly string _itemId;

		public XmppAuction(XmppConnection conn, string itemId)
		{
			_conn = conn;
			_itemId = itemId;
		}

		public void Bid(int amount)
		{
			SendMessage(String.Format(AuctionSniperConstants.BidCommandFormat, amount));
		}

		public void Join()
		{
			SendMessage(AuctionSniperConstants.JoinCommandFormat);
		}

		private void SendMessage(string body)
		{
			_conn.Send(new Message
			{
				To = AuctionId,
				Body = body
			});
		}

		private string AuctionId
		{
			get
			{
				return String.Format(AuctionIdFormat, _itemId, _conn.Server);
			}
		}
	}
}