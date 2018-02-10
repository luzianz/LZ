using System.Threading;
using System.Threading.Tasks;

namespace LZ.Async {

	/// <summary>
	/// Fetches a resource asynchronously.
	/// </summary>
	public interface IAsyncResource<T> {

		Task<T> GetAsync(CancellationToken cancellationToken);
	}
}
