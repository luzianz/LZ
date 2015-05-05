using System;
using System.Threading.Tasks;

namespace LZ.Commanding
{
	public class DelegateAsyncCommand : AsyncCommand
	{
		#region Fields

		private readonly Func<object, Task> executeAsyncFunc;
		private readonly Func<object, bool, bool> canExecutePredicate;

		#endregion

		#region Ctor

		public DelegateAsyncCommand(Func<object, Task> executeAsyncFunc = null, Func<object, bool, bool> canExecutePredicate = null)
		{
			this.executeAsyncFunc = executeAsyncFunc;
			this.canExecutePredicate = canExecutePredicate;
		}

		#endregion

		#region AsyncCommand

		protected override async Task ExecuteAsync(object parameter)
		{
			if (executeAsyncFunc == null) return;

			await executeAsyncFunc(parameter);
		}

		public override bool CanExecute(object parameter)
		{
			if (canExecutePredicate == null)
			{
				return base.CanExecute(parameter);
			}
			else
			{
				return canExecutePredicate(parameter, IsExecuting);
			}
		}

		#endregion
	}
}
