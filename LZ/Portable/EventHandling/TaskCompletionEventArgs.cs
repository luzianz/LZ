using System;
using System.Threading.Tasks;

namespace LZ.EventHandling
{
	public class TaskCompletionEventArgs : EventArgs, ITaskCompletionEventArgs
	{
		#region Fields

		private readonly TaskCompletionSource<object> taskCompletionSource;

		#endregion

		#region Constructor

		public TaskCompletionEventArgs(TaskCompletionSource<object> taskCompletionSource)
		{
			if (taskCompletionSource == null) throw new ArgumentException("taskCompletionSource");
			this.taskCompletionSource = taskCompletionSource;
		}

		#endregion

		#region ITaskCompletionEventArgs

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

		#endregion
	}

	public class TaskCompletionEventArgs<T> : EventArgs, ITaskCompletionEventArgs<T>
	{
		#region Fields

		private readonly TaskCompletionSource<T> taskCompletionSource;

		#endregion

		#region Constructor

		public TaskCompletionEventArgs(TaskCompletionSource<T> taskCompletionSource)
		{
			this.taskCompletionSource = taskCompletionSource;
		}

		#endregion

		#region ITaskCompletionEventArgs<T>

		public void SetResult(T result)
		{
			taskCompletionSource.SetResult(result);
		}

		#endregion

		#region ITaskCompletionEventArgs

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

		#endregion
	}
}
