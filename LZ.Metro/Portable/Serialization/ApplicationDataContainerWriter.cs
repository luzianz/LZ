using LZ.Collections;
using Windows.Storage;

namespace LZ.Metro.Serialization {

	public class ApplicationDataContainerWriter : IKeyedWriter<string, object> {

		#region Fields

		private readonly ApplicationDataContainer container;

		#endregion

		#region Constructor

		public ApplicationDataContainerWriter(ApplicationDataContainer container) {
			this.container = container;
		}

		#endregion

		#region IKeyedWriter<string, object>

		public void SetValue(string key, object value) {
			container.Values[key] = value;
		}

		#endregion
	}
}
