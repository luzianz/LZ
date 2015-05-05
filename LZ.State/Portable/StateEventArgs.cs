using System;

namespace LZ.State
{
    public class StateEventArgs<T> : EventArgs
    {
        public T OldState { get; private set; }
        public T NewState { get; private set; }

        public StateEventArgs(T oldState, T newState)
        {
            OldState = oldState;
            NewState = newState;
        }
    }
}