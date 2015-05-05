using LZ;
using LZ.EventHandling;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

namespace LZ.Metro.ViewModel
{
	/// <summary>
	/// Provides context for ShareCommand.
	/// This interface is designed to provide the ShareCommand with only the resources it needs to function, thus protecting resources of its
	/// corresponding viewmodel not relevant to its operation.
	/// </summary>
	internal interface IShareCommandContext
	{
		ShareOperation ShareOperation { get; }
		void RaiseSharing(ITaskCompletionEventArgs args);
	}
}
