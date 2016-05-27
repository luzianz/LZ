namespace LZ.ComponentModel {

	/// <summary>
	/// An object aware of how to clone itself.
	/// </summary>
	public interface IExportable<T> {

		T Export();
	}
}
