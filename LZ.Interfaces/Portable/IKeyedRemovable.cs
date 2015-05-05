namespace LZ
{
	/// <summary>
	/// Remove from a key/value store without implementing a full <see cref="System.Collections.Generic.IDictionary&lt;TKey, TValue&gt;"/>
	/// </summary>
	public interface IKeyedRemovable<TKey>
	{
		bool Remove(TKey key);
	}
}
