using System;

namespace LZ.Reactive {

	public static class ReactiveExtensions {

		public static IDisposable SubscribeForOne<T>(this IObservable<T> observable, Action<T> onResult) {
			IDisposable subscription = null;

			Action<T> onFirst = result => {
				onResult(result);
				subscription.Dispose();
			};

			var observer = new DelegateObserver<T>(
				onFirst,
				ex => subscription.Dispose(),
				() => subscription.Dispose());

			subscription = observable.Subscribe(observer);

			return subscription;
		}
	}
}
