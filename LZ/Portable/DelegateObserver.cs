using System;

namespace LZ
{
	public class DelegateObserver<T> : IObserver<T>
	{
		#region Fields

		private readonly Action<T> onNext;
		private readonly Action<Exception> onError;
		private readonly Action onComplete;

		#endregion

		#region Constructor

		public DelegateObserver(Action<T> onNext, Action<Exception> onError = null, Action onComplete = null)
		{
			if (onNext == null) throw new ArgumentNullException("onNext");

			this.onNext = onNext;
			this.onError = onError;
			this.onComplete = onComplete;
		}

		#endregion

		#region IObserver<T>

		public void OnCompleted()
		{
			if (onComplete == null) return;
			onComplete();
		}

		public void OnError(Exception error)
		{
			if (onError == null) return;
			onError(error);
		}

		public void OnNext(T value)
		{
			onNext(value);
		}

		#endregion
	}
}
