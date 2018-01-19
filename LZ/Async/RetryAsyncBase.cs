using System;
using System.Threading;
using System.Threading.Tasks;

namespace LZ.Async {

	public abstract class RetryAsyncBase<T> : IAsyncResource<T> {

		#region Fields

		private readonly uint attempts;

		#endregion

		#region Constructor

		public RetryAsyncBase(uint attempts) {
			this.attempts = attempts;
		}

		#endregion

		#region IAsyncResource<T>

		public async Task<T> GetAsync(CancellationToken cancellationToken) {
			if (attempts == 0) return default(T);

			int attemptCount = 0;
			var exceptions = new CompositeException("Retry failed");

			while (true) {
				cancellationToken.ThrowIfCancellationRequested();

				var retryToken = new RetryReportToken();

				await TryExecuteAsync(cancellationToken, retryToken);

				if (retryToken.WasExceptionSet) {
					exceptions.Add(retryToken.Exception);
				} else if (retryToken.WasResultSet) {
					return retryToken.Result;
				}

				attemptCount++;

				if (attemptCount >= attempts) {
					throw exceptions;
				}
			}
		}

		#endregion

		#region Abstract

		protected abstract Task TryExecuteAsync(CancellationToken cancellationToken, IRetryReportToken reportToken);

		#endregion

		#region Types

		protected interface IRetryReportToken {

			T Result { get; set; }

			Exception Exception { get; set; }
		}

		private class RetryReportToken : IRetryReportToken {

			#region Fields

			private T _Result;
			private Exception _Exception;

			#endregion

			#region Properties

			public bool WasResultSet { get; private set; } = false;

			public bool WasExceptionSet { get; private set; } = false;

			#endregion

			#region IRetryReportToken

			public T Result {
				get { return _Result; }
				set {
					if (WasResultSet || WasExceptionSet) throw new InvalidOperationException("you can only set result or exception once");

					_Result = value;
					WasResultSet = true;
				}
			}

			public Exception Exception {
				get { return _Exception; }
				set {
					if (WasResultSet || WasExceptionSet) throw new InvalidOperationException("you can only set result or exception once");
					if (value == null) throw new ArgumentNullException(nameof(value));

					_Exception = value;
					WasExceptionSet = true;
				}
			}

			#endregion
		}

		#endregion
	}
}
