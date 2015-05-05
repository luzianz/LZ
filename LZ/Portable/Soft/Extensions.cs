using System;
using System.Threading.Tasks;

namespace LZ.Soft
{
	/// <summary>
	/// Extension methods that expects certain conditions of failure, thus wont throw exceptions in those cases.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Assigns the out parameter if the source is its type.
		/// </summary>
		/// <returns>True is assignment was successful.</returns>
		public static bool TryAssign<T>(this object inValue, out T outValue)
		{
			if (inValue == null) throw new NullReferenceException();

			if (inValue is T)
			{
				outValue = (T)inValue;
				return true;
			}
			else
			{
				outValue = default(T);
				return false;
			}
		}

		/// <summary>
		/// Performs an action if the source is of the type provided.
		/// </summary>
		/// <param name="action">The action performed on success.</param>
		/// <param name="otherwise">The action performed on failure.</param>
		/// <returns>True if the source is of the provided generic type.</returns>
		public static bool DoAs<T>(this object candidate, Action<T> action, Action otherwise = null)
		{
			if (candidate is T)
			{
				action((T)candidate);
				return true;
			}
			else
			{
				if (otherwise != null)
				{
					otherwise();
				}
				return false;
			}
		}

		/// <summary>
		/// Performs an action if the source is not null.
		/// </summary>
		/// <returns>True if not null.</returns>
		public static bool DoIfNotNull<T>(this T obj, Action<T> action) where T : class
		{
			if (action == null) throw new ArgumentNullException("action");

			if (obj == null)
			{
				return false;
			}
			else
			{
				action(obj);
				return true;
			}
		}

		/// <summary>
		/// Performs an action if the source is not null.
		/// </summary>
		/// <returns>True if not null.</returns>
		public static bool DoIfNotNull<T>(this Nullable<T> obj, Action<T> action) where T : struct
		{
			if (action == null) throw new ArgumentNullException("action");

			if (!obj.HasValue)
			{
				return false;
			}
			else
			{
				action(obj.Value);
				return true;
			}
		}
	}
}
