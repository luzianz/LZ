namespace LZ
{
	/// <summary>
	/// Converts one object to another.
	/// </summary>
	/// <typeparam name="TIn">Source object type</typeparam>
	/// <typeparam name="TOut">Converted object type</typeparam>
	public interface IConverter<in TIn, out TOut>
	{
		TOut Convert(TIn data);
	}
}
