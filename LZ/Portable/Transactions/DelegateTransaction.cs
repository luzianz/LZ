using System;

namespace LZ.Transactions {

	/// <summary>
	/// Rolls back a value type when an exception occurs. 
	/// </summary>
	/// <typeparam name="T">Type of the value.</typeparam>
	public class DelegateTransaction<T> : TransactionBase {

		#region Fields
		
		private Action rollback;

		#endregion

		#region Constructor

		/// <param name="rollback">A delegate intended to set the source to its original value.</param>
		public DelegateTransaction(Action rollback) {
			if (rollback == null) throw new ArgumentNullException(nameof(rollback));

			this.rollback = rollback;
		}

		#endregion

		#region TransactionBase

		protected override void Rollback() {
			rollback();
		} 

		#endregion
	}
}
