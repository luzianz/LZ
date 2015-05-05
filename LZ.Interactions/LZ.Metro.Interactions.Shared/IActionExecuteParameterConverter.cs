namespace LZ.Interactions
{
	/// <summary>
	/// The IAction's Execute method is provided two parameters (sender and parameter). But the ICommand's
	/// CanExecute/Execute methods take only one parameter.
	/// This interface gives you the opportunity to extract information from the sender to optionally
	/// provide to the parameter of CanExecute/Execute.
	/// </summary>
    public interface IActionExecuteParameterConverter
    {
		object Convert(object sender, object parameter);
    }
}
