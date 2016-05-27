namespace LZ.ComponentModel {

	/// <summary>
	/// An object aware of how it imports data from an external object to itself.
	/// Intended to set the initial state of the object (fields instead of properties)
	/// </summary>
	public interface IImportable<T> {

		/// <summary>
		/// Copies data from an external source.
		/// </summary>
		/// <param name="source">The object from which you are importing data from.</param>
		void ImportFrom(T source);
	}
}