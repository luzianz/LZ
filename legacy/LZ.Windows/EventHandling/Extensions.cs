using Windows.Foundation;

namespace LZ.Windows.EventHandling {

	public static class Extensions {

		public static void Invoke<TSender, TArgs>(this TypedEventHandler<TSender, TArgs> eventHandlerDelegate, TSender sender, TArgs args) {
			eventHandlerDelegate?.Invoke(sender, args);
		}
	}
}
