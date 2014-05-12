using System;
using NUnit.Framework;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace AuctionSniperApplication.Tests
{
	public class AuctionSniperDriver : IDisposable
	{
		private readonly ListView _snipers;
		private readonly Application _app;
		private readonly Window _window;

		public AuctionSniperDriver(Application app)
		{
			_app = app;
			_window = _app.GetWindow(AuctionSniperConstants.MainWindowName);
			_window.WaitWhileBusy();
			_snipers = _window.Get<ListView>(AuctionSniperConstants.SnipersDataGridName);
		}

		public void Dispose()
		{
			_app.Close();
		}

		public void ShowsSniperStatus(string itemId, int lastPrice, int lastBid, string status)
		{
			_window.WaitWhileBusy();
			Assert.That(_snipers.Rows[0].Cells[0].Text, Is.EqualTo(itemId));
			Assert.That(_snipers.Rows[0].Cells[1].Text, Is.EqualTo(lastPrice.ToString()));
			Assert.That(_snipers.Rows[0].Cells[2].Text, Is.EqualTo(lastBid.ToString()));
			Assert.That(_snipers.Rows[0].Cells[3].Text, Is.EqualTo(status));
		}

		public void ShowsSniperStatus(string status)
		{
			_window.WaitWhileBusy();
			Assert.That(_snipers.Rows[0].Cells[3].Text, Is.EqualTo(status));
		}

		public void HasTitle(string title)
		{
			_window.WaitWhileBusy();
			Assert.That(_window.Title, Is.EqualTo(title));
		}

		public void HasColumnTitles()
		{
			Assert.That(_snipers.Header.Columns[0].Text, Is.EqualTo("Item"));
			Assert.That(_snipers.Header.Columns[1].Text, Is.EqualTo("Last Price"));
			Assert.That(_snipers.Header.Columns[2].Text, Is.EqualTo("Last Bid"));
			Assert.That(_snipers.Header.Columns[3].Text, Is.EqualTo("Status"));
		}
	}
}