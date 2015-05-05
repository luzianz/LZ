using System.Threading.Tasks;

namespace LZ
{
	/// <summary>
	/// Defines methods that read/write data from/to a persistent data store.
	/// </summary>
	public interface IPersistence : ILoadable, ISavable, IKeyedReader<string, object>, IKeyedWriter<string, object>
	{
	}
}
