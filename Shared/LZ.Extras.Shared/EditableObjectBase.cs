namespace LZ.ComponentModel {
	public abstract class EditableObjectBase
#if (NETSTANDARD2_0 || IEDITABLEOBJECT)
		: System.ComponentModel.IEditableObject
#endif
	{

		#region Fields

		private readonly object editLock = new object();
		private bool isEditing;

		#endregion

		#region IEditableObject

		public void BeginEdit() {
			if (isEditing) return;

			lock (editLock) {
				if (isEditing) return;
				isEditing = true;
				Backup();
			}
		}

		public void CancelEdit() {
			if (!isEditing) return;

			lock (editLock) {
				if (!isEditing) return;
				Restore();
				isEditing = false;
			}
		}

		public void EndEdit() {
			if (!isEditing) return;

			lock (editLock) {
				if (!isEditing) return;
				isEditing = false;
			}
		}

		#endregion

		#region Abstract

		protected abstract void Backup();
		protected abstract void Restore();

		#endregion
	}
}
