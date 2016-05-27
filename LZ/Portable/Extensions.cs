using System;

namespace LZ {

	public static class Extensions {

		/// <summary>
		/// Performs a "using" on an <see cref="System.IDiposable"/> with an action delegate as the body.
		/// </summary>
		public static void Using(this IDisposable disposable, Action action) {
			if (disposable == null) throw new NullReferenceException();
			if (action == null) throw new ArgumentNullException(nameof(action));

			using (disposable) {
				action();
			}
		}
		
		public static TOut Convert<Tin, TOut>(this IConverter<Tin, TOut> converter, Tin input, bool throwOnFail = true) {
			if (converter == null) throw new NullReferenceException();

			TOut output;
			if (!converter.TryConvert(input, out output) && throwOnFail) {
				throw new ArgumentException("Cannot convert", nameof(input));
			}

			return output;
		}
	}
}
