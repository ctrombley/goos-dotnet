using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;

namespace AuctionSniperApplication
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, ISniperListener
	{
		private const int ArgHostname = 1;
		private const int ArgUsername = 2;
		private const int ArgPassword = 3;
		private const int ArgItemId = 4;

		private readonly IMessageListener _listener;
		private readonly IAuction _auction;

		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainWindowViewModel();

			string[] args = Environment.GetCommandLineArgs();

			try
			{
				var conn = Connect(args[ArgHostname], args[ArgUsername], args[ArgPassword]);
				StatusLabel.Content = AuctionSniperConstants.StatusJoining;

				_auction = new XmppAuction(conn, args[ArgItemId]);
				_listener = new AuctionMessageTranslator(new AuctionSniper(_auction, this));
			}
			catch (Exception ex)
			{
				Trace.WriteLine(String.Format("ERROR: {0}", ex));	
			}
		}

		private void OnAuthError(object sender, Element e)
		{
			Trace.WriteLine("AUTHERROR");
		}

		private void OnReadSocketData(object sender, byte[] data, int count)
		{
			Trace.WriteLine(String.Format("READ: {0}", Encoding.Default.GetString(data, 0, count)));
		}

		private void OnWriteSocketData(object sender, byte[] data, int count)
		{
			Trace.WriteLine(String.Format("WRITE: {0}", Encoding.Default.GetString(data, 0, count)));
		}

		private void OnClose(object sender)
		{
			Trace.WriteLine("CLOSE");
		}

		private void OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
		{
			Trace.WriteLine(String.Format("STATE: {0}", state));
		}

		private void OnError(object sender, Exception ex)
		{
			Trace.WriteLine(String.Format("ERROR: {0}", ex));
		}

		private void OnLogin(object sender)
		{
			Trace.WriteLine("LOGGED IN");
			_auction.Join();
		}

		private void OnMessage(object sender, Message msg)
		{
			Trace.WriteLine(String.Format("MESSAGE: {0}", msg));
			_listener.ProcessMessage(msg);
		}

		private void UpdateStatusLabel(string status)
		{
			Trace.WriteLine(String.Format("Updating UI Status: {0}", status));
			StatusLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
				(ThreadStart) delegate { StatusLabel.Content = status; });
		}

		private XmppClientConnection Connect(string hostname, string username, string password)
		{
			var conn = new XmppClientConnection(hostname) {AutoAgents = false};

			conn.OnMessage += OnMessage;
			conn.OnAuthError += OnAuthError;
			conn.OnError += OnError;
			conn.OnLogin += OnLogin;
			conn.OnMessage += OnMessage;
			conn.OnClose += OnClose;
			conn.OnXmppConnectionStateChanged += OnXmppConnectionStateChanged;
			conn.OnWriteSocketData += OnWriteSocketData;
			conn.OnReadSocketData += OnReadSocketData;

			conn.Open(username, password, AuctionSniperConstants.AuctionResource);

			return conn;
		}

		public void SniperLost()
		{
			UpdateStatusLabel(AuctionSniperConstants.StatusLost);
		}

		public void SniperBidding()
		{
			UpdateStatusLabel(AuctionSniperConstants.StatusBidding);
		}
	}
}
