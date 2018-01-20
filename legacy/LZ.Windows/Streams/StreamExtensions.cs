using LZ.Windows.Async;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace LZ.Windows.Streams {

	#region Delegates

	public delegate Task<uint> ReadAsyncDelegate(byte[] bytes, uint count, CancellationToken cancellationToken);
	public delegate Task WriteAsyncDelegate(byte[] bytes, uint count, CancellationToken cancellationToken);
	public delegate Task FlushAsyncDelegate();

	#endregion

	public static partial class StreamExtensions {
		
		public static async Task WriteAsync(this IOutputStream stream, byte[] bytes, CancellationToken cancellationToken) {
			var asyncOp = stream.WriteAsync(bytes.AsBuffer());

			using (var reg = cancellationToken.RegisterAsyncAction(asyncOp)) {
				await asyncOp;
			}
		}

		public static async Task<uint> ReadAsync(this IInputStream stream, byte[] bytes, uint count, CancellationToken cancellationToken) {
			var asyncOp = stream.ReadAsync(bytes.AsBuffer(), count, InputStreamOptions.Partial);

			using (var reg = cancellationToken.RegisterAsyncAction(asyncOp)) {
				var buffer = await asyncOp;
				return buffer.Length;
			}
		}
	}
}
