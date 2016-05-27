using System;

namespace LZ.Transactions {

	public abstract class TransactionBase : DisposableBase, ITransaction {

		#region Fields

		private Action rollBack;

		#endregion

		#region Properties

		public bool IsCommitted { get; private set; } = false;

		#endregion

		#region Methods

		public void Commit() => IsCommitted = true;

		protected abstract void Rollback();

		#endregion

		#region DisposableBase

		protected override void OnDispose() {
			if (!IsCommitted) {
				Rollback();
			}
		}

		#endregion
	}
}
