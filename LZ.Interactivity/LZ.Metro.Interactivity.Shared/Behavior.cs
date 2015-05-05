using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;

namespace LZ.Interactivity
{
	public class Behavior : FrameworkElement, IBehavior
	{
		#region IBehavior
		public DependencyObject AssociatedObject { get; private set; }
		public void Attach(DependencyObject associatedObject)
		{
			OnAttaching(associatedObject);
			OnAttached();
		}
		public void Detach()
		{
			OnDetaching();
		}
		#endregion

		protected virtual void OnAttaching(DependencyObject associatedObject)
		{
			this.AssociatedObject = associatedObject;
		}
		protected virtual void OnAttached() { }
		protected virtual void OnDetaching() { }
	}
}
