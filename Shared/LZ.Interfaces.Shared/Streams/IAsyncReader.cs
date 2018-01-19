using System.Threading;
using System.Threading.Tasks;

namespace LZ.Streams {

	public interface IAsyncReader {

		Task<uint> ReadAsync(byte[] bytes, uint count, CancellationToken cancellationToken);
	}
}
