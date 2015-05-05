using Windows.UI.Xaml;

namespace LZ.Interactions
{
	public static class Extensions
	{
		public static bool IsFullscreen(this FrameworkElement element)
		{
			return element.ActualHeight == Window.Current.Bounds.Height &&
				element.ActualWidth == Window.Current.Bounds.Width;
		}
	}
}
