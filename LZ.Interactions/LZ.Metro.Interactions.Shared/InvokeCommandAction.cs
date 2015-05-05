using Microsoft.Xaml.Interactivity;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace LZ.Interactions
{
	/// <summary>
	/// An IAction that invokes an ICommand
	/// </summary>
    public class InvokeCommandAction : FrameworkElement, IAction
	{
		#region Dependency Properties

		#region Command
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandAction), new PropertyMetadata(null));
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}
		#endregion

		#region CommandParameter
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandAction), new PropertyMetadata(null));
		public object CommandParameter
		{
			get { return (object)GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}
		#endregion

		#region ParameterConverter
		public static readonly DependencyProperty ParameterConverterProperty = DependencyProperty.Register("ParameterConverter", typeof(IActionExecuteParameterConverter), typeof(InvokeCommandAction), new PropertyMetadata(null));
		public IActionExecuteParameterConverter ParameterConverter
		{
			get { return (IActionExecuteParameterConverter)GetValue(ParameterConverterProperty); }
			set { SetValue(ParameterConverterProperty, value); }
		}
		#endregion

		#endregion

		#region IAction

		public object Execute(object sender, object parameter)
		{
			if (Command == null)
			{
				return null;
			}
			else
			{
				object commandParameter = CommandParameter;

				// if ParameterConverter is provided, use it.
				if (ParameterConverter != null)
				{
					commandParameter = ParameterConverter.Convert(sender, parameter);
				}

				if (!Command.CanExecute(commandParameter))
				{
					return false;
				}
				else
				{
					Command.Execute(commandParameter);
					return true;
				}
			}
		}

		#endregion
	}
}
