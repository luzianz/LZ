using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.UI.Xaml.Media;

namespace LZ.Metro.ViewModel
{
	/// <summary>
	/// Provides context for LoadCommand.
	/// This interface is designed to provide the LoadCommand with only the resources it needs to function, thus protecting resources of its
	/// corresponding viewmodel not relevant to its operation.
	/// </summary>
	internal interface ILoadCommandContext
	{
		ShareOperation ShareOperation { get; }
		string Description { set; }
		string Title { set; }
		string SourceApplicationName { set; }
		string TextContent { set; }
		ImageSource Thumbnail { set; }
	}
}
