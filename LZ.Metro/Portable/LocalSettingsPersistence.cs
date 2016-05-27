using LZ.Metro.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using System;

namespace LZ.Metro {

	public class LocalSettingsPersistence : IPersistence {

		public async Task LoadAsync() {
			await Task.Yield();
		}

		public bool Remove(string key) {
			return ApplicationData.Current.LocalSettings.Values.Remove(key);
		}

		public async Task SaveAsync() {
			await Task.Yield();
		}

		public void SetValue(string key, object value) {
			ApplicationData.Current.LocalSettings.Values[key] = value;
		}

		public bool TryGetValue(string key, out object value) {
			return ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out value);
		}
	}
}
