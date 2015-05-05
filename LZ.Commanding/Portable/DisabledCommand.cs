namespace LZ.Commanding
{
	/// <summary>
	/// A command that cannot execute. Useful in debugging and design-time scenarios.
	/// </summary>
	public class DisabledCommand : Command
	{
		public override bool CanExecute(object parameter)
		{
			return false;
		}
	}
}
