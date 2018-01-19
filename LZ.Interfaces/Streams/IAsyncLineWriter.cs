using System.Threading.Tasks;

namespace LZ.Streams {

	public interface IAsyncLineWriter {

		Task WriteLineAsync(string line, bool flush);
	}
}