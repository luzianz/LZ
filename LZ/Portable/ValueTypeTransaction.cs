using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZ
{
	/// <summary>
	/// Rolls back a value type when an exception occurs. 
	/// </summary>
	/// <typeparam name="T">Type of the value.</typeparam>
	public class ValueTypeTransaction<T> : IDisposable where T : struct
	{
		#region Fields
		private T backup;
		private Action<T> recovery;
		private bool isEditing;
		#endregion

		/// <param name="source">The original value</param>
		/// <param name="recovery">A delegate intended to set the source to it original value.</param>
		public ValueTypeTransaction(T source, Action<T> recovery)
		{
			if (recovery == null) throw new ArgumentNullException("recovery");
			this.recovery = recovery;

			isEditing = true;

			backup = source;
		}

		public void Commit()
		{
			if (!isEditing) throw new InvalidOperationException();
			isEditing = false;

			backup = default(T);
			recovery = null;
		}

		public void Dispose()
		{
			if (isEditing)
			{
				isEditing = false;
				recovery(backup);
				backup = default(T);
				recovery = null;
			}
		}
	}
}
