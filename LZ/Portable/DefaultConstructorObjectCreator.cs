namespace LZ {

	public class DefaultConstructorObjectCreator<T> : IObjectCreator<T> where T : new() {

		#region IObjectCreator<T>

		public T CreateInstance() {
			return new T();
		}

		#endregion
	}
}