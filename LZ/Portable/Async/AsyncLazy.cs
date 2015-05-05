using System;
using System.Threading.Tasks;

namespace LZ.Async
{
	/// <summary>
	/// An <see cref="LZ.Async.IAsyncResource&lt;T&gt;"/> that returns the same item (caches) once it's fetched.
	/// </summary>
	public class AsyncLazy<T> : IAsyncResource<T>
	{
		#region Fields
		private readonly Func<Task<T>> getAsync;
		private readonly bool retryNullResult;
		private T data;
		private bool wasFetched;
		#endregion

		#region Constructor
		public AsyncLazy(Func<Task<T>> getAsync, bool retryNullResult = false)
		{
			if (getAsync == null) throw new ArgumentNullException("getAsync");
			this.getAsync = getAsync;

			this.retryNullResult = retryNullResult;
		}
		#endregion

		#region IAsyncResource<T>
		public async Task<T> GetAsync()
		{
			if (!wasFetched)
			{
				data = await getAsync();

				wasFetched = !(retryNullResult && data == null);
			}
			return data;
		}
		#endregion
	}
}
