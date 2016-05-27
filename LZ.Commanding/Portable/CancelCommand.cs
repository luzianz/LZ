using System;
using System.Threading;
using System.Threading.Tasks;
using LZ.ComponentModel;

namespace LZ.Commanding {

	public class CancelCommand : AsyncCommand {

		#region Fields

		private readonly ICancellable cancellable;

		#endregion

		#region Constructor

		public CancelCommand(ICancellable cancellable) {
			if (cancellable == null) throw new ArgumentNullException(nameof(cancellable));

			this.cancellable = cancellable;

			cancellable.CanCancelChanged += Cancellable_CanCancelChanged;
			AddDisposable(new DelegateDisposable(() => cancellable.CanCancelChanged -= Cancellable_CanCancelChanged));
		} 

		#endregion

		private void Cancellable_CanCancelChanged(object sender, EventArgs e) {
			RaiseCanExecuteChanged();
		}

		#region Command

		public override bool CanExecute(object parameter) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(CancelCommand));

			return base.CanExecute(parameter) && cancellable.CanCancel;
		}

		protected async override Task ExecuteAsync(object parameter, CancellationToken cancellationToken) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(CancelCommand));

			await BeforeCancel(parameter);
			cancellable.Cancel();
			await AfterCancel(parameter);
		}
		
		#endregion

		protected async virtual Task BeforeCancel(object parameter) => await Task.Yield();
		protected async virtual Task AfterCancel(object parameter) => await Task.Yield();
	}
}
