using System.Threading.Tasks;

namespace LZ.Streams {

	public interface IAsyncLineReader {

		Task<string> ReadLineAsync();
	}
}