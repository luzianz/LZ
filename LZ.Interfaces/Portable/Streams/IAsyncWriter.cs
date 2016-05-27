using System.Threading;
using System.Threading.Tasks;

namespace LZ.Streams {

	public interface IAsyncWriter {

		Task WriteAsync(byte[] bytes, uint count, CancellationToken cancellationToken);

		Task FlushAsync();
	}
}
