using System;

namespace LZ.Security.OAuth {

	public interface IOAuthEndpoints {

		Uri AuthorizeUri { get; }

		Uri RequestTokenUri { get; }

		Uri AccessTokenUri { get; }
	}
}
