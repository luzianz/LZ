using System;
using System.Collections.Generic;

namespace LZ.State
{
	public class StateMachine<TState, TAction> : IStateMachine<TState, TAction>
	{
		#region Events

		public event EventHandler<StateEventArgs<TState>> StateChanged;

		#endregion

		#region Constructor

		public StateMachine()
		{
			Transitions = new List<Transition<TState, TAction>>();
		}

		#endregion

		#region Properties

		public List<Transition<TState, TAction>> Transitions { get; private set; }

		#endregion

		#region Virtual

		protected virtual void OnStateChanged(TState oldState, TState newState) { }

		#endregion

		private void RaiseStateChanged(TState oldState, TState newState)
		{
			if (StateChanged == null) return;

			StateChanged(this, new StateEventArgs<TState>(oldState, newState));
		}

		#region IStateMachine<TState, TAction>

		protected TState _CurrentState;
		public TState CurrentState
		{
			get { return _CurrentState; }
		}

		public bool TryGetNext(TAction action, out TState nextState)
		{
			bool success = false;
			nextState = default(TState);

			foreach (var stateTransition in Transitions)
			{
				if (stateTransition.FromState.Equals(CurrentState) && stateTransition.Action.Equals(action))
				{
					nextState = stateTransition.ToState;
					success = true;
					break;
				}
			}

			return success;
		}

		public bool TryMoveNext(TAction action)
		{
			TState nextState;

			if (TryGetNext(action, out nextState))
			{
				TState previousState = _CurrentState;
				_CurrentState = nextState;
				RaiseStateChanged(previousState, nextState);
				OnStateChanged(previousState, nextState);
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion
	}
}