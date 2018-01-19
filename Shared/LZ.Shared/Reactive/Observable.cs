using System;
using System.Collections.Generic;
using LZ.Collections;

namespace LZ.Reactive {

	public class Observable<T> : ObservableBase<T> {

		#region Fields

		private List<IObserver<T>> observers = new List<IObserver<T>>();

		#endregion

		protected void SetCompleted() {
			observers.ForEach(o => o.OnCompleted());
			observers.Clear();
		}

		protected void SetError(Exception error) {
			observers.ForEach(o => o.OnError(error));
			observers.Clear();
		}

		protected void SetNext(T value) {
			observers.ForEach(o => o.OnNext(value));
		}

		#region ObservableBase<T>

		protected override void AddObserver(IObserver<T> observer) {
			observers.Add(observer);
		}

		protected override void RemoveObserver(IObserver<T> observer) {
			observers.Remove(observer);
		}

		#endregion
	}
}