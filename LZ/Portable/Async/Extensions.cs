using LZ.EventHandling;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LZ.Async
{
	/// <summary>
	/// A delegate representing an async operation.
	/// </summary>
	/// <typeparam name="T">Async return type.</typeparam>
	/// <param name="onResult">Action invoked when on completion and result is acquired.</param>
	/// <param name="onError">Action invoked when an error occurs.</param>
	/// <returns>Action to be invoked to request cancellation.</returns>
	public delegate Action AsyncMethod<T>(Action<T> onResult, Action<Exception> onError);

	public static class Extensions
	{
		/// <summary>
		/// Retries an async operation until it is successful or the number of attempts exceed a given count.
		/// </summary>
		/// <param name="attemptCount">Number of tries before giving up.</param>
		/// <param name="delay">A delay given between retries.</param>
		public static async Task RetryAsync(this Func<Task> todo, uint attemptCount, TimeSpan delay, CancellationToken cancellationToken)
		{
			if (todo == null) throw new ArgumentNullException("todo");
			if (attemptCount == 0) throw new ArgumentOutOfRangeException("attemptCount", "Needs to be at least one.");

			int attempt = 0;
			bool willRetry;
			while (attempt < attemptCount)
			{
				if (cancellationToken.IsCancellationRequested) break;

				try
				{
					willRetry = false;
					await todo();
					break;
				}
				catch
				{
					willRetry = true;
				}
				finally
				{
					attempt++;
				}

				if (willRetry)
				{
					try
					{
						await Task.Delay(delay, cancellationToken);
					}
					catch (TaskCanceledException)
					{
						// break from loop if task was cancelled
						break;
					}
				}
			}
		}

		/// <summary>
		/// Retries an async operation until it is successful or the number of attempts exceed a given count.
		/// </summary>
		/// <param name="attemptCount">Number of tries before giving up.</param>
		/// <param name="delay">A delay given between retries.</param>
		public static Task RetryAsync(this Func<Task> todo, uint attemptCount, TimeSpan delay)
		{
			return RetryAsync(todo, attemptCount, delay, CancellationToken.None);
		}

		public static Task<T> GetAsync<T>(this AsyncMethod<T> method)
		{
			return GetAsync(method, CancellationToken.None);
		}

		public static Task<T> GetAsync<T>(this AsyncMethod<T> method, CancellationToken cancellationToken)
		{
			if (method == null) throw new NullReferenceException();

			var completion = new TaskCompletionSource<T>();

			var cancel = method(completion.SetResult, completion.SetException);

			cancellationToken.Register(cancel);
			cancellationToken.Register(completion.SetCanceled);

			return completion.Task;
		}

		public static Task<T> InvokeAsync<T>(this EventHandler<ITaskCompletionEventArgs<T>> @delegate, object sender)
		{
			// As events can be null if there are no subscribers, we should gracefully ignore this case.
			if (@delegate == null) return Task.FromResult<T>(default(T));

			var tcs = new TaskCompletionSource<T>();
			@delegate(sender, new TaskCompletionEventArgs<T>(tcs));
			return tcs.Task;
		}

		public static Task InvokeAsync(this EventHandler<ITaskCompletionEventArgs> @delegate, object sender)
		{
			// As events can be null if there are no subscribers, we should gracefully ignore this case.
			if (@delegate == null) return Task.FromResult<object>(null);

			var tcs = new TaskCompletionSource<object>();
			@delegate(sender, new TaskCompletionEventArgs(tcs));
			return tcs.Task;
		}
	}
}
