using System;

namespace LZ.State
{
	public sealed class Transition<TState, TAction>
	{
		#region Constructor

		public Transition(TState fromState, TAction action, TState toState)
		{
			if (fromState == null) throw new ArgumentNullException("fromState");
			if (action == null) throw new ArgumentNullException("action");
			if (toState == null) throw new ArgumentNullException("toState");

			FromState = fromState;
			Action = action;
			ToState = toState;
		}

		#endregion

		#region Properties

		public TState FromState { get; private set; }
		public TAction Action { get; private set; }
		public TState ToState { get; private set; }

		#endregion

		#region Object

		public override bool Equals(object other)
		{
			if (other == null) return false;
			else if (other is Transition<TState, TAction>)
			{
				var otherTransition = other as Transition<TState, TAction>;
				return ToState.Equals(otherTransition.ToState)
					&& FromState.Equals(otherTransition.FromState)
					&& Action.Equals(otherTransition.Action);
			}
			else return false;
		}
		public override int GetHashCode()
		{
			int hash = 3;
			hash = (hash * 5) + FromState.GetHashCode();
			hash = (hash * 7) + ToState.GetHashCode();
			hash = (hash * 11) + Action.GetHashCode();

			return hash;
		}

		#endregion
	}
}
