using System;
using System.Diagnostics;
using System.Windows;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;

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
		private const int ArgItemId = 4;

		private readonly IAuction _auction;
		private readonly IMessageListener _listener;

		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainWindowViewModel();

			string[] args = Environment.GetCommandLineArgs();

			try
			{
				var sniperJid = new Jid(args[ArgUsername], args[ArgServer], AuctionSniperConstants.AuctionResource);
				XmppClientConnection conn = Connect(sniperJid, args[ArgPassword]);
				StatusLabel.Content = AuctionSniperConstants.StatusJoining;

				_auction = new XmppAuction(conn, args[ArgItemId]);
				_listener = new AuctionMessageTranslator(sniperJid, new AuctionSniper(_auction, new SniperStateDisplayer(this)));
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

		private XmppClientConnection Connect(Jid sniperId, string password)
		{
			var conn = new XmppClientConnection(sniperId.Server) {AutoAgents = false};

			conn.OnLogin += OnLogin;
			conn.OnMessage += OnMessage;
			conn.OnAuthError += OnAuthError;

			conn.Open(sniperId.User, password, sniperId.Resource);

			return conn;
		}
	}
}