using LZ.EventHandling;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace LZ.Windows.ViewModel
{
	public class WindowViewModel : INotifyPropertyChanged
	{
		#region Fields

		private readonly Window window;

		#endregion

		#region Properties

		public double Width
		{
			get { return _Width; }
			private set { PropertyChanged.SetProperty(this, ref _Width, value); }
		}

		public double Height
		{
			get { return _Height; }
			private set { PropertyChanged.SetProperty(this, ref _Height, value); }
		}

		#endregion

		#region Property Backing

		private double _Width;
		private double _Height;

		#endregion

		#region Constructor

		public WindowViewModel(Window window)
		{
			this.window = window;

			this.window.SizeChanged += window_SizeChanged;
		}

		#endregion

		#region Event Handlers

		void window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
		{
			if (e.Size.IsEmpty) return;

			this.Width = e.Size.Width;
			this.Height = e.Size.Height;
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
