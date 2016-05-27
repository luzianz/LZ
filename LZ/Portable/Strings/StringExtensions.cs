using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LZ.Collections;

namespace LZ.Strings {

	public static class StringExtensions {

		public static void Split(this string str, Action<string> onChunk, Action<string> onRemaining, params char[] delimiters) {
			int previousDelimitedIndex = -1;
			for (int i = 0; i < str.Length; i++) {
				if (delimiters.Any(c => str[i] == c)) {
					// if previous character was also a delimiter, ignore
					if (i - 1 == previousDelimitedIndex) {
						// ignore
					} else {
						int startIndex = previousDelimitedIndex + 1;
						int len = i - startIndex;
						string line = str.Substring(startIndex, len);
						onChunk(line);
					}

					previousDelimitedIndex = i;
				} else if (i == str.Length - 1) { // final character isn't a delimiter
					int startIndex = previousDelimitedIndex + 1;
					int len = i - startIndex;
					string remaining = str.Substring(startIndex, len);
					onRemaining(remaining);
				}
			}
		}

		public static string JoinToString<T>(this IEnumerable<T> source, object delimiter = null, Func<T, string> toString = null) {
			return delimiter == null ? _JoinToString(source, toString) : _JoinToString(source, delimiter, toString);
		}

		#region Private

		private static void _Join<T>(IEnumerable<T> source, Action<T> appendItem, Action appendDelimiter = null) {
			source.ForEach((item, iteration) => {
				if (appendDelimiter != null && iteration > 0) appendDelimiter();
				appendItem(item);
			});
		}

		private static string _JoinToString<T, D>(IEnumerable<T> source, D delimiter, Func<T, string> toString) {
			var sb = new StringBuilder();
			_Join(source, item => sb.Append(toString == null ? item.ToString() : toString(item)), () => sb.Append(delimiter));

			return sb.ToString();
		}

		private static string _JoinToString<T>(IEnumerable<T> source, Func<T, string> toString) {
			var sb = new StringBuilder();
			_Join(source, item => sb.Append(toString == null ? item.ToString() : toString(item)));

			return sb.ToString();
		}

		#endregion
	}
}