using System;

namespace LZ.State {
	public class StateEventArgs<T> : EventArgs {

		#region Properties

		public T OldState { get; }
		public T NewState { get; }

		#endregion

		#region Constructor

		public StateEventArgs(T oldState, T newState) {
			OldState = oldState;
			NewState = newState;
		}

		#endregion
	}
}