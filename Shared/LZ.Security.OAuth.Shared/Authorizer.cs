using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace LZ.Security.OAuth {

	public class Authorizer {

		#region Fields

		private readonly ICredential consumerCredentials;
		private readonly IWebAuthenticationBroker authenticationBroker;

		#endregion

		#region Constructor

		public Authorizer(ICredential consumerCredentials, IWebAuthenticationBroker authenticationBroker) {
			this.consumerCredentials = consumerCredentials;
			this.authenticationBroker = authenticationBroker;
		}

		#endregion

		public async Task<ICredential> AuthorizeAsync(string requestTokenUrl, string authorizeUrl, string accessTokenUrl) {
			var client = new HttpClient();
			Uri callback = authenticationBroker.GetCurrentApplicationCallbackUri();

			var requestToken = await client.ObtainRequestTokenAsync(
				requestUri: new Uri(requestTokenUrl),
				callbackUri: callback,
				httpMethod: HttpMethod.Post,
				consumerCredentials: consumerCredentials);

			string fullAuthorizeUrlStr = $"{authorizeUrl}?oauth_token={requestToken.Key}";

			var authorizationToken = await authenticationBroker.AuthenticateAsync(
				new Uri(fullAuthorizeUrlStr),
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
