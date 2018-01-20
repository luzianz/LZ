using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LZ.Windows.Async {
	public static class Extensions {

		public static IDisposable RegisterAsyncAction(this CancellationToken cancellationToken, IAsyncInfo asyncAction) {
			return cancellationToken.Register(() => {
				if (asyncAction != null && asyncAction.Status == AsyncStatus.Started) {
					asyncAction.Cancel();
				}
			});
		}

		public static async Task InvokeAsync(this IAsyncAction asyncAction, CancellationToken cancellationToken) {
			cancellationToken.ThrowIfCancellationRequested();

			using (var reg = cancellationToken.RegisterAsyncAction(asyncAction)) {
				await asyncAction;
			}
		}

		public static async Task<TResult> InvokeAsync<TResult>(this IAsyncOperation<TResult> asyncOp, CancellationToken cancellationToken) {
			cancellationToken.ThrowIfCancellationRequested();

			using (var reg = cancellationToken.RegisterAsyncAction(asyncOp)) {
				return await asyncOp;
			}
		}

		public static async Task InvokeAsync<TProgress>(this IAsyncActionWithProgress<TProgress> asyncAction, IProgress<TProgress> progress, CancellationToken cancellationToken) {
			cancellationToken.ThrowIfCancellationRequested();

			using (var reg = cancellationToken.RegisterAsyncAction(asyncAction)) {
				asyncAction.Progress = (s, p) => progress.Report(p);
				await asyncAction;
			}
		}

		public static async Task<TResult> InvokeAsync<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> asyncOp, IProgress<TProgress> progress, CancellationToken cancellationToken) {
			cancellationToken.ThrowIfCancellationRequested();

			using (var reg = cancellationToken.RegisterAsyncAction(asyncOp)) {
				asyncOp.Progress = (s, p) => progress.Report(p);
				return await asyncOp;
			}
		}
	}
}
