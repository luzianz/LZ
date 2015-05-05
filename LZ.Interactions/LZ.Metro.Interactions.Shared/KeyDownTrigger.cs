using Microsoft.Xaml.Interactivity;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace LZ.Interactions
{
	/// <summary>
	/// A Trigger which activates on a keyboard key press.
	/// </summary>
	[TypeConstraint(typeof(FrameworkElement))]
	[ContentProperty(Name = "Actions")]
	public class KeyDownTrigger : LZ.Interactivity.Trigger<FrameworkElement>
	{
		#region Dependency Properties

		#region TriggerKey
		public static readonly DependencyProperty TriggerKeyProperty = DependencyProperty.Register("TriggerKey", typeof(object), typeof(KeyDownTrigger), new PropertyMetadata(VirtualKey.None));
		/// <summary>The keyboard key which activates this trigger</summary>
		public object TriggerKey
		{
			get { return GetValue(TriggerKeyProperty); }
			set { SetValue(TriggerKeyProperty, value); }
		}
		#endregion

		#endregion

		#region Trigger<FrameworkElement>

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.KeyDown += AssociatedObject_KeyDown;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
		}

		#endregion

		#region Event Handlers

		void AssociatedObject_KeyDown(object sender, KeyRoutedEventArgs args)
		{
			// Resolves weird bug when pressing the enter key, an extra event is fired
			if (args.KeyStatus.ScanCode == 0) return;

			bool canParse = false;
			VirtualKey key;

			if (TriggerKey is VirtualKey)
			{
				key = (VirtualKey)TriggerKey;
				canParse = true;
			}
			else if (Enum.TryParse<VirtualKey>(TriggerKey.ToString(), out key))
			{
				canParse = true;
			}

			if (canParse)
			{
				if (key == args.Key)
				{
					base.ExecuteActions(args);
				}
#if DEBUG
				else if (key == VirtualKey.None)
				{
					/// Calling attention to an occurance when <see cref="LZ.Metro.Interactions.KeyFilterAction.TriggerKey"/>
					/// is not set in XAML, which should not occur.
					System.Diagnostics.Debug.WriteLine("TriggerKey is not set in XAML");
					System.Diagnostics.Debugger.Break();
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("{0} was pressed", args.Key);
				}
#endif
			}
#if DEBUG
			else
			{
				/// Calling attention to an occurance when <see cref="LZ.Interactions.KeyFilterAction.TriggerKey"/>
				/// is invalid, which should not occur.
				System.Diagnostics.Debug.WriteLine("TriggerKey is invalid");
				System.Diagnostics.Debugger.Break();
			}
#endif
		}

		#endregion
	}
}
