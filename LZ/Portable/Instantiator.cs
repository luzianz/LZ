namespace LZ
{
	/// <summary>
	/// A simple implementation of <see cref="LZ.ICreator&lt;T&gt;"/> that makes use of the "new()" type constraint to instantiate objects.
	/// </summary>
	public class Instantiator<T> : ICreator<T> where T : new()
	{
		public T CreateNew()
		{
			return new T();
		}
	}

	/// <summary>
	/// A simple implementation of <see cref="LZ.ICreator&lt;T&gt;"/> that makes use of the "new()" type constraint to instantiate objects.
	/// This is useful for when you need the type definition to exact.
	/// </summary>
	public class Instantiator<TBase, TDerrived> : ICreator<TBase> where TDerrived : TBase, new()
	{
		public TBase CreateNew()
		{
			return new TDerrived();
		}
	}
}
