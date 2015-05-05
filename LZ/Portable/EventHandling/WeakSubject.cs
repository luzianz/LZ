using System;
using LZ.Enumerable;

namespace LZ.EventHandling
{
	/// <summary>
	/// A subject that holds no strong references of its subscribers.
	/// </summary>
	public class WeakSubject<T> : IObservable<T>, IObserver<T>
	{
		#region Fields

		private bool isPublishing = true;
		private WeakDictionary<Guid, IObserver<T>> subscribers;

		#endregion

		#region Constructor

		public WeakSubject()
		{
			subscribers = new WeakDictionary<Guid, IObserver<T>>();
		}

		#endregion

		private void EndPublication()
		{
			subscribers.Dispose();
			subscribers = null;
			isPublishing = false;
		}

		#region IObservable<T>

		public IDisposable Subscribe(IObserver<T> observer)
		{
			Guid key = Guid.NewGuid();
			subscribers.SetValue(key, observer);
			return new Unsubscriber(subscribers, key);
		}

		#endregion

		#region IObserver<T>

		public void OnCompleted()
		{
			if (!isPublishing) throw new InvalidOperationException("Publication has ended");

			subscribers.AliveReferences.ForEach(s => s.OnCompleted());

			EndPublication();
		}

		public void OnError(Exception error)
		{
			if (!isPublishing) throw new InvalidOperationException("Publication has ended");

			subscribers.AliveReferences.ForEach(s => s.OnError(error));

			EndPublication();
		}

		public void OnNext(T value)
		{
			if (!isPublishing) throw new InvalidOperationException("Publication has ended");

			subscribers.AliveReferences.ForEach(s => s.OnNext(value));
		}

		#endregion

		#region Classes

		private class Unsubscriber : IDisposable
		{
			#region Fields

			private WeakReference<IKeyedRemovable<Guid>> weakPublisher;
			private readonly Guid key;

			#endregion

			#region Constructor

			public Unsubscriber(IKeyedRemovable<Guid> publisher, Guid key)
			{
				weakPublisher = new WeakReference<IKeyedRemovable<Guid>>(publisher);
				this.key = key;
			}

			#endregion

			#region IDisposable

			public void Dispose()
			{
				IKeyedRemovable<Guid> publisher;
				if (weakPublisher.TryGetTarget(out publisher))
				{
					publisher.Remove(key);
				}
				weakPublisher = null;
			}

			#endregion
		}

		#endregion
	}
}
