using System;

namespace LZ
{
	/// <summary>
	/// Rolls back a reference type when an exception occurs. 
	/// </summary>
	/// <typeparam name="T">Type of the value.</typeparam>
	public class ObjectTransaction<T> : IDisposable where T : class, ICopyable<T>, new()
	{
		#region Fields

		private T backup;
		private T source;
		private bool isEditing;

		#endregion

		#region Constructor

		public ObjectTransaction(ref T source)
		{
			isEditing = true;

			backup = new T();
			backup.Copy(source);

			this.source = source;
		}

		#endregion

		public void Commit()
		{
			if (!isEditing) throw new InvalidOperationException();
			isEditing = false;

			backup = null;
		}

		#region IDisposable

		public void Dispose()
		{
			if (isEditing)
			{
				isEditing = false;
				source.Copy(backup);
				backup = null;
			}
		}

		#endregion
	}
}
