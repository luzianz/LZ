using System;
using System.Threading;
using System.Threading.Tasks;

namespace LZ.Async {

	public class TaskCompletionDeferral : DisposableBase, IDeferral {

		#region Fields

		private readonly TaskCompletionSource<object> taskCompletionSource;
		private readonly IDisposable cancellationRegistration;
		private Action onCancel;

		#endregion

		#region Constructor

		public TaskCompletionDeferral(CancellationToken cancellationToken) {
			taskCompletionSource = new TaskCompletionSource<object>();
			cancellationRegistration = cancellationToken.Register(onCancel);
		}

		#endregion

		#region Properties

		public Task Task {
			get {
				if (IsDisposed) throw new ObjectDisposedException(nameof(TaskCompletionDeferral));

				return taskCompletionSource.Task;
			}
		}

		#endregion
		
		#region IDeferral
		
		public void RegisterCancellationHandler(Action onCancel) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(TaskCompletionDeferral));

			this.onCancel += onCancel;
		}

		#endregion

		#region DisposableBase

		protected override void OnDispose() {
			cancellationRegistration.Dispose();
			taskCompletionSource.SetResult(null);
		}

		#endregion
	}
}
