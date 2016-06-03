using System;

namespace LZ.Security.OAuth.Uris {

	public class TumblrEndpoints : IOAuthEndpoints {

		#region IOAuthEndpoints

		public Uri AuthorizeUri { get; } = new Uri("https://www.tumblr.com/oauth/authorize");

		public Uri RequestTokenUri { get; } = new Uri("http://www.tumblr.com/oauth/request_token");

		public Uri AccessTokenUri { get; } = new Uri("http://www.tumblr.com/oauth/access_token");

		#endregion
	}
}
