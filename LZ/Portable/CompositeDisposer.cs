using System;
using System.Collections.Generic;

namespace LZ
{
	/// <summary>
	/// Combines IDisposable resources to be disposed together.
	/// </summary>
	public class CompositeDisposer : IDisposable
	{
		#region Fields

		private List<IDisposable> disposers;

		#endregion

		#region Constructor

		public CompositeDisposer(params IDisposable[] disposers)
		{
			this.disposers = new List<IDisposable>(disposers);
		}

		#endregion

		public void AddDisposable(IDisposable disposable)
		{
			disposers.Add(disposable);
		}

		#region IDisposable

		public void Dispose()
		{
			if (disposers == null) throw new InvalidOperationException("Object has already been disposed");

			foreach(var disposer in disposers)
			{
				disposer.Dispose();
			}

			disposers.Clear();
			disposers = null;
		}

		#endregion
	}
}
