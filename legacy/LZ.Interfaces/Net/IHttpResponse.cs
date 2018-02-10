using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LZ.Net {

	public interface IHttpResponse : IDisposable {

		HttpStatusCode StatusCode { get; }
		IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; }
		Task<string> GetStringContentAsync();
	}
}
