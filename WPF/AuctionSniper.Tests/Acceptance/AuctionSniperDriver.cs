using System;
using System.Linq;
using NUnit.Framework;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace AuctionSniperApplication.Tests.Acceptance
{
	public class AuctionSniperDriver : IDisposable
	{
		private readonly Application _app;
		private readonly Window _window;

		public AuctionSniperDriver(Application app)
		{
			_app = app;
			_window = _app.GetWindow(AuctionSniperConstants.MainWindowName);
			_window.WaitWhileBusy();
		}

		public ListView SnipersDataGrid
		{
			get { return _window.Get<ListView>(AuctionSniperConstants.SnipersDataGridName); }
		}

		public TextBox NewItemTextBox
		{
			get { return _window.Get<TextBox>(AuctionSniperConstants.NewItemIdTextboxName); }
		}

		public Button BidButton
		{
			get { return _window.Get<Button>(AuctionSniperConstants.BidButtonName); }
		}

		public void Dispose()
		{
			_app.Close();
		}

		public void ShowsSniperStatus(string itemId, int lastPrice, int lastBid, string status)
		{
			_window.WaitWhileBusy();
			Assert.IsTrue(HasRow(itemId, lastPrice, lastBid, status));
		}

		public void HasTitle(string title)
		{
			_window.WaitWhileBusy();
			Assert.That(_window.Title, Is.EqualTo(title));
		}

		public void HasColumnTitles()
		{
			Assert.That(SnipersDataGrid.Header.Columns[0].Text, Is.EqualTo("Item"));
			Assert.That(SnipersDataGrid.Header.Columns[1].Text, Is.EqualTo("Last Price"));
			Assert.That(SnipersDataGrid.Header.Columns[2].Text, Is.EqualTo("Last Bid"));
			Assert.That(SnipersDataGrid.Header.Columns[3].Text, Is.EqualTo("Status"));
		}

		public void StartBiddingFor(string itemId)
		{
			NewItemTextBox.Text = itemId;
			BidButton.Click();
		}

		private bool HasRow(string itemId, int lastPrice, int lastBid, string status)
		{
			return SnipersDataGrid.Rows.Any(sniper => sniper.Cells[0].Text.Equals(itemId) 
				&& sniper.Cells[1].Text.Equals(lastPrice.ToString()) 
				&& sniper.Cells[2].Text.Equals(lastBid.ToString()) 
				&& sniper.Cells[3].Text.Equals(status));
		}
	}
}