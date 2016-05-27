using LZ.Collections;

namespace LZ.ComponentModel {

	/// <summary>
	/// Represents an object that can serialize itself into a key/value store.
	/// </summary>
	public interface ISerializable {

		void Serialize(IKeyedWriter<string, object> writer);

		void Deserialize(IKeyedReader<string, object> reader);
	}
}
