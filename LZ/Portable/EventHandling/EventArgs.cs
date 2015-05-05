using System;

namespace LZ.EventHandling
{
	public class EventArgs<T> : EventArgs, IEventArgs<T>
	{
		#region Constructor

		public EventArgs(T args)
		{
			this.Args = args;
		}

		#endregion

		#region IEventArgs<T>

		public T Args { get; private set; }

		#endregion
	}
}