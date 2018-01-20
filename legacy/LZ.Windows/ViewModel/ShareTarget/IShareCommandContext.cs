using LZ.Async;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

namespace LZ.Windows.ViewModel {

	/// <summary>
	/// Provides context for ShareCommand.
	/// This interface is designed to provide the ShareCommand with only the resources it needs to function, thus protecting resources of its
	/// corresponding viewmodel not relevant to its operation.
	/// </summary>
	internal interface IShareCommandContext {

		ShareOperation ShareOperation { get; }

		void RaiseSharing(IDeferralProvider args);
	}
}
