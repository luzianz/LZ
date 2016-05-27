using System;
using System.Collections.Generic;
using LZ.Collections;

namespace LZ {

	public class CompositeDisposableBase : DisposableBase {

		#region Fields

		private List<IDisposable> disposers = new List<IDisposable>();

		#endregion

		#region Methods
		
		protected void AddDisposable(IDisposable disposable) {
			if (disposable == null) throw new ArgumentNullException(nameof(disposable));

			disposers.Add(disposable);
		}

		#endregion

		#region DisposableBase

		protected override void OnDispose() {
			disposers.ForEach(d => d.Dispose());
			disposers.Clear();
			disposers = null;
		}

		#endregion
	}
}
