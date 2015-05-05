using System;

namespace LZ.EventHandling
{
	/// <summary>
	/// Event arguments intended for events that need to be asynchronously awaited.
	/// </summary>
	public interface ITaskCompletionEventArgs
	{
		void Complete();
		void SetException(Exception ex);
		void SetCanceled();
	}

	/// <summary>
	/// Event arguments intended for events that need to be asynchronously awaited.
	/// </summary>
	/// <typeparam name="T">Result type.</typeparam>
	public interface ITaskCompletionEventArgs<T> : ITaskCompletionEventArgs
	{
		void SetResult(T result);
	}
}
