namespace LZ {

	public class DefaultConstructorObjectCreator<T> : IObjectCreator<T> where T : new() {

		#region IObjectCreator<T>

		public T CreateInstance() {
			return new T();
		}

		#endregion
	}

	public class DefaultConstructorObjectCreator<T, I> : IObjectCreator<I> where T : I, new() {

		#region IObjectCreator<I>

		public I CreateInstance() {
			return new T();
		}

		#endregion
	}
}