using System.Threading;
using System.Threading.Tasks;

namespace LZ.Async
{
	/// <summary>
	/// Fetches a resource asynchronously, and can be cancelled.
	/// </summary>
	public interface ICancellableAsyncResource<T> : IAsyncResource<T>
	{
		Task<T> GetAsync(CancellationToken cancellationToken);
	}
}
