using System;
using Windows.UI.Xaml;

namespace LZ.Interactions
{
	/// <summary>
	/// Represents an entry in the settings pane.
	/// Used by <see cref="LZ.Interactions.SettingsContractBehavior"/>
	/// </summary>
	public class SettingsPaneEntry : DependencyObject
	{
		private static Type settingsPaneEntryType = typeof(SettingsPaneEntry);

		#region Dependency Properties

		#region Id
		public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(object), settingsPaneEntryType, new PropertyMetadata(null));
		public object Id
		{
			get { return (object)GetValue(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		#endregion

		#region Label
		public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), settingsPaneEntryType, new PropertyMetadata(null));
		public string Label
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}
		#endregion

		#region ContentTemplate
		public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), settingsPaneEntryType, new PropertyMetadata(null));
		public DataTemplate ContentTemplate
		{
			get { return (DataTemplate)GetValue(ContentTemplateProperty); }
			set { SetValue(ContentTemplateProperty, value); }
		}
		#endregion

		#endregion
	}
}
