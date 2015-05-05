using LZ;
using Windows.Storage;

namespace LZ.Metro.Serialization
{
	public class LocalSettingsWriter : IKeyedWriter<string, object>
	{
		public void SetValue(string key, object value)
		{
			ApplicationData.Current.LocalSettings.Values[key] = value;
		}
	}
}
