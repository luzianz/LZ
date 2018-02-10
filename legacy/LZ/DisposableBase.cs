using System;

namespace LZ {

	/// <summary>
	/// Provides a derrived classes to dispose only once, and a means to determine whether it has been disposed.
	/// </summary>
	public abstract class DisposableBase : IDisposable {

		#region Properties

		protected bool IsDisposed { get; private set; } = false;

		#endregion

		#region IDisposable

		public void Dispose() {
			if (IsDisposed) return;

			OnDispose();
			IsDisposed = true;
		}

		#endregion

		#region Abstract

		protected abstract void OnDispose(); 

		#endregion
	}
}
