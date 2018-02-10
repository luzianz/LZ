using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LZ.Net {

	public interface IHttpClient : IDisposable {

		Task<IHttpResponse> SendAsync(Uri requestUri, HttpMethod httpMethod, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers);
	}
}
