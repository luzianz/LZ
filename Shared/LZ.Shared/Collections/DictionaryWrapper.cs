using System.Collections.Generic;

namespace LZ.Collections {

	public class DictionaryWrapper<K, V> : IKeyedReader<K, V>, IKeyedWriter<K, V>, IKeyedRemovable<K> {

		#region Fields

		private readonly IDictionary<K, V> dictionary;

		#endregion

		#region Constructors

		public DictionaryWrapper() : this(new Dictionary<K, V>()) { }

		public DictionaryWrapper(IDictionary<K, V> dictionary) {
			this.dictionary = dictionary;
		}

		#endregion

		#region IKeyedRemovable<K>

		public bool Remove(K key) {
			return dictionary.Remove(key);
		}

		#endregion

		#region IKeyedWriter<K, V>

		public void SetValue(K key, V value) {
			dictionary[key] = value;
		}

		#endregion

		#region IKeyedReader<K, V>

		public bool TryGetValue(K key, out V value) {
			return dictionary.TryGetValue(key, out value);
		}

		#endregion
	}
}
