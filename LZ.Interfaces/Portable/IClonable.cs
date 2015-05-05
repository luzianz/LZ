namespace LZ
{
	/// <summary>
	/// An object aware of how to clone itself.
	/// </summary>
	public interface ICloneable<T>
	{
		T Clone();
	}
}
