﻿using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LZ.Net {
	public class TcpClientConnection : IConnection {

		private readonly TcpClient tcpClient;

		public TcpClientConnection() {
			tcpClient = new TcpClient();
		}

		public async Task ConnectAsync(string hostName, int port, CancellationToken cancellationToken) {
			await tcpClient.ConnectAsync(hostName, port);
		}

		public void Dispose() {
			tcpClient.Close();
		}

		public async Task FlushAsync() {
			await tcpClient.GetStream().FlushAsync();
		}

		public async Task<uint> ReadAsync(byte[] bytes, uint count, CancellationToken cancellationToken) {
			return (uint)await tcpClient.GetStream().ReadAsync(bytes, 0, (int)count, cancellationToken);
		}

		public async Task WriteAsync(byte[] bytes, uint count, CancellationToken cancellationToken) {
			await tcpClient.GetStream().WriteAsync(bytes, 0, (int)count, cancellationToken);
		}
	}
}