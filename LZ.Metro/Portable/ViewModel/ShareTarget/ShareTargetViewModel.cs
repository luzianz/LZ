using LZ.EventHandling;
using LZ.Metro.EventHandling;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace LZ.Metro.ViewModel
{
	public partial class ShareTargetViewModel : INotifyPropertyChanged, ILoadCommandContext, IShareCommandContext
	{
		#region Events

		public event TypedEventHandler<ShareTargetViewModel, ITaskCompletionEventArgs> Sharing;

		#endregion

		#region Fields

		private readonly ShareOperation _ShareOperation;

		#endregion

		#region Properties

		private string _Description;
		public string Description
		{
			get { return _Description; }
			private set { PropertyChanged.SetProperty(this, ref _Description, value); }
		}

		private string _Title;
		public string Title
		{
			get { return _Title; }
			private set { PropertyChanged.SetProperty(this, ref _Title, value); }
		}

		private string _SourceApplicationName;
		public string SourceApplicationName
		{
			get { return _SourceApplicationName; }
			private set { PropertyChanged.SetProperty(this, ref _SourceApplicationName, value); }
		}

		private string _TextContent;
		public string TextContent
		{
			get { return _TextContent; }
			private set { PropertyChanged.SetProperty(this, ref _TextContent, value); }
		}

		private ImageSource _Thumbnail;
		public ImageSource Thumbnail
		{
			get { return _Thumbnail; }
			private set { PropertyChanged.SetProperty(this, ref _Thumbnail, value); }
		}

		public ICommand Load { get; private set; }

		public ICommand Share { get; private set; }

		#endregion

		#region Constructor

		public ShareTargetViewModel(ShareOperation shareOperation)
		{
			if (shareOperation == null) throw new ArgumentNullException("shareOperation");

			_ShareOperation = shareOperation;

			Load = new LoadCommand(this);
			Share = new ShareCommand(this);
		}

		#endregion

		#region ILoadCommandContext

		ShareOperation ILoadCommandContext.ShareOperation
		{
			get { return _ShareOperation; }
		}

		string ILoadCommandContext.Description
		{
			set { Description = value; }
		}

		string ILoadCommandContext.Title
		{
			set { Title = value; }
		}

		string ILoadCommandContext.TextContent
		{
			set { TextContent = value; }
		}

		string ILoadCommandContext.SourceApplicationName
		{
			set { SourceApplicationName = value; }
		}

		ImageSource ILoadCommandContext.Thumbnail
		{
			set { Thumbnail = value; }
		}

		#endregion

		#region IShareCommandContext

		void IShareCommandContext.RaiseSharing(ITaskCompletionEventArgs args)
		{
			Sharing.TryRaiseEvent(this, args);
		}

		ShareOperation IShareCommandContext.ShareOperation
		{
			get { return _ShareOperation; }
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
