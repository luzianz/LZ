using System;

namespace LZ.Security.OAuth.Uris {

	public class TwitterEndpoints : IOAuthEndpoints {

		#region IOAuthEndpoints

		public Uri AuthorizeUri { get; } = new Uri("https://api.twitter.com/oauth/authorize");

		public Uri RequestTokenUri { get; } = new Uri("https://api.twitter.com/oauth/request_token");

		public Uri AccessTokenUri { get; } = new Uri("https://api.twitter.com/oauth/access_token");

		#endregion
	}
}
