namespace LZ.State
{
	/// <summary>
	/// Represents an object aware of how it manages state.
	/// </summary>
	public interface IStateMachine<TState, TAction>
	{
		TState CurrentState { get; }

		/// <returns>false if the current state cannot change with the given action</returns>
		bool TryGetNext(TAction action, out TState nextState);

		/// <returns>false if the current state cannot change with the given action</returns>
		bool TryMoveNext(TAction action);
	}
}