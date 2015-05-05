using System;

namespace LZ
{
	/// <summary>
	/// Converts using a delagate for its Convert method.
	/// </summary>
	public class Converter<TIn, TOut> : IConverter<TIn, TOut>
	{
		#region Fields

		private readonly Func<TIn, TOut> converterDelegate;

		#endregion

		#region Constructor

		public Converter(Func<TIn, TOut> converterDelegate)
		{
			if (converterDelegate == null) throw new ArgumentNullException("converterDelegate");

			this.converterDelegate = converterDelegate;
		}

		#endregion

		#region IConverter<TIn, TOut>

		public TOut Convert(TIn data)
		{
			return converterDelegate(data);
		}

		#endregion
	}
}
