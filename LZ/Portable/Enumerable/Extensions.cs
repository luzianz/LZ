using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LZ.Enumerable
{
	public static class Extensions
	{
		/// <summary>
		/// Removes all items matching the given predicate.
		/// </summary>
		/// <param name="source">The list from which the items are removed.</param>
		/// <param name="predicate">A predicate representing the items to be removed</param>
		public static void Remove<T>(this IList<T> source, Func<T, bool> predicate)
		{
			if (source == null) throw new NullReferenceException();

			var matches = source.Where(predicate).ToArray();

			matches.ForEach(match => source.Remove(match));
		}

		/// <summary>
		/// Adds multiple items to a collection.
		/// </summary>
		/// <param name="destination">The collection where the items are added.</param>
		/// <param name="source">Multiple items to be added.</param>
		public static void Add<T>(this ICollection<T> destination, IEnumerable<T> source)
		{
			if (destination == null) throw new NullReferenceException();

			source.ForEach(item => destination.Add(item));
		}

		/// <summary>
		/// Performs a foreach using a delegate as the body.
		/// </summary>
		/// <param name="enumerable">The enumeration to be iterated.</param>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			if (enumerable == null) throw new NullReferenceException();
			if (action == null) throw new ArgumentNullException("action");

			foreach (T item in enumerable) action(item);
		}

		/// <summary>
		/// Iterates through an enumeration, expects, and continues through failure until the action does not throw an exception.
		/// </summary>
		/// <param name="action">An action that might throw an exception.</param>
		public static void TryUntilSuccess<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			if (enumeration == null) throw new NullReferenceException();
			if (action == null) throw new ArgumentNullException("action");

			foreach (T item in enumeration)
			{
				try
				{
					action(item);
					break;
				}
				catch
				{
					// exceptions are expected, and will be ignored/supressed
				}
			}
		}

		/// <summary>
		/// Merges items.
		/// Items from the source that do not exist in the destination will be added.
		/// </summary>
		public static void Merge<T>(this ICollection<T> destination, IEnumerable<T> source, IEqualityComparer<T> comparer)
		{
			if (destination == null) throw new NullReferenceException();
			if (source == null) throw new ArgumentNullException("source");
			if (comparer == null) throw new ArgumentNullException("comparer");

			foreach (T item in source)
			{
				// if the key has been previously added, skip it
				if (destination.Contains(item, comparer)) continue;

				destination.Add(item);
			}
		}

		/// <summary>
		/// Merges items.
		/// Items from the source that do not exist in the destination will be added.
		/// </summary>
		public static void Merge<T>(this ICollection<T> destination, IEnumerable<T> source)
		{
			if (destination == null) throw new NullReferenceException();
			if (source == null) throw new ArgumentNullException("source");

			foreach (T item in source)
			{
				// if the key has been previously added, skip it
				if (destination.Contains(item)) continue;

				destination.Add(item);
			}
		}

		/// <summary>
		/// Joins an enumeration to a string which can be delimited.
		/// </summary>
		/// <param name="delimeter">The string inserted between in item.</param>
		/// <param name="itemConverter">A delegate that converts each item into a string. Otherwise "ToString()" is used.</param>
		/// <returns>Resulting joined string</returns>
		public static string JoinToString<T>(this IEnumerable<T> enumerable, string delimeter = null, Func<T, string> itemConverter = null)
		{
			if (enumerable == null) throw new NullReferenceException();

			StringBuilder stringBuilder = null;
			bool isFirst = true;

			foreach (T item in enumerable)
			{
				if (isFirst)
				{
					stringBuilder = new StringBuilder();
					isFirst = false;
				}
				else if (delimeter != null)
				{
					stringBuilder.Append(delimeter);
				}

				if (itemConverter == null)
				{
					stringBuilder.Append(item);
				}
				else
				{
					stringBuilder.Append(itemConverter(item));
				}
			}

			return stringBuilder == null ? String.Empty : stringBuilder.ToString();
		}
	}
}