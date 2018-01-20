using System;

namespace LZ.Windows.Serialization {

	public static class Extensions {

		public static void TryGetValueAndRemove<T>(this IPersistence source, string key, Action<T> onFound) {
			object value;
			if (source.TryGetValue(key, out value)) {
				onFound((T)value);
				source.Remove(key);
			}
		}
	}
}
