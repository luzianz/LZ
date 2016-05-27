using System;
using System.Threading;
using System.Threading.Tasks;
using LZ.Streams;

namespace LZ.Net {

	public interface IConnection : IAsyncReader, IAsyncWriter, IDisposable {

		Task ConnectAsync(string hostName, int port, CancellationToken cancellationToken);
	}
}
