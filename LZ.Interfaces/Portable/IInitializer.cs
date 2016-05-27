namespace LZ {
	
	/// <summary>
	/// The implementation knows how to initialize another object.
	/// </summary>
	/// <typeparam name="T">Type of object to be initialized.</typeparam>
	public interface IInitializer<T> {

		void Initialize(T subject);
	}
}
