using System;

namespace LZ.Reactive {

	public abstract class ObservableBase<T> : IObservable<T> {

		#region Abstract

		protected abstract void AddObserver(IObserver<T> observer);
		protected abstract void RemoveObserver(IObserver<T> observer);

		#endregion

		#region IObservable<T>

		public IDisposable Subscribe(IObserver<T> observer) {
			AddObserver(observer);

			return new DelegateDisposable(() => RemoveObserver(observer));
		}

		#endregion
	}
}
