namespace LZ
{
	/// <summary>
	/// Creates new objects.
	/// An implementation would decide what to pass to its constructor, and how the object is initialized.
	/// </summary>
	public interface ICreator<out T>
	{
		T CreateNew();
	}
}