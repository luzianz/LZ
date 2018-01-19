using LZ.Strings;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LZ.Streams {

	#region Delegates

	public delegate Task<uint> ReadAsyncDelegate(byte[] bytes, uint count, CancellationToken cancellationToken);
	public delegate Task WriteAsyncDelegate(byte[] bytes, uint count, CancellationToken cancellationToken);
	public delegate Task FlushAsyncDelegate();

	#endregion

	public static partial class StreamExtensions {
		
		public static async Task<uint> ReadAsync(this Stream stream, byte[] bytes, uint count, CancellationToken cancellationToken) {
			return (uint)await stream.ReadAsync(bytes, 0, (int)count);
		}
		
		public static async Task ObserveLinesAsync(this IAsyncReader reader, IObserver<string> observer, CancellationToken token, uint bufferLen) {
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			if (observer == null) throw new ArgumentNullException(nameof(observer));

			var sb = new StringBuilder((int)bufferLen);
			byte[] buffer = new byte[bufferLen];
			
			while (!token.IsCancellationRequested) {
				try {
					// if InputStreamOptions is ReadAhead, the async operation wont complete
					//  until the amount you requested (count) is received.
					uint received = await reader.ReadAsync(buffer, (uint)buffer.Length, token);

					if (received > 0) {
						var str = Encoding.UTF8.GetString(buffer, 0, (int)received);

						// check for line breaks
						str.Split(line => { // on line
							sb.Append(line);
							observer.OnNext(sb.ToString());
							sb.Clear();
						}, partial => { // rest of string not ending with a line break
							sb.Append(partial);
						}, '\r', '\n');
					} else { // EOF
						observer.OnCompleted();
						break;
					}
				} catch (TaskCanceledException) { // cancelled
					observer.OnCompleted();
					break;
				} catch (Exception ex) { // unhandled exception
					observer.OnError(ex);
				}
			}
		}
	}
}
