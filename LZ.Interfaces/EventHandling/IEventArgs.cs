namespace LZ.EventHandling {

	/// <summary>
	/// Generic event arguments
	/// </summary>
	public interface IEventArgs<T> {

		T Args { get; }
	}
}
