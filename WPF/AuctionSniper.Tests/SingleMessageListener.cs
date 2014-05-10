using System.Collections.Concurrent;
using agsXMPP;
using agsXMPP.protocol.client;
using NHamcrest;
using NUnit.Framework;

namespace AuctionSniperApplication.Tests
{
	public class SingleMessageListener : IMessageListener
	{
		private readonly BlockingCollection<Message> _messages = new BlockingCollection<Message>();

		public void ProcessMessage(Message message)
		{
			_messages.Add(message);
		}

		public void ReceivesAMessage(Jid sniperId, IMatcher<string> messageMatcher)
		{
			Message message;
			_messages.TryTake(out message, 5000);

			Assert.That(message, Is.Not.Null);
			Assert.That(message.From, Is.EqualTo(sniperId));
			Assert.That(messageMatcher.Matches(message.Body));
		}
	}
}