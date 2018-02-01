using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LZ.Interactivity {

	public static class Extensions {

		public static void BindTo(this FrameworkElement el, FrameworkElement source) {
			var binding = new Binding {
				Path = new PropertyPath("DataContext"),
				Source = source
			};
			el.SetBinding(FrameworkElement.DataContextProperty, binding);
		}

		public static void Unbind(this FrameworkElement el) {
			el.ClearValue(FrameworkElement.DataContextProperty);
		}
	}
}
