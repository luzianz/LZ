using LZ.EventHandling;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace LZ.Commanding
{
	public class Command : ICommand, IDisposable
	{
		#region Fields

		private event Action UnsubscribePropertyChanges;

		#endregion

        public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged.RaiseEvent(this);
		}

		protected void ObservePropertyChanges(INotifyPropertyChanged observed, params string[] propertyNames)
		{
			if (observed == null) throw new ArgumentNullException("observed");

			UnsubscribePropertyChanges += observed.ObservePropertyChanges(_ => RaiseCanExecuteChanged(), propertyNames);
		}

		#region ICommand

		public virtual bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;

		public virtual void Execute(object parameter) { }

		#endregion

		#region IDisposable

		public virtual void Dispose()
		{
			if (UnsubscribePropertyChanges != null)
			{
				UnsubscribePropertyChanges();
			}
		}

		#endregion
	}
}