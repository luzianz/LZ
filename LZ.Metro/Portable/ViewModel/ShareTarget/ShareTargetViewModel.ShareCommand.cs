using LZ;
using LZ.Commanding;
using LZ.EventHandling;
using System;
using System.Threading.Tasks;

namespace LZ.Metro.ViewModel
{
	public partial class ShareTargetViewModel
	{
		private class ShareCommand : AsyncCommand
		{
			#region Fields

			private IShareCommandContext context;

			#endregion

			#region Constructor

			public ShareCommand(IShareCommandContext context)
			{
				this.context = context;
			}

			#endregion

			#region AsyncCommand

			protected override async Task ExecuteAsync(object parameter)
			{
				context.ShareOperation.ReportStarted();
				var tcs = new TaskCompletionSource<object>();

				context.RaiseSharing(new TaskCompletionEventArgs(tcs));

				try
				{
					await tcs.Task;
					context.ShareOperation.ReportCompleted();
				}
				catch (Exception ex)
				{
					context.ShareOperation.ReportError(ex.Message);
				}
			}

			#endregion
		}
	}
}
