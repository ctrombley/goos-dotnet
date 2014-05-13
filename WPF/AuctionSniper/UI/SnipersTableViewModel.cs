using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AuctionSniperApplication.UI
{
	public class SnipersTableViewModel : ISniperListener
	{
		private readonly ObservableCollection<SniperSnapshotViewModel> _snipers = new ObservableCollection<SniperSnapshotViewModel>();
		public ObservableCollection<SniperSnapshotViewModel> Snipers
		{
			get { return _snipers; }
		}

		public void Add(SniperSnapshot snapshot)
		{
			_snipers.Add(new SniperSnapshotViewModel(snapshot));	
		}

		public void SniperStateChanged(SniperSnapshot newSnapshot)
		{
			var sniper = GetSniperMatching(newSnapshot);
			if (sniper == null) return;
			sniper.Update(newSnapshot);
		}

		private SniperSnapshotViewModel GetSniperMatching(SniperSnapshot newSnapshot)
		{
			var sniper = _snipers.FirstOrDefault(s => s.ItemId == newSnapshot.ItemId);

			if (sniper == null)
			{
				throw new Defect(String.Format("Cannot find match for " + newSnapshot.ItemId));
			}

			return sniper;
		}
	}
}