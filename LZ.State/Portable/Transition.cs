using System;

namespace LZ.State {
	public sealed class Transition<TState, TAction> : IEquatable<Transition<TState, TAction>> {

		#region Constructor

		public Transition(TState fromState, TAction action, TState toState) {
			if (fromState == null) throw new ArgumentNullException(nameof(fromState));
			if (action == null) throw new ArgumentNullException(nameof(action));
			if (toState == null) throw new ArgumentNullException(nameof(toState));

			FromState = fromState;
			Action = action;
			ToState = toState;
		}

		#endregion

		#region Properties

		public TState FromState { get; }
		public TAction Action { get; }
		public TState ToState { get; }

		#endregion

		#region Object

		public override bool Equals(object other) {
			return Equals(other as Transition<TState, TAction>);
		}

		public override int GetHashCode() {
			int hash = 3;
			hash = (hash * 5) + FromState.GetHashCode();
			hash = (hash * 7) + ToState.GetHashCode();
			hash = (hash * 11) + Action.GetHashCode();

			return hash;
		}

		#endregion

		#region IEquatable<Transition<TState, TAction>>

		public bool Equals(Transition<TState, TAction> other) {
			if (other == null) return false;
			else {
				return ToState.Equals(other.ToState)
					&& FromState.Equals(other.FromState)
					&& Action.Equals(other.Action);
			}
		}

		#endregion
	}
}
