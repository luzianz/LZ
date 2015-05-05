using System;
using System.Threading.Tasks;

namespace LZ.Commanding
{
	/// <summary>
	/// A command that can execute only when it is not busy.
	/// This command is busy after it has been executed asynchronously, and before it completes.
	/// </summary>
	public abstract class AsyncCommand : Command
	{
		#region Fields

		private ObservableBox<bool> isExecuting;

		#endregion

		#region Properties

		protected bool IsExecuting
		{
			get { return isExecuting; }
		}

		#endregion

		#region Constructor

		public AsyncCommand()
		{
			isExecuting = new ObservableBox<bool>(false, RaiseCanExecuteChanged);
		}

		#endregion

		#region Abstract

		protected abstract Task ExecuteAsync(object parameter);

		#endregion

		#region Command

		public async override void Execute(object parameter)
		{
			try
			{
				isExecuting.Value = true;
				await ExecuteAsync(parameter);
			}
			finally
			{
				isExecuting.Value = false;
			}
		}

		public override bool CanExecute(object parameter)
		{
			return !isExecuting;
		}

		#endregion
	}
}
