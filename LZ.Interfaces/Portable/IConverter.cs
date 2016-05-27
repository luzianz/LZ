namespace LZ {

	/// <summary>
	/// Converts one object to another.
	/// </summary>
	/// <typeparam name="TIn">Source object type</typeparam>
	/// <typeparam name="TOut">Converted object type</typeparam>
	public interface IConverter<TIn, TOut> {

		bool TryConvert(TIn input, out TOut output);
	}
}
