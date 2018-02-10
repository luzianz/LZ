using System.Threading;
using System.Threading.Tasks;

namespace LZ.Net {

	public interface IListener {

		Task ListenAsync(CancellationToken cancellationToken);
	}
}
