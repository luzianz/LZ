using LZ.Commanding;
using LZ.Interactivity;
using Microsoft.Xaml.Interactivity;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace LZ.Interactions {

	/// <summary>
	/// Provides commands to navigate.
	/// </summary>
	[TypeConstraint(typeof(Page))]
	public class NavigatorBehavior : Behavior<Page> {

		#region Dependency Properties

		#region GoBack

		public static readonly DependencyProperty GoBackProperty = DependencyProperty.Register(
			nameof(GoBack), 
			typeof(ICommand),
			typeof(NavigatorBehavior), 
			new PropertyMetadata(null));
		public ICommand GoBack {
			get { return (ICommand)GetValue(GoBackProperty); }
			private set { SetValue(GoBackProperty, value); }
		}

		#endregion

		#region GoForward

		public static readonly DependencyProperty GoForwardProperty = DependencyProperty.Register("GoForward", typeof(ICommand), typeof(NavigatorBehavior), new PropertyMetadata(null));
		public ICommand GoForward {
			get { return (ICommand)GetValue(GoForwardProperty); }
			private set { SetValue(GoForwardProperty, value); }
		}

		#endregion

		#region CanGoForward

		public static readonly DependencyProperty CanGoForwardProperty = DependencyProperty.Register("CanGoForward", typeof(bool), typeof(NavigatorBehavior), new PropertyMetadata(false));
		public bool CanGoForward {
			get { return (bool)GetValue(CanGoForwardProperty); }
			private set { SetValue(CanGoForwardProperty, value); }
		}

		#endregion

		#region CanGoBack

		public static readonly DependencyProperty CanGoBackProperty = DependencyProperty.Register("CanGoBack", typeof(bool), typeof(NavigatorBehavior), new PropertyMetadata(false));
		public bool CanGoBack {
			get { return (bool)GetValue(CanGoBackProperty); }
			private set { SetValue(CanGoBackProperty, value); }
		}

		#endregion

		private static readonly DependencyProperty FrameProperty = DependencyProperty.Register(
			"Frame",
			typeof(Frame), 
			typeof(NavigatorBehavior), 
			new PropertyMetadata(null, FramePropertyChanged));

		#endregion

		#region Dependency Property Change Handlers

		private static void FramePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var behavior = d as NavigatorBehavior;
			if (behavior == null)
				return;

			behavior.OnFrameChanged(e);
		}

		#endregion

		#region Event Handlers

		protected virtual void OnFrameChanged(DependencyPropertyChangedEventArgs e) {
			if (AssociatedObject.Frame != null) {
				AssociatedObject.Frame.Navigated += (s, e2) => {
					CanGoForward = AssociatedObject.Frame.CanGoForward;
					CanGoBack = AssociatedObject.Frame.CanGoBack;
				};
			}
		}

		#endregion

		#region Behavior<Page>

		protected override void OnAttached() {
			base.OnAttached();

			GoBack = new DelegateCommand(
				_ => AssociatedObject.Frame.GoBack(),
				_ => AssociatedObject.Frame.CanGoBack);
			GoForward = new DelegateCommand(
				_ => AssociatedObject.Frame.GoForward(),
				_ => AssociatedObject.Frame.CanGoForward);

			SetBinding(FrameProperty, new Binding {
				Source = AssociatedObject,
				Path = new PropertyPath("Frame"),
				Mode = BindingMode.OneWay
			});
		}

		protected override void OnDetaching() {
			base.OnDetaching();
			GoBack = null;
			GoForward = null;

			ClearValue(FrameProperty);
		}

		#endregion
	}
}
