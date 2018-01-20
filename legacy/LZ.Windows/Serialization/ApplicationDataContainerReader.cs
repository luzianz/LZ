using LZ.Collections;
using Windows.Storage;

namespace LZ.Windows.Serialization {

	public class ApplicationDataContainerReader : IKeyedReader<string, object> {

		#region Fields

		private readonly ApplicationDataContainer container;

		#endregion

		#region Constructor

		public ApplicationDataContainerReader(ApplicationDataContainer container) {
			this.container = container;
		}

		#endregion

		#region IKeyedReader<string, object>

		public bool TryGetValue(string key, out object value) {
			return container.Values.TryGetValue(key, out value);
		}

		#endregion
	}
}
