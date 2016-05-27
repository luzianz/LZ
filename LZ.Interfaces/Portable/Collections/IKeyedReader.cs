namespace LZ.Collections {

	/// <summary>
	/// Read from a key/value store without implementing a full <see cref="System.Collections.Generic.IDictionary{TKey, TValue}"/>
	/// </summary>
	public interface IKeyedReader<TKey, TValue> {

		bool TryGetValue(TKey key, out TValue value);
	}
}
