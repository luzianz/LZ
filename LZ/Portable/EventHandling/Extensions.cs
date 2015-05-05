using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LZ.EventHandling
{
	public static class Extensions
	{
		/// <summary>
		/// Encapsulated boilerplate for setting properties honoring INotifyPropertyChanged
		/// </summary>
		/// <param name="sender">The object calling this function</param>
		/// <param name="storage">The field intended to store the property's value</param>
		/// <param name="value">The property setter value - new value</param>
		/// <param name="propertyName">The name of the property</param>
		/// <returns>true if the property's value changed - the backing field is different from the new property value</returns>
		public static bool SetProperty<T>(this PropertyChangedEventHandler eh, object sender, ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Object.Equals(storage, value)) return false;

			storage = value;

			eh.RaiseEvent(sender, propertyName);

			return true;
		}

		/// <summary>
		/// Convenience method to subscribe to an observable using delegates instead of an <see cref="System.IObserver&lt;T&gt;"/>.
		/// </summary>
		public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext, Action<Exception> onError = null, Action onComplete = null)
		{
			return observable.Subscribe(new DelegateObserver<T>(onNext, onError, onComplete));
		}

		/// <summary>
		/// Convenience method to raise an event.
		/// </summary>
		public static void RaiseEvent<TEventArgs>(this EventHandler<TEventArgs> eventHandlerDelegate, object sender, TEventArgs e) where TEventArgs : EventArgs
		{
			if (eventHandlerDelegate == null) return;

			eventHandlerDelegate(sender, e);
		}

		/// <summary>
		/// Convenience method to raise an event.
		/// </summary>
		public static void RaiseEvent(this EventHandler eventHandlerDelegate, object sender)
		{
			if (eventHandlerDelegate == null) return;

			eventHandlerDelegate(sender, EventArgs.Empty);
		}

		/// <summary>
		/// Convenience method to raise an event.
		/// </summary>
		public static void RaiseEvent(this PropertyChangedEventHandler eventHandlerDelegate, object sender, string propertyName)
		{
			if (eventHandlerDelegate == null) return;

			eventHandlerDelegate(sender, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Subscribes to property change events.
		/// </summary>
		/// <param name="onChange">Action invoked when any on the given properties have changed.</param>
		/// <param name="propertyNames">Names of properties you're interested to know when they change.</param>
		/// <returns></returns>
		public static Action ObservePropertyChanges(this INotifyPropertyChanged observed, Action<string> onChange, params string[] propertyNames)
		{
			if (observed == null) throw new NullReferenceException();
			if (onChange == null) throw new ArgumentNullException("onChange");

			PropertyChangedEventHandler handler = (sender, eventArgs) =>
			{
				if (propertyNames.Contains(eventArgs.PropertyName)) onChange(eventArgs.PropertyName);
			};

			observed.PropertyChanged += handler;

			// return a delegate that unsubscribes from the property change events
			return () => observed.PropertyChanged -= handler;
		}
	}
}