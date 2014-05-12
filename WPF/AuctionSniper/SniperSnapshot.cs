namespace AuctionSniperApplication
{
	public class SniperSnapshot
	{
		public string ItemId { get; set; }
		public int LastPrice { get; set; }
		public int LastBid { get; set; }
		public SniperState Status { get; set; }

		public SniperSnapshot(string itemId, int lastPrice, int lastBid, SniperState status)
		{
			ItemId = itemId;
			LastBid = lastBid;
			LastPrice = lastPrice;
			Status = status;
		}

		protected bool Equals(SniperSnapshot other)
		{
			return string.Equals(ItemId, other.ItemId)
			       && LastPrice == other.LastPrice
			       && LastBid == other.LastBid;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (ItemId != null ? ItemId.GetHashCode() : 0);
				hashCode = (hashCode*397) ^ LastPrice;
				hashCode = (hashCode*397) ^ LastBid;
				return hashCode;
			}
		}


		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((SniperSnapshot) obj);
		}

		public static SniperSnapshot Bidding(string itemId, int newLastPrice, int newLastBid)
		{
			return new SniperSnapshot(itemId, newLastPrice, newLastBid, SniperState.Bidding);
		}

		public static SniperSnapshot Winning(string itemId, int newLastPrice)
		{
			return new SniperSnapshot(itemId, newLastPrice, newLastPrice, SniperState.Winning);
		}

		public static SniperSnapshot Joining(string itemId)
		{
			return new SniperSnapshot(itemId, 0, 0, SniperState.Joining);
		}

		public static SniperSnapshot Closed(SniperSnapshot previousSnapshot)
		{
			var nextStatus = previousSnapshot.Status == SniperState.Winning 
				? SniperState.Won 
				: SniperState.Lost;

			return new SniperSnapshot(previousSnapshot.ItemId, previousSnapshot.LastPrice, previousSnapshot.LastBid,
				nextStatus);
		}
	}
}