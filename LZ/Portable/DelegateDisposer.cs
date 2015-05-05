using System;

namespace LZ
{
	/// <summary>
	/// Uses a delegate for its dispose method.
	/// </summary>
	public class DelegateDisposer : IDisposable
	{
		#region Fields

		private readonly Action onDispose;

		#endregion

		#region Constructor

		public DelegateDisposer(Action onDispose)
		{
			if (onDispose == null) throw new ArgumentNullException("onDispose");

			this.onDispose = onDispose;
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			onDispose();
		}

		#endregion
	}
}
