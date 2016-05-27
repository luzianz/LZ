using LZ;
using LZ.Commanding;
using LZ.EventHandling;
using System;
using System.Threading.Tasks;
using LZ.Async;
using System.Threading;

namespace LZ.Metro.ViewModel {

	public partial class ShareTargetViewModel {

		private class ShareCommand : AsyncCommand {

			#region Fields

			private IShareCommandContext context;

			#endregion

			#region Constructor

			public ShareCommand(IShareCommandContext context) {
				this.context = context;
			}

			#endregion

			#region AsyncCommand

			protected override async Task ExecuteAsync(object parameter, CancellationToken cancellationToken) {
				context.ShareOperation.ReportStarted();

				var args = new TaskCompletionEventArgs(cancellationToken);
				
				try {
					context.RaiseSharing(args);

					await args.AwaitAll();

					context.ShareOperation.ReportCompleted();
				} catch (Exception) {
					//context.ShareOperation.ReportError(ex.Message);
					context.ShareOperation.ReportError("unknown error");
				}
			}

			#endregion
		}
	}
}
