using Microsoft.Xaml.Interactivity;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace LZ.Interactions
{
	/// <summary>A declarative way of populating the settings pane.</summary>
	[ContentProperty(Name = "SettingsPaneEntries")]
	public class SettingsContractBehavior : DependencyObject, IBehavior
	{
		#region Fields

		private SettingsPane settingsPane;

		#endregion

		#region Dependency Properties

		#region SettingsPaneEntries

		public static readonly DependencyProperty SettingsPaneEntriesProperty = DependencyProperty.Register("SettingsPaneEntries", typeof(DependencyObjectCollection), typeof(SettingsContractBehavior), new PropertyMetadata(new DependencyObjectCollection()));
		public DependencyObjectCollection SettingsPaneEntries
		{
			get { return (DependencyObjectCollection)GetValue(SettingsPaneEntriesProperty); }
		}

		#endregion

		#endregion

		#region IBehavior

		public DependencyObject AssociatedObject { get; private set; }

		public void Attach(DependencyObject associatedObject)
		{
			AssociatedObject = associatedObject;

			settingsPane = SettingsPane.GetForCurrentView();
			settingsPane.CommandsRequested += settingsPane_CommandsRequested;
		}

		public void Detach()
		{
			settingsPane.CommandsRequested -= settingsPane_CommandsRequested;
		}

		#endregion

		#region Event Handlers

		void settingsPane_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
		{
			if (SettingsPaneEntries == null) return;

			foreach (SettingsPaneEntry entry in SettingsPaneEntries)
			{
				// All properties of a settings pane entry are required
				if (entry == null || entry.Id == null || entry.Label == null || entry.ContentTemplate == null)
				{
					// ...so ignore any that are missing anything.
					continue;
				}

				var command = new SettingsCommand(
					entry.Id,
					entry.Label,
					_ =>
					{
						var settingsFlyout = new SettingsFlyout();
						settingsFlyout.Content = entry.ContentTemplate.LoadContent();
						settingsFlyout.Show();
					});

				args.Request.ApplicationCommands.Add(command);
			}
		}

		#endregion
	}
}
