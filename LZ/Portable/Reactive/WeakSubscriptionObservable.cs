using System;
using LZ.Collections;

namespace LZ.Reactive {

	public class WeakSubscriptionObservable<T> : ObservableBase<T> {

		#region Fields

		private WeakCollection<IObserver<T>> weakObservers = new WeakCollection<IObserver<T>>();

		#endregion

		#region ObservableBase<T>

		protected override void AddObserver(IObserver<T> observer) {
			weakObservers.Add(observer);
		}

		protected override void RemoveObserver(IObserver<T> observer) {
			weakObservers.Remove(observer);
		}

		#endregion
	}
}
