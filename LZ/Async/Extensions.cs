using System;
using System.Threading;
using System.Threading.Tasks;

namespace LZ.Async {
	
	public static class Extensions {
		
		public static IDisposable RegisterDisposable(this CancellationToken cancellationToken, IDisposable disposable) {
			return cancellationToken.Register(() => disposable?.Dispose());
		}

		public static async Task InvokeAsync(this EventHandler<IDeferralProvider> ev, object sender, CancellationToken cancellationToken) {
			if (ev != null) {
				var args = new TaskCompletionEventArgs(cancellationToken);
				ev.Invoke(sender, args);
				
				await args.AwaitAll();
			}
		}
	}
}
