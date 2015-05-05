namespace LZ
{
	/// <summary>
	/// An object aware of how it copies data from an object of the same type to itself.
	/// </summary>
	public interface ICopyable<T>
	{
		void Copy(T source);
	}
}
