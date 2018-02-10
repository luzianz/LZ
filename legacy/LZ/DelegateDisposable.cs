using System;

namespace LZ {

	/// <summary>
	/// Uses a delegate for its dispose method.
	/// </summary>
	public class DelegateDisposable : DisposableBase {

		#region Fields

		private readonly Action onDispose;

		#endregion

		#region Constructor

		public DelegateDisposable(Action onDispose) {
			if (onDispose == null) throw new ArgumentNullException(nameof(onDispose));

			this.onDispose = onDispose;
		}

		#endregion

		#region IDisposable
		
		protected override void OnDispose() {
			onDispose();
		}

		#endregion
	}
}
