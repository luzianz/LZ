using System;
using System.Collections.Generic;
using LZ.Collections;

namespace LZ {

	/// <summary>
	/// Combines IDisposable resources to be disposed together.
	/// </summary>
	public class CompositeDisposable : CompositeDisposableBase {
		
		#region Constructor

		public CompositeDisposable(params IDisposable[] disposers) {
			disposers.ForEach(d => AddDisposable(d));
		}

		#endregion

		#region Methods

		public void Add(IDisposable disposable) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(CompositeDisposable));
			if (disposable == null) throw new ArgumentNullException(nameof(disposable));

			AddDisposable(disposable);
		}

		#endregion
	}
}
