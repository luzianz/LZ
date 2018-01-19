using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LZ.State {
	public class StateMachine<TState, TAction> : IStateMachine<TState, TAction>, INotifyPropertyChanged {

		#region Events

		public event EventHandler<StateEventArgs<TState>> StateChanged;

		#endregion

		#region Constructor

		public StateMachine() {
			Transitions = new List<Transition<TState, TAction>>();
		}

		#endregion

		#region Properties

		public List<Transition<TState, TAction>> Transitions { get; }

		#endregion

		#region Virtual

		protected virtual void OnStateChanged(TState oldState, TState newState) { }

		#endregion

		private void RaiseStateChanged(TState oldState, TState newState) {
			if (StateChanged != null) {
				var stateChanged = StateChanged;
				stateChanged(this, new StateEventArgs<TState>(oldState, newState));
			}

			if (PropertyChanged != null) {
				var propertyChanged = PropertyChanged;
				propertyChanged(this, new PropertyChangedEventArgs("CurrentState"));
			}
		}

		#region IStateMachine<TState, TAction>
		
		public TState CurrentState { get; protected set; }

		public bool TryPeekNext(TAction action, out TState nextState) {
			bool success = false;
			nextState = default(TState);

			foreach (var stateTransition in Transitions) {
				if (stateTransition.FromState.Equals(CurrentState) && stateTransition.Action.Equals(action)) {
					nextState = stateTransition.ToState;
					success = true;
					break;
				}
			}

			return success;
		}

		public bool TryMoveNext(TAction action) {
			TState nextState;

			if (TryPeekNext(action, out nextState)) {
				TState previousState = CurrentState;
				CurrentState = nextState;
				RaiseStateChanged(previousState, nextState);
				OnStateChanged(previousState, nextState);
				return true;
			} else {
				return false;
			}
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}