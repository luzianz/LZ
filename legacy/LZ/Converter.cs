using System;

namespace LZ {

	/// <summary>
	/// Converts using a delagate for its Convert method.
	/// </summary>
	public class Converter<TIn, TOut> : IConverter<TIn, TOut> {

		#region Fields

		private readonly Func<TIn, TOut> convert;

		#endregion

		#region Constructor

		public Converter(Func<TIn, TOut> convert) {
			if (convert == null) throw new ArgumentNullException(nameof(convert));

			this.convert = convert;
		}

		#endregion

		#region IConverter<TIn, TOut>
		
		public bool TryConvert(TIn input, out TOut output) {
			try {
				output = convert(input);
				return true;
			} catch {
				output = default(TOut);
				return false;
			}
		}

		#endregion
	}
}
