using System;
using System.Collections.Generic;

namespace LZ.Format.Web
{
	internal class QueryParameterComparer : IComparer<KeyValuePair<string, string>>
	{
		#region Singleton

		private static Lazy<QueryParameterComparer> lazyInstance = new Lazy<QueryParameterComparer>(() => new QueryParameterComparer());

		public static QueryParameterComparer Instance
		{
			get
			{
				return lazyInstance.Value;
			}
		}

		// prevent instantiation outside the class
		private QueryParameterComparer() { }

		#endregion

		#region IComparer<KeyValuePair<string, string>>

		int IComparer<KeyValuePair<string, string>>.Compare(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
		{
			if (x.Key == y.Key)
			{
				return string.Compare(x.Value, y.Value);
			}
			else
			{
				return string.Compare(x.Key, y.Key);
			}
		}

		#endregion
	}
}
