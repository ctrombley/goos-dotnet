using System.Collections.Generic;

namespace AuctionSniperApplication
{
	public static class AuctionEventCreator
	{
		public static AuctionEvent From(string body)
		{
			var auctionEvent = new AuctionEvent();

			foreach (var field in FieldsIn(body))
			{
				AddField(field, auctionEvent);
			}

			return auctionEvent;
		}

		private static void AddField(string field, AuctionEvent auctionEvent)
		{
			var pair = field.Split(':');

			if (pair.Length == 2)
			{
				auctionEvent.Add(pair[0].Trim(), pair[1].Trim());
			}
		}

		private static IEnumerable<string> FieldsIn(string body)
		{
			return body.Split(';');
		}
	}
}