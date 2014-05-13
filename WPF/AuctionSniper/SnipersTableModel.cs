using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AuctionSniperApplication.Properties;

namespace AuctionSniperApplication
{
	public class SnipersTableModel : INotifyPropertyChanged
	{
		private String _status;
		public String Status
		{
			get { return _status; }
			set
			{
				_status = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}