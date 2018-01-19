using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using LZ.Streams;
using LZ.Async;

namespace LZ.Windows.Net {

	public class SocketConnection : DisposableBase, IConnection {

		#region Fields

		private readonly StreamSocket socket;

		#endregion

		#region Constructor

		public SocketConnection() {
			socket = new StreamSocket();
		}

		#endregion

		#region IConnection

		public async Task ConnectAsync(string hostName, int port, CancellationToken cancellationToken) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(SocketConnection));
			
			var asyncAction = socket.ConnectAsync(new HostName(hostName), port.ToString());
			using (var reg = cancellationToken.RegisterAsyncAction(asyncAction)) {
				await asyncAction;
			}
		}

		#endregion

		#region DisposableBase

		protected override void OnDispose() {
			socket.Dispose();
		}

		#endregion

		#region IAsyncReader

		public async Task<uint> ReadAsync(byte[] bytes, uint count, CancellationToken cancellationToken) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(SocketConnection));

			return await socket.InputStream.ReadAsync(bytes, count, cancellationToken);
		}

		#endregion

		#region IAsyncWriter

		public async Task FlushAsync() {
			if (IsDisposed) throw new ObjectDisposedException(nameof(SocketConnection));

			await socket.OutputStream.FlushAsync();
		}

		public async Task WriteAsync(byte[] bytes, uint count, CancellationToken cancellationToken) {
			if (IsDisposed) throw new ObjectDisposedException(nameof(SocketConnection));

			await socket.OutputStream.WriteAsync(bytes, cancellationToken);
		}

		#endregion
	}
}
