using System;

namespace LZ.Reactive {

	public class DelegateObserver<T> : IObserver<T> {

		#region Fields

		private readonly Action<T> onNext;
		private readonly Action<Exception> onError;
		private readonly Action onComplete;

		#endregion

		#region Constructor

		public DelegateObserver(Action<T> onNext = null, Action<Exception> onError = null, Action onComplete = null) {
			this.onNext = onNext;
			this.onError = onError;
			this.onComplete = onComplete;
		}

		#endregion

		#region IObserver<T>

		public void OnCompleted() {
			onComplete?.Invoke();
		}

		public void OnError(Exception error) {
			onError?.Invoke(error);
		}

		public void OnNext(T value) {
			onNext?.Invoke(value);
		}

		#endregion
	}
}
