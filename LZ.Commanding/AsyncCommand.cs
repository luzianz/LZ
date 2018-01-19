using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LZ.ComponentModel;

namespace LZ.Commanding {

	/// <summary>
	/// A command that can execute only when it is not busy.
	/// This command is busy after it has been executed asynchronously, and before it completes.
	/// </summary>
	public abstract class AsyncCommand : Command, ICancellable {

		#region Fields

		private NotifyingValueContainer<bool> isExecuting;
		private CancellationTokenSource cts;

		#endregion

		#region Properties

		protected bool IsExecuting {
			get {
				if (IsDisposed) throw new ObjectDisposedException(nameof(AsyncCommand));

				return isExecuting;
			}
			private set {
				if (IsDisposed) throw new ObjectDisposedException(nameof(AsyncCommand));

				isExecuting.Value = value;
			}
		}

		#endregion

		#region Constructor

		public AsyncCommand() {
			isExecuting = new NotifyingValueContainer<bool>(false, () => {
				RaiseCanExecuteChanged();
				CanCancelChanged?.Invoke(this, EventArgs.Empty);
			});
		}

		#endregion

		#region Abstract

		protected abstract Task ExecuteAsync(object parameter, CancellationToken cancellationToken);

		#endregion

		protected virtual void OnError(Exception ex) {
#if DEBUG
			Debug.WriteLine(ex.Message);
			Debugger.Break();
#endif
		}

		#region ICancellable

		public event EventHandler CanCancelChanged;

		public bool CanCancel {
			get {
				if (IsDisposed) throw new ObjectDisposedException(nameof(AsyncCommand));

				return IsExecuting;
			}
		}

		public void Cancel() {
			if (IsDisposed) throw new ObjectDisposedException(nameof(AsyncCommand));
			if (!CanCancel) throw new InvalidOperationException("already cancelled");

			cts.Cancel();
		}

		#endregion

		#region Command

		public async override void Execute(object parameter) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(AsyncCommand));

			try {
				using (cts = new CancellationTokenSource()) {
					IsExecuting = true;
					await ExecuteAsync(parameter, cts.Token);
				}
			} catch (TaskCanceledException) {
				// cancellation is ok
			} catch (Exception ex) {
				// exceptions in async void methods cannot caught by the caller, so we'll use a handler
				OnError(ex);
			} finally {
				cts = null;
				IsExecuting = false;
			}
		}

		public override bool CanExecute(object parameter) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(AsyncCommand));

			return !IsExecuting;
		}

		#endregion
	}
}
