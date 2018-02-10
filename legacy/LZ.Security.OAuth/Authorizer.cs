using LZ.Net;
using System;
using System.Threading.Tasks;

namespace LZ.Security.OAuth {

	public class Authorizer {

		#region Fields

		private readonly ICredential consumerCredentials;
		private readonly IWebAuthenticationBroker authenticationBroker;
		private readonly IObjectCreator<IHttpClient> httpClientCreator;

		#endregion

		#region Constructor

		public Authorizer(ICredential consumerCredentials, IWebAuthenticationBroker authenticationBroker, IObjectCreator<IHttpClient> httpClientCreator) {
			this.consumerCredentials = consumerCredentials;
			this.authenticationBroker = authenticationBroker;
			this.httpClientCreator = httpClientCreator;
		}

		#endregion

		public async Task<ICredential> AuthorizeAsync(string requestTokenUrl, string authorizeUrl, string accessTokenUrl) {
			using (var httpClient = httpClientCreator.CreateInstance()) {
				Uri callback = authenticationBroker.GetCurrentApplicationCallbackUri();

				var requestToken = await httpClient.ObtainRequestTokenAsync(
					requestUri: new Uri(requestTokenUrl),
					callbackUri: callback,
					httpMethod: HttpMethod.Post,
					consumerCredentials: consumerCredentials);

				string fullAuthorizeUrlStr = $"{authorizeUrl}?oauth_token={requestToken.Key}";

				var authorizationToken = await authenticationBroker.AuthenticateAsync(
					new Uri(fullAuthorizeUrlStr),
					callback);

				var accessToken = await httpClient.ObtainAccessTokenAsync(
					requestUri: new Uri(accessTokenUrl),
					httpMethod: HttpMethod.Post,
					consumerCredentials: consumerCredentials,
					authorizationToken: authorizationToken,
					requestToken: requestToken);

				return accessToken;
			}
		}
	}
}
