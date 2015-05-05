using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LZ
{
	public static class Extensions
	{
		/// <summary>
		/// Performs a "using" on an <see cref="System.IDiposable"/> with an action delegate as the body.
		/// </summary>
		public static void Using(this IDisposable disposable, Action action)
		{
			if (disposable == null) throw new NullReferenceException();
			if (action == null) throw new ArgumentNullException("action");

			using(disposable)
			{
				action();
			}
		}

		/// <summary>
		/// Wraps a dictionary into a <see cref="LZ.IKeyedWriter&lt;TKey, TValue&gt;"/>.
		/// </summary>
		public static IKeyedWriter<TKey, TValue> ToWriter<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
		{
			return new DictionaryKeyedWriter<TKey, TValue>(dictionary);
		}

		/// <summary>
		/// Wraps a dictionary into a <see cref="LZ.IKeyedReader&lt;TKey, TValue&gt;"/>.
		/// </summary>
		public static IKeyedReader<TKey, TValue> ToReader<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
		{
			return new DictionaryKeyedReader<TKey, TValue>(dictionary);
		}

		/// <summary>
		/// Convenience method using an delegate to perform an action on a value instead of using an awkward "out" parameter.
		/// </summary>
		/// <param name="setValue">An action that sets the value retreived (if the key exists).</param>
		/// <returns>True if the key exists.</returns>
		public static bool TryGetValue<T>(this IKeyedReader<string, object> reader, string key, Action<T> setValue)
		{
			object obj;
			if (reader.TryGetValue(key, out obj))
			{
				if (obj is T)
				{
					setValue((T)obj);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Wrapper for a dictionary.
		/// </summary>
		private class DictionaryKeyedWriter<TKey, TValue> : IKeyedWriter<TKey, TValue>
		{
			private readonly IDictionary<TKey, TValue> dictionary;
			public DictionaryKeyedWriter(IDictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null) throw new ArgumentNullException("dictionary");
				this.dictionary = dictionary;
			}
			public void SetValue(TKey key, TValue value)
			{
				dictionary[key] = value;
			}
		}

		/// <summary>
		/// Wrapper for a dictionary.
		/// </summary>
		private class DictionaryKeyedReader<TKey, TValue> : IKeyedReader<TKey, TValue>
		{
			private readonly IDictionary<TKey, TValue> dictionary;
			public DictionaryKeyedReader(IDictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null) throw new ArgumentNullException("dictionary");
				this.dictionary = dictionary;
			}
			public bool TryGetValue(TKey key, out TValue value)
			{
				return dictionary.TryGetValue(key, out value);
			}
		}
	}
}
