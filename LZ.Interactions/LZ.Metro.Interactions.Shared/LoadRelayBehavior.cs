using LZ.ComponentModel;
using LZ.Interactivity;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace LZ.Interactions {

	/// <summary>
	/// Invokes the bound ILoadable (Target property) when the attached FrameworkElement is loaded.
	/// </summary>
	[TypeConstraint(typeof(FrameworkElement))]
	public class LoadRelayBehavior : Behavior<FrameworkElement> {

		#region Dependency Properties

		#region Target

		public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
			nameof(Target),
			typeof(object),
			typeof(LoadRelayBehavior), 
			new PropertyMetadata(null));
		/// <summary>
		/// An object that implements ILoadable.
		/// If this property is not used, the DataContext will be used.
		/// </summary>
		public object Target {
			get { return GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}

		#endregion

		#region IsLoading

		public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
			nameof(IsLoading), 
			typeof(bool), 
			typeof(LoadRelayBehavior), 
			new PropertyMetadata(false));
		public bool IsLoading {
			get { return (bool)GetValue(IsLoadingProperty); }
			private set { SetValue(IsLoadingProperty, value); }
		}

		#endregion

		#endregion

		#region Behavior<FrameworkElement>

		protected override void OnAttached() {
			base.OnAttached();
			AssociatedObject.Loaded += AssociatedObject_Loaded;
		}

		protected override void OnDetaching() {
			base.OnDetaching();
			AssociatedObject.Loaded -= AssociatedObject_Loaded;
		}

		#endregion

		#region Event Handlers

		async void AssociatedObject_Loaded(object sender, RoutedEventArgs e) {
			// Use the Target if provided, else use the DataContext of the AssociatedObject
			var target = Target ?? AssociatedObject.DataContext;

			// Load the target given it supports ILoadable
			var loadable = target as ILoadable;
			if (loadable != null) {
				IsLoading = true;
				await loadable.LoadAsync();
				IsLoading = false;
			}
		}

		#endregion
	}
}
