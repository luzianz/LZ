namespace LZ
{
	/// <summary>
	/// Write to a key/value store without implementing a full <see cref="System.Collections.Generic.IDictionary&lt;TKey, TValue&gt;"/>
	/// </summary>
	public interface IKeyedWriter<TKey, TValue>
	{
		void SetValue(TKey key, TValue value);
	}
}
