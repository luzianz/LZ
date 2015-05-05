using System;

namespace LZ.EventHandling
{
	public abstract class Publisher<T> : IObservable<T>
	{
		#region Abstract
		protected abstract void AddObserver(IObserver<T> observer);
		protected abstract void RemoveObserver(IObserver<T> observer);
		#endregion

		#region IObservable<T>
		public IDisposable Subscribe(IObserver<T> observer)
		{
			AddObserver(observer);
			return new DelegateDisposer(() => RemoveObserver(observer));
		}
		#endregion
	}
}
