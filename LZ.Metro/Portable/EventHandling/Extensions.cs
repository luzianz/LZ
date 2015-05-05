using Windows.Foundation;

namespace LZ.Metro.EventHandling
{
	public static class Extensions
	{
		public static void RaiseEvent<TSender, TArgs>(this TypedEventHandler<TSender, TArgs> eventHandlerDelegate, TSender sender, TArgs args)
		{
			if (eventHandlerDelegate == null) return;

			eventHandlerDelegate(sender, args);
		}
	}
}
