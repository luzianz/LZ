using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LZ
{
	/// <summary>
	/// A key/value store for values that can be subject to garbage collection.
	/// </summary>
	public class WeakDictionary<TKey, TValue> :
	   IKeyedRemovable<TKey>,
	   IKeyedReader<TKey, TValue>,
	   IKeyedWriter<TKey, TValue>,
	   IDisposable where TValue : class
	{
		#region Fields
		private Dictionary<TKey, WeakReference<TValue>> dictionary;
		#endregion

		#region Constructor
		public WeakDictionary()
		{
			this.dictionary = new Dictionary<TKey, WeakReference<TValue>>();
		}
		#endregion

		#region Properties
		public IEnumerable<TValue> AliveReferences
		{
			get
			{
				var deadKeys = new List<TKey>();

				foreach (var kv in dictionary)
				{
					TValue item;
					if (kv.Value.TryGetTarget(out item))
					{
						yield return item;
					}
					else
					{
						deadKeys.Add(kv.Key);
					}
				}
				foreach (var key in deadKeys)
				{
					dictionary.Remove(key);
				}
			}
		}
		#endregion

		#region IKeyedReader<TToken, TValue>
		public bool TryGetValue(TKey key, out TValue value)
		{
			WeakReference<TValue> weakValue;
			if (dictionary.TryGetValue(key, out weakValue))
			{
				if (weakValue.TryGetTarget(out value))
				{
					return true;
				}
				else
				{
					// remove the garbage-collected weak reference
					dictionary.Remove(key);
				}
			}

			value = default(TValue);
			return false;
		}
		#endregion

		#region IKeyedRemovable<TKey>
		public bool Remove(TKey key)
		{
			return dictionary.Remove(key);
		}
		#endregion

		#region IKeyedWriter<TKey, TValue>
		public void SetValue(TKey key, TValue value)
		{
			dictionary[key] = new WeakReference<TValue>(value);
		}
		#endregion

		#region IDisposable
		public void Dispose()
		{
			dictionary.Clear();
		}
		#endregion
	}
}
