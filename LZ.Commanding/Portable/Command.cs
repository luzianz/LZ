using LZ.EventHandling;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace LZ.Commanding {

	public class Command : CompositeDisposableBase, ICommand {

		#region Methods
		
		public void RaiseCanExecuteChanged() {
			if (IsDisposed) throw new ObjectDisposedException(nameof(Command));

			CanExecuteChanged.Invoke(this);
		}

		protected void ObservePropertyChanges(INotifyPropertyChanged source, params string[] propertyNames) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(Command));
			if (source == null) throw new ArgumentNullException(nameof(source));

			var observePropertiesSubscription = source.ObservePropertyChanges(_ => RaiseCanExecuteChanged(), propertyNames);
			AddDisposable(observePropertiesSubscription);
		}
		
		#endregion

		#region ICommand

		public virtual bool CanExecute(object parameter) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(Command));

			return true;
		}

		public event EventHandler CanExecuteChanged;

		public virtual void Execute(object parameter) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(Command));
		}

		#endregion
	}
}