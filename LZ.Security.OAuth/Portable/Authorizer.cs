using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace LZ.Security.OAuth
{
	public class Authorizer
	{
		#region Fields

		private readonly ICredential consumerCredentials;

		#endregion

		#region Constructor

		public Authorizer(ICredential consumerCredentials)
		{
			this.consumerCredentials = consumerCredentials;
		}

		#endregion

		public async Task<ICredential> AuthorizeAsync(string requestTokenUrl, string authorizeUrl, string accessTokenUrl)
		{
			var client = new HttpClient();
			Uri callback = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();

			var requestToken = await client.ObtainRequestTokenAsync(
				requestUri: new Uri(requestTokenUrl),
				callbackUri: callback,
				httpMethod: HttpMethod.Post,
				consumerCredentials: consumerCredentials);

			string full_authorize_url_str = string.Format("{0}?oauth_token={1}", authorizeUrl, requestToken.Key);

			var authorizationToken = await WebAuthenticationBroker.AuthenticateAsync(
				new Uri(full_authorize_url_str),
				callback);

			var accessToken = await client.ObtainAccessTokenAsync(
				requestUri: new Uri(accessTokenUrl),
				httpMethod: HttpMethod.Post,
				consumerCredentials: consumerCredentials,
				authorizationToken: authorizationToken,
				requestToken: requestToken);

			return accessToken;
		}
	}
}
