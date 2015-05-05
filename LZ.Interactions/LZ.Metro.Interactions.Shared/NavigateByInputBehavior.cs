using LZ.Interactivity;
using Microsoft.Xaml.Interactivity;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LZ.Interactions
{
	/// <summary>
	/// Allows user to navigate the application with keyboard and mouse.
	/// No configuration required.
	/// </summary>
	[TypeConstraint(typeof(Page))]
	public class NavigateByInputBehavior : Behavior<Page>
	{
		#region Behavior<Page>

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.Loaded += Page_Loaded;
			AssociatedObject.Unloaded += Page_Unloaded;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.Loaded -= Page_Loaded;
			AssociatedObject.Unloaded -= Page_Unloaded;
		}

		#endregion

		#region Event Handlers

		void Page_Loaded(object sender, RoutedEventArgs e)
		{
			// Keyboard and mouse navigation only apply when occupying the entire window
			if (AssociatedObject.IsFullscreen())
			{
				// Listen to the window directly so focus isn't required
				Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
				Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
			}
		}

		void Page_Unloaded(object sender, RoutedEventArgs e)
		{
			Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;
			Window.Current.CoreWindow.PointerPressed -= CoreWindow_PointerPressed;
		}

		void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
		{
			var properties = e.CurrentPoint.Properties;

			// Ignore button chords with the left, right, and middle buttons
			if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
				properties.IsMiddleButtonPressed) return;

			// If back or foward are pressed (but not both) navigate appropriately
			bool backPressed = properties.IsXButton1Pressed;
			bool forwardPressed = properties.IsXButton2Pressed;
			if (backPressed ^ forwardPressed)
			{
				e.Handled = true;
				if (backPressed && AssociatedObject.Frame.CanGoBack) AssociatedObject.Frame.GoBack();
				if (forwardPressed && AssociatedObject.Frame.CanGoForward) AssociatedObject.Frame.GoForward();
			}
		}

		void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
		{
			var virtualKey = e.VirtualKey;

			// Only investigate further when Left, Right, or the dedicated Previous or Next keys
			// are pressed
			if ((e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown ||
				e.EventType == CoreAcceleratorKeyEventType.KeyDown) &&
				(virtualKey == VirtualKey.Left || virtualKey == VirtualKey.Right ||
				(int)virtualKey == 166 || (int)virtualKey == 167))
			{
				var coreWindow = Window.Current.CoreWindow;
				var downState = CoreVirtualKeyStates.Down;
				bool menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
				bool controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
				bool shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;
				bool noModifiers = !menuKey && !controlKey && !shiftKey;
				bool onlyAlt = menuKey && !controlKey && !shiftKey;

				if (((int)virtualKey == 166 && noModifiers) || (virtualKey == VirtualKey.Left && onlyAlt))
				{
					// When the previous key or Alt+Left are pressed navigate back
					e.Handled = true;
					if (AssociatedObject.Frame.CanGoBack) AssociatedObject.Frame.GoBack();
					//this.GoBackCommand.Execute(null);
				}
				else if (((int)virtualKey == 167 && noModifiers) ||
					(virtualKey == VirtualKey.Right && onlyAlt))
				{
					// When the next key or Alt+Right are pressed navigate forward
					e.Handled = true;
					if (AssociatedObject.Frame.CanGoForward) AssociatedObject.Frame.GoForward();
					//this.GoForwardCommand.Execute(null);
				}
			}
		}

		#endregion
	}
}
