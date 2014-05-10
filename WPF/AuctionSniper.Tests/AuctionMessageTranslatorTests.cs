using System;
using agsXMPP.protocol.client;
using NUnit.Framework;
using Rhino.Mocks;

namespace AuctionSniperApplication.Tests
{
	[TestFixture]
	public class AuctionMessageTranslatorTests
	{
		[SetUp]
		public void SetUp()
		{
			_listener = MockRepository.GenerateMock<IAuctionEventListener>();
			_translator = new AuctionMessageTranslator(ApplicationRunner.SniperJid, _listener);
		}

		private IAuctionEventListener _listener;
		private AuctionMessageTranslator _translator;

		[Test]
		public void NotifiesAuctionClosedWhenCloseMessageReceived()
		{
			var message = new Message
			{
				Body = "SOLVersion: 1.1; Event: CLOSE;"
			};

			_translator.ProcessMessage(message);

			_listener.AssertWasCalled(l => l.AuctionClosed(), options => options.Repeat.Once());
		}

		[Test]
		public void NotifiesBidDetailsWhenCurrentPriceMessageReceivedFromOtherBidder()
		{
			var message = new Message
			{
				Body = String.Format("SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; Increment: 7; Bidder: Someone else;",
					PriceSource.FromOtherBidder)
			};

			_translator.ProcessMessage(message);

			_listener.AssertWasCalled(l => l.CurrentPrice(192, 7, PriceSource.FromOtherBidder), options => options.Repeat.Once());
		}

		[Test]
		public void NotifiesBidDetailsWhenCurrentPriceMessageReceivedFromSniper()
		{
			var message = new Message
			{
				Body = String.Format("SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; Increment: 7; Bidder: {0};",
					ApplicationRunner.SniperJid)
			};

			_translator.ProcessMessage(message);

			_listener.AssertWasCalled(l => l.CurrentPrice(192, 7, PriceSource.FromSniper), options => options.Repeat.Once());
		}
	}
}