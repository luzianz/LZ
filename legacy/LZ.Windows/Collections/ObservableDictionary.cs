using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Collections;

namespace LZ.Collections {

	public class ObservableDictionary<K, V> : IObservableMap<K, V> {
		
		#region Fields

		private Dictionary<K, V> _dictionary = new Dictionary<K, V>();

		#endregion

		#region Private Methods

		private void InvokeMapChanged(CollectionChange change, K key) {
			MapChanged?.Invoke(this, new ObservableDictionaryChangedEventArgs(change, key));
		}

		#endregion

		#region IObservableMap<K, V>

		public event MapChangedEventHandler<K, V> MapChanged;

		#endregion

		#region IDictionary<K, V>

		public V this[K key] {
			get {
				return this._dictionary[key];
			}
			set {
				this._dictionary[key] = value;
				this.InvokeMapChanged(CollectionChange.ItemChanged, key);
			}
		}

		public void Add(K key, V value) {
			this._dictionary.Add(key, value);
			this.InvokeMapChanged(CollectionChange.ItemInserted, key);
		}

		public bool Remove(K key) {
			if (this._dictionary.Remove(key)) {
				this.InvokeMapChanged(CollectionChange.ItemRemoved, key);
				return true;
			}
			return false;
		}

		public ICollection<K> Keys {
			get { return this._dictionary.Keys; }
		}

		public ICollection<V> Values {
			get { return this._dictionary.Values; }
		}

		public bool ContainsKey(K key) {
			return this._dictionary.ContainsKey(key);
		}

		public bool TryGetValue(K key, out V value) {
			return this._dictionary.TryGetValue(key, out value);
		}

		#endregion

		#region ICollection<KeyValuePair<K, V>>

		public void Add(KeyValuePair<K, V> item) {
			this.Add(item.Key, item.Value);
		}

		public bool Remove(KeyValuePair<K, V> item) {
			V currentValue;
			if (this._dictionary.TryGetValue(item.Key, out currentValue) &&
				Object.Equals(item.Value, currentValue) && this._dictionary.Remove(item.Key)) {
				this.InvokeMapChanged(CollectionChange.ItemRemoved, item.Key);
				return true;
			}

			return false;
		}

		public void Clear() {
			var priorKeys = this._dictionary.Keys.ToArray();

			this._dictionary.Clear();

			foreach (var key in priorKeys) {
				this.InvokeMapChanged(CollectionChange.ItemRemoved, key);
			}
		}

		public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) {
			int arraySize = array.Length;
			foreach (var pair in this._dictionary) {
				if (arrayIndex >= arraySize) break;
				array[arrayIndex++] = pair;
			}
		}

		public bool Contains(KeyValuePair<K, V> item) {
			return this._dictionary.Contains(item);
		}

		public int Count {
			get { return this._dictionary.Count; }
		}

		public bool IsReadOnly {
			get { return false; }
		}

		#endregion

		#region IEnumerable<KeyValuePair<K, V>

		public IEnumerator<KeyValuePair<K, V>> GetEnumerator() {
			return this._dictionary.GetEnumerator();
		}

		#endregion

		#region IEnumerable

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this._dictionary.GetEnumerator();
		}

		#endregion

		#region Types

		private class ObservableDictionaryChangedEventArgs : IMapChangedEventArgs<K> {

			#region Constructor

			public ObservableDictionaryChangedEventArgs(CollectionChange change, K key) {
				this.CollectionChange = change;
				this.Key = key;
			}

			#endregion

			#region IMapChangedEventArgs<K>

			public CollectionChange CollectionChange { get; }
			public K Key { get; }

			#endregion
		}

		#endregion
	}
}