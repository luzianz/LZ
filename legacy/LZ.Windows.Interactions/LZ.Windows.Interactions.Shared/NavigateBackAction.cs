using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LZ.Interactions {

	public class NavigateBackAction : DependencyObject, IAction {

		public object Execute(object sender, object parameter) {
			var page = sender as Page;
			if (page.Frame != null && page.Frame.CanGoBack) {
				page.Frame.GoBack();
			}
			return null;
		}
	}
}
