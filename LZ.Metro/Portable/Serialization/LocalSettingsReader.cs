using LZ;
using Windows.Storage;

namespace LZ.Metro.Serialization
{
	public class LocalSettingsReader : IKeyedReader<string, object>
	{
		public bool TryGetValue(string key, out object value)
		{
			return ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out value);
		}
	}
}
