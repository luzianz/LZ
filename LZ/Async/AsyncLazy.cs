using System;
using System.Threading;
using System.Threading.Tasks;

namespace LZ.Async {

	/// <summary>
	/// An <see cref="LZ.Async.IAsyncResource{T}"/> that returns the same item (caches) once it's fetched.
	/// </summary>
	public class AsyncLazy<T> : IAsyncResource<T> {

		#region Fields

		private readonly Func<CancellationToken, Task<T>> getAsync;
		private T data;
		private bool wasFetched;

		#endregion

		#region Constructor

		public AsyncLazy(Func<Task<T>> getAsync) 
			: this(ct => getAsync()) {
		}

		public AsyncLazy(Func<CancellationToken, Task<T>> getAsync) {
			if (getAsync == null) throw new ArgumentNullException(nameof(getAsync));

			this.getAsync = getAsync;
		}

		#endregion

		#region IAsyncResource<T>

		public async Task<T> GetAsync(CancellationToken cancellationToken) {
			if (!wasFetched) {
				data = await getAsync(cancellationToken);

				wasFetched = true;
			}
			return data;
		}

		#endregion
	}
}
