using LZ.Soft;
using System;

namespace LZ
{
	public class EditableObject<T> : EditableObjectBase, IBox<T> where T : ICopyable<T>, ICloneable<T>
	{
		#region Fields
		private readonly T source;
		private T backup;
		#endregion

		public EditableObject(T source)
		{
			if (source == null) throw new ArgumentNullException("source");
			this.source = source;
		}

		#region IBox<T>
		public T Value
		{
			get
			{
				return source;
			}
		}
		#endregion

		#region EditableObjectBase
		protected override void Backup()
		{
			backup = source.Clone();
		}

		protected override void Restore()
		{
			source.Copy(backup);
			backup.DoAs<IDisposable>(d => d.Dispose());
		}
		#endregion
	}
}
