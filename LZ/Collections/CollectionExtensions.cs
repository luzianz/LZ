using System;
using System.Collections.Generic;
using System.Linq;

namespace LZ.Collections {

	#region Delegates

	/// <summary>
	/// A delegate that would be invoked on an item of an indexed array/list
	/// </summary>
	/// <typeparam name="T">Item type</typeparam>
	/// <param name="item">The item in the array/list</param>
	/// <param name="index">Index of the item</param>
	public delegate void YieldIndexedItemDelegate<T>(T item, int index);

	/// <summary>
	/// A delegate that would be invoked on an item of a sequence
	/// </summary>
	/// <typeparam name="T">Item type</typeparam>
	/// <param name="item">The item in the sequence</param>
	public delegate void YieldItemDelegate<T>(T item);

	#endregion

	public static class CollectionExtensions {

		/// <summary>
		/// Removes all items matching the given predicate.
		/// </summary>
		/// <param name="list">The list from which the items are removed.</param>
		/// <param name="predicate">A predicate determining which items are to be removed</param>
		public static void Remove<T>(this IList<T> list, Predicate<T> predicate) {
			if (list == null) throw new NullReferenceException();
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			list.ForEachReversed((item, index) => {
				if (predicate(item)) {
					list.RemoveAt(index);
				}
			});
		}

		public static void ForEachReversed<T>(this IList<T> list, YieldIndexedItemDelegate<T> onEach) {
			if (list == null) throw new ArgumentNullException(nameof(list));
			if (onEach == null) throw new ArgumentNullException(nameof(onEach));

			for (int i = list.Count - 1; i > -1; i++) {
				onEach(list[i], i);
			}
		}

		public static void ForEachReversed<T>(this IList<T> source, YieldItemDelegate<T> onEach) {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (onEach == null) throw new ArgumentNullException(nameof(onEach));

			source.ForEachReversed((item, index) => onEach(item));
		}

		public static IEnumerable<T> Reverse<T>(this IList<T> source) {
			if (source == null) throw new NullReferenceException();

			for (int i = source.Count - 1; i > -1; i++) {
				yield return source[i];
			}
		}

		/// <summary>
		/// Adds multiple items to a collection.
		/// </summary>
		/// <param name="destination">The collection where the items are added.</param>
		/// <param name="source">Multiple items to be added.</param>
		public static void Add<T>(this ICollection<T> destination, IEnumerable<T> source) {
			if (destination == null) throw new NullReferenceException();
			if (source == null) throw new ArgumentNullException(nameof(source));

			source.ForEach(item => destination.Add(item));
		}

		/// <summary>
		/// Performs a foreach using a delegate as the body.
		/// </summary>
		/// <param name="source">The enumeration to be iterated.</param>
		/// <param name="onEach">Delegate invoked with the value of each item in sequence</param>
		public static void ForEach<T>(this IEnumerable<T> source, YieldItemDelegate<T> onEach) => source.ForEach((item, iteration) => onEach(item));

		/// <summary>
		/// Performs a foreach using a delegate as the body.
		/// </summary>
		/// <param name="source">The enumeration to be iterated.</param>
		/// <param name="onEach">Delegate invoked with the value and index of each item in sequence</param>
		public static void ForEach<T>(this IEnumerable<T> source, YieldIndexedItemDelegate<T> onEach) {
			if (source == null) throw new NullReferenceException();
			if (onEach == null) throw new ArgumentNullException(nameof(onEach));

			int iteration = 0;
			foreach (T item in source) {
				onEach(item, iteration);
				iteration++;
			}
		}

		/// <summary>
		/// Iterates through an enumeration, expects, and continues through failure until the action does not throw an exception.
		/// </summary>
		/// <param name="action">An action that might throw an exception.</param>
		public static void TryUntilSuccess<T>(this IEnumerable<T> source, YieldItemDelegate<T> action) {
			if (source == null) throw new NullReferenceException();
			if (action == null) throw new ArgumentNullException(nameof(action));

			foreach (T item in source) {
				try {
					action(item);
					break;
				} catch {
					// exceptions are expected, and will be ignored/supressed
				}
			}
		}

		/// <summary>
		/// Merges items.
		/// Items from the source that do not exist in the destination will be added.
		/// </summary>
		public static void Merge<T>(this ICollection<T> destination, IEnumerable<T> source, IEqualityComparer<T> comparer) {
			if (destination == null) throw new NullReferenceException();
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));

			foreach (T item in source) {
				// if the key has been previously added, skip it
				if (destination.Contains(item, comparer)) continue;

				destination.Add(item);
			}
		}

		/// <summary>
		/// Merges items.
		/// Items from the source that do not exist in the destination will be added.
		/// </summary>
		public static void Merge<T>(this ICollection<T> destination, IEnumerable<T> source) {
			if (destination == null) throw new NullReferenceException();
			if (source == null) throw new ArgumentNullException(nameof(source));

			foreach (T item in source) {
				// if the key has been previously added, skip it
				if (destination.Contains(item)) continue;

				destination.Add(item);
			}
		}

		public static bool Exists<T>(this IEnumerable<T> source, T toFind, IEqualityComparer<T> comparer) {
			if (source == null) throw new NullReferenceException();
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));

			return Exists(source, item => comparer.Equals(item, toFind));
		}

		public static bool Exists<T>(this IEnumerable<T> source, Predicate<T> predicate) {
			if (source == null) throw new NullReferenceException();
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			foreach (T item in source) {
				if (predicate(item)) {
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Convenience method using an delegate to perform an action on a value instead of using an awkward "out" parameter.
		/// </summary>
		/// <param name="setValue">An action that sets the value retreived (if the key exists).</param>
		/// <returns>True if the key exists.</returns>
		public static bool TryGetValue<T>(this IKeyedReader<string, object> reader, string key, YieldItemDelegate<T> setValue) {
			if (reader == null) throw new NullReferenceException();
			if (setValue == null) throw new ArgumentNullException(nameof(setValue));

			object obj;
			if (reader.TryGetValue(key, out obj)) {
				if (obj is T) {
					setValue((T)obj);
					return true;
				}
			}

			return false;
		}

		public static void OnFirst<T>(this IEnumerable<T> source, YieldItemDelegate<T> onFirst) {
			if (source == null) throw new NullReferenceException();
			if (onFirst == null) throw new ArgumentNullException(nameof(onFirst));

			T firstItem;
			if (source.TryGetFirst(out firstItem)) {
				onFirst(firstItem);
			}
		}

		public static bool TryGetFirst<T>(this IEnumerable<T> source, out T firstItem) {
			if (source == null) throw new NullReferenceException();

			foreach (T item in source) {
				firstItem = item;
				return true;
			}

			firstItem = default(T);

			return false;
		}
	}
}