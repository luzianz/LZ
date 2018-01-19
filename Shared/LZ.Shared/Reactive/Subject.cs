using System;

namespace LZ.Reactive {

	public class Subject<T> : Observable<T>, IObserver<T> {

		#region IObserver<T>

		public virtual void OnCompleted() {
			base.SetCompleted();
		}

		public virtual void OnError(Exception error) {
			base.SetError(error);
		}

		public virtual void OnNext(T value) {
			base.SetNext(value);
		}

		#endregion
	}
}
