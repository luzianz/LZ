using System;
using System.Collections.Generic;

namespace LZ.EventHandling
{
	public class Subject<T> : Publisher<T>, IObserver<T>
	{
		#region Fields
		private readonly List<IObserver<T>> observers;
		#endregion

		#region Constructor
		public Subject()
		{
			observers = new List<IObserver<T>>();
		}
		#endregion

		#region Publisher<T>
		protected override void AddObserver(IObserver<T> observer)
		{
			observers.Add(observer);
		}

		protected override void RemoveObserver(IObserver<T> observer)
		{
			observers.Remove(observer);
		}
		#endregion

		#region IObserver<T>
		public void OnCompleted()
		{
			foreach (var observer in observers)
			{
				observer.OnCompleted();
			}
			observers.Clear();
		}

		public void OnError(Exception error)
		{
			foreach (var observer in observers)
			{
				observer.OnError(error);
			}
		}

		public void OnNext(T value)
		{
			foreach (var observer in observers)
			{
				observer.OnNext(value);
			}
		}
		#endregion
	}
}
