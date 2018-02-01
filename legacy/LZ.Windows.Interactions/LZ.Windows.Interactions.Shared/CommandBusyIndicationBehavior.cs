using LZ.Interactivity;
using Microsoft.Xaml.Interactivity;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LZ.Interactions
{
	/// <summary>
	/// Provides a ProgressRing indication on progress based on whether a command cannot execute. Given 
	/// that the command's "CanExecute" determines if the operation isn't in progress.
	/// </summary>
	[TypeConstraint(typeof(ProgressRing))]
    public class CommandBusyIndicationBehavior : Behavior<ProgressRing>
	{
		#region Dependency Properties

		#region Command

		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandBusyIndicationBehavior), new PropertyMetadata(null, OnCommandChanged));
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		#endregion

		#endregion

		#region Dependency Property Change Event Handlers

		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behavior = d as CommandBusyIndicationBehavior;

			// When the command's ability to execute (busy/not busy) has changed, this fact is propogated
			// to whether the ProgressRing is active (indicating progress)
			EventHandler command_CanExecuteChanged = (senderCommand, _) =>
			{
				var command = senderCommand as ICommand;
				behavior.AssociatedObject.IsActive = !command.CanExecute(null);
			};


			var oldCommand = e.OldValue as ICommand;
			if (oldCommand != null)
			{
				// Stop observing the old (discarded) value of the Command property
				oldCommand.CanExecuteChanged -= command_CanExecuteChanged;
			}

			var newCommand = e.NewValue as ICommand;
			if (newCommand != null)
			{
				// Observing the new (provided) value of the Command property
				newCommand.CanExecuteChanged += command_CanExecuteChanged;
			}
		}

		#endregion

		#region Behavior<ProgressRing>

		protected override void OnDetaching()
		{
			base.OnDetaching();
			Command = null;
		}

		#endregion
	}
}
