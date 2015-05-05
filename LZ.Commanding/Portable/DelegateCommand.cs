using System;

namespace LZ.Commanding
{
	public class DelegateCommand : Command
	{
		#region Fields

		private readonly Action<object> _executeAction;
		private readonly Predicate<object> _canExecutePredicate;

		#endregion

		#region Ctor

		public DelegateCommand(Action<object> executeAction = null, Predicate<object> canExecutePredicate = null)
		{
			_executeAction = executeAction;
			_canExecutePredicate = canExecutePredicate;
		}

		#endregion

		#region Command

		public override bool CanExecute(object parameter)
		{
			return _canExecutePredicate == null || _canExecutePredicate(parameter);
		}

		public override void Execute(object parameter)
		{
			if (_executeAction != null) _executeAction(parameter);
		}

		#endregion
	}
}