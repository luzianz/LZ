namespace LZ.State
{
	public class ProcessStateMachine : StateMachine<ProcessState, ProcessActions>
	{
		public ProcessStateMachine()
		{
			Transitions.Add(
				new Transition<ProcessState, ProcessActions>(
					ProcessState.Inactive,
					ProcessActions.Begin,
					ProcessState.Active));
			Transitions.Add(
				new Transition<ProcessState, ProcessActions>(
					ProcessState.Active,
					ProcessActions.End,
					ProcessState.Inactive));
		}
	}
}