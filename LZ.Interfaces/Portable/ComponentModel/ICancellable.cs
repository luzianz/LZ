using System;

namespace LZ.ComponentModel {

	public interface ICancellable {

		event EventHandler CanCancelChanged;

		bool CanCancel { get; }

		void Cancel();
	}
}
