using System;
using System.Threading.Tasks;

namespace LZ.EventHandling
{
	public class TaskCompletionEventArgs : EventArgs, ITaskCompletionEventArgs
	{
		private readonly TaskCompletionSource<object> taskCompletionSource;
		public TaskCompletionEventArgs(TaskCompletionSource<object> taskCompletionSource)
		{
			if (taskCompletionSource == null) throw new ArgumentException("taskCompletionSource");
			this.taskCompletionSource = taskCompletionSource;
		}

		public void Complete()
		{
			taskCompletionSource.SetResult(null);
		}

		public void SetException(Exception ex)
		{
			taskCompletionSource.SetException(ex);
		}

		public void SetCanceled()
		{
			taskCompletionSource.SetCanceled();
		}
	}
	public class TaskCompletionEventArgs<T> : EventArgs, ITaskCompletionEventArgs<T>
	{
		private readonly TaskCompletionSource<T> taskCompletionSource;
		public TaskCompletionEventArgs(TaskCompletionSource<T> taskCompletionSource)
		{
			this.taskCompletionSource = taskCompletionSource;
		}

		public void SetResult(T result)
		{
			taskCompletionSource.SetResult(result);
		}

		public void Complete()
		{
			taskCompletionSource.SetResult(default(T));
		}

		public void SetException(Exception ex)
		{
			taskCompletionSource.SetException(ex);
		}

		public void SetCanceled()
		{
			taskCompletionSource.SetCanceled();
		}
	}
}
