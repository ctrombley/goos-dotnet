using System;
using System.Diagnostics;
using System.Text;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.x.muc;
using agsXMPP.Xml.Dom;
using NHamcrest;
using NHamcrest.Core;

namespace AuctionSniperApplication.Tests
{
	public class FakeAuctionServer
	{
		public const string ItemIdAsLogin = "auction-{0}";
		private const string AuctionPassword = "auction";

		private readonly XmppClientConnection _conn;
		private readonly SingleMessageListener _messageListener;
		private MucManager _chat;

		public FakeAuctionServer(string itemId)
		{
			ItemId = itemId;
			_conn = new XmppClientConnection(ApplicationRunner.XmppServer) {AutoAgents = false};

			_conn.OnMessage += OnMessage;
			_conn.OnAuthError += OnAuthError;
			_conn.OnError += OnError;
			_conn.OnLogin += OnLogin;
			_conn.OnMessage += OnMessage;
			_conn.OnClose += OnClose;
			_conn.OnXmppConnectionStateChanged += OnXmppConnectionStateChanged;
			_conn.OnWriteSocketData += OnWriteSocketData;
			_conn.OnReadSocketData += OnReadSocketData;

			_messageListener = new SingleMessageListener();
		}

		public string ItemId { get; private set; }

		public void StartSellingItem()
		{
			_conn.Open(string.Format(ItemIdAsLogin, ItemId), AuctionPassword, ApplicationRunner.AuctionResource);
		}

		private void OnAuthError(object sender, Element e)
		{
			Debug.WriteLine("AUTHERROR");
		}

		private void OnReadSocketData(object sender, byte[] data, int count)
		{
			Debug.WriteLine(String.Format("READ: {0}", Encoding.Default.GetString(data, 0, count)));
		}

		private void OnWriteSocketData(object sender, byte[] data, int count)
		{
			Debug.WriteLine(String.Format("WRITE: {0}", Encoding.Default.GetString(data, 0, count)));
		}

		private void OnClose(object sender)
		{
			Debug.WriteLine("CLOSE");
		}

		private void OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
		{
			Debug.WriteLine("STATE: {0}", state);
		}

		private void OnError(object sender, Exception ex)
		{
			Debug.WriteLine("ERROR: {0}", ex);
		}

		private void OnLogin(object sender)
		{
			Debug.WriteLine("LOGGED IN");
		}

		private void OnMessage(object sender, Message msg)
		{
			Debug.WriteLine("MESSAGE: {0}", msg);
			_messageListener.ProcessMessage(msg);
		}

		public void HasReceivedJoinRequestFrom(string sniperId)
		{
			ReceivesAMessageMatching(sniperId, Is.EqualTo(AuctionSniperConstants.JoinCommandFormat));
		}

		private void ReceivesAMessageMatching(string sniperId, IMatcher<string> messageMatcher)
		{
			_messageListener.ReceivesAMessage(sniperId, messageMatcher);
		}

		public void AnnounceClosed()
		{
			SendMessage("SOLVersion: 1.1; Event: CLOSE;");
		}

		private void SendMessage(string body)
		{
			var message = (new Message
			{
				To = ApplicationRunner.SniperXmppId,
				Body = body
			});

			_conn.Send(message);
		}

		public void Stop()
		{
			_conn.Close();
		}

		public void ReportPrice(int price, int increment, string bidder)
		{
			SendMessage(String.Format("SOLVersion: 1.1; Event: PRICE; "
									+ "CurrentPrice: {0}; Increment: {1}; Bidder: {2};", price, increment, bidder));
		}

		public void HasReceivedBid(int bid, string sniperId)
		{
			ReceivesAMessageMatching(sniperId, Is.EqualTo(String.Format(AuctionSniperConstants.BidCommandFormat, bid)));
		}
	}
}