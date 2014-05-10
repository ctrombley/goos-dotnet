using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

namespace AuctionSniperApplication
{
	public class SniperStateDisplayer : ISniperListener
	{
		private readonly MainWindow _ui;

		public SniperStateDisplayer(MainWindow ui)
		{
			_ui = ui;
		}

		public void SniperLost()
		{
			UpdateStatusLabel(AuctionSniperConstants.StatusLost);
		}

		public void SniperWon()
		{
			UpdateStatusLabel(AuctionSniperConstants.StatusWon);
		}

		public void SniperWinning()
		{
			UpdateStatusLabel(AuctionSniperConstants.StatusWinning);
		}

		public void SniperBidding()
		{
			UpdateStatusLabel(AuctionSniperConstants.StatusBidding);
		}

		private void UpdateStatusLabel(string status)
		{
			Trace.WriteLine(String.Format("Updating UI Status: {0}", status));
			_ui.StatusLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
				(ThreadStart) delegate { _ui.StatusLabel.Content = status; });
		}
	}
}