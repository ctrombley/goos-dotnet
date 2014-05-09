using System;
using NUnit.Framework;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace AuctionSniperApplication.Tests
{
	public class AuctionSniperDriver : IDisposable
	{
		private readonly WPFLabel _statusLabel;
		private readonly Application _app;
		private readonly Window _window;

		public AuctionSniperDriver(Application app)
		{
			_app = app;
			_window = _app.GetWindow(AuctionSniperConstants.MainWindowName);
			_window.WaitWhileBusy();
			_statusLabel = _window.Get<WPFLabel>(AuctionSniperConstants.SniperStatusName);
		}

		public void Dispose()
		{
			_app.Close();
		}

		public void ShowsSniperStatus(string status)
		{
			_window.WaitWhileBusy();
			Assert.AreEqual(status, _statusLabel.Text);
		}
	}
}