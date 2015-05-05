using System;

namespace LZ.State
{
	public static class Extensions
	{
		public static void MoveNextOrFail<TState, TAction>(this IStateMachine<TState, TAction> stateMachine, TAction action)
		{
			if (!stateMachine.TryMoveNext(action)) throw new InvalidOperationException();
		}
	}
}