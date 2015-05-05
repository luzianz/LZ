using LZ.Interactivity;
using Microsoft.Xaml.Interactivity;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace LZ.Interactions
{
	/// <summary>
	/// Provides a bindable property containing selected items.
	/// </summary>
	[TypeConstraint(typeof(Selector))]
    public class SelectionBindingBehavior : Behavior<Selector>
	{
		#region Dependency Properties

		#region SelectedItems

		public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IList), typeof(SelectionBindingBehavior), new PropertyMetadata(null));
		/// <summary>
		/// An IList containing the selected items.
		/// </summary>
		public IList SelectedItems
		{
			get { return (IList)GetValue(SelectedItemsProperty); }
			set { SetValue(SelectedItemsProperty, value); }
		}

		#endregion

		#endregion

		#region Behavior<T>

		protected override void OnAttached()
		{
			AssociatedObject.SelectionChanged += Selector_SelectionChanged;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.SelectionChanged -= Selector_SelectionChanged;
		}

		#endregion

		#region Event Handlers

		void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (SelectedItems == null)
			{
				return;
			}

			if (e.RemovedItems != null)
			{
				foreach (object item in e.RemovedItems)
				{
					SelectedItems.Remove(item);
				}
			}
			if (e.AddedItems != null)
			{
				foreach (object item in e.AddedItems)
				{
					SelectedItems.Add(item);
				}
			}
		}

		#endregion
	}
}
