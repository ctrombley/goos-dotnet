using agsXMPP.protocol.client;
using NUnit.Framework;
using Rhino.Mocks;

namespace AuctionSniperApplication.Tests
{
	[TestFixture]
	public class AuctionMessageTranslatorTests
	{
		private IAuctionEventListener _listener;
		private AuctionMessageTranslator _translator;

		[SetUp]
		public void SetUp()
		{
			_listener = MockRepository.GenerateMock<IAuctionEventListener>();
			_translator = new AuctionMessageTranslator(_listener);
		}

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
		public void NotifiesBidDetailsWhenCurrentPriceMessageReceived()
		{
			var message = new Message
			{
				Body = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; Increment: 7; Bidder: Someone else;"
			};

			_translator.ProcessMessage(message);

			_listener.AssertWasCalled(l => l.CurrentPrice(192, 7), options => options.Repeat.Once());
		}
	}
}
