namespace LZ.Collections {

	/// <summary>
	/// Remove from a key/value store without implementing a full <see cref="System.Collections.Generic.IDictionary{TKey, TValue}"/>
	/// </summary>
	public interface IKeyedRemovable<TKey> {

		bool Remove(TKey key);
	}
}
