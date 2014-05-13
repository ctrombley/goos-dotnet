using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using AuctionSniperApplication.UI;

namespace AuctionSniperApplication
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int ArgServer = 1;
		private const int ArgUsername = 2;
		private const int ArgPassword = 3;
		private const string ItemIdAsLogin = "auction-{0}";

		private readonly Jid _sniperId;
		private readonly XmppClientConnection _conn;
		public readonly  SnipersTableViewModel SnipersViewModel = new SnipersTableViewModel();
		private readonly Dictionary<string, IMessageListener> _listeners = new Dictionary<string,IMessageListener>();

		public MainWindow()
		{
			string[] args = Environment.GetCommandLineArgs();

			try
			{
				_sniperId = new Jid(args[ArgUsername], args[ArgServer], AuctionSniperConstants.AuctionResource);

				_conn = Connect(args[ArgPassword]);
			}
			catch (Exception ex)
			{
				Trace.WriteLine(String.Format("ERROR: {0}", ex));
			}

			InitializeComponent();
			SnipersDataGrid.ItemsSource = SnipersViewModel.Snipers;
		}

		private void JoinAuction(string itemId)
		{
			var auction = new XmppAuction(_conn, itemId);
			_listeners.Add(String.Format(ItemIdAsLogin, itemId), 
				new AuctionMessageTranslator(_sniperId, new AuctionSniper(itemId, auction, SnipersViewModel)));
			SnipersViewModel.Add(SniperSnapshot.Joining(itemId));
			auction.Join();
		}

		private void OnAuthError(object sender, Element e)
		{
			Trace.WriteLine("AUTHERROR");
		}

		private void OnLogin(object sender)
		{
			Trace.WriteLine("LOGGED IN");
		}

		private void OnMessage(object sender, Message msg)
		{
			Trace.WriteLine(String.Format("MESSAGE: {0}", msg));
			var listener = _listeners[msg.From.User];
			listener.ProcessMessage(msg);
		}

		private XmppClientConnection Connect(string password)
		{
			var conn = new XmppClientConnection(_sniperId.Server) {AutoAgents = false};

			conn.OnLogin += OnLogin;
			conn.OnMessage += OnMessage;
			conn.OnAuthError += OnAuthError;

			conn.Open(_sniperId.User, password, _sniperId.Resource);

			return conn;
		}

		private void BidButton_Click(object sender, RoutedEventArgs e)
		{
			JoinAuction(NewItemTextBox.Text);
		}
	}
}