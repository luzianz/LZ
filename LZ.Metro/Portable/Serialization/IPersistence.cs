using LZ.Collections;
using LZ.ComponentModel;

namespace LZ.Metro.Serialization {

	public interface IPersistence : ILoadable, ISavable, IKeyedRemovable<string>, IKeyedWriter<string, object>, IKeyedReader<string, object> {
	}
}
