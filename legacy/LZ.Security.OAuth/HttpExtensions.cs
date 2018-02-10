using LZ.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LZ.Security.OAuth {

	public static partial class HttpExtensions {

		internal async static Task<RequestToken> ObtainRequestTokenAsync(
			this IHttpClient httpClient,
			Uri requestUri,
			Uri callbackUri,
			HttpMethod httpMethod,
			ICredential consumerCredentials) {

			var parameters = new Dictionary<string, string>();
			string oauthHeaderValue = parameters.GenerateOAuthHeader(
				httpMethod.ToString(),
				consumerCredentials.Key,
				consumerCredentials.Secret,
				requestUri,
				callbackUri);

			var headers = new List<KeyValuePair<string, IEnumerable<string>>>();
			headers.AddOauthAuthorizationHeader(oauthHeaderValue);

			using (var response = await httpClient.SendAsync(requestUri, httpMethod, headers)) {
				string responseString = await response.GetStringContentAsync();
				return ParseRequestToken(responseString);
			}
		}

		internal async static Task<AccessToken> ObtainAccessTokenAsync(
			this IHttpClient httpClient,
			Uri requestUri,
			HttpMethod httpMethod,
			ICredential consumerCredentials,
			AuthorizationToken authorizationToken,
			RequestToken requestToken) {
			
			var parameters = new Dictionary<string, string>();
			string oauthHeaderValue = parameters.GenerateOAuthHeader(
				httpMethod.ToString(),
				consumerCredentials.Key,
				consumerCredentials.Secret,
				requestUri,
				token: authorizationToken.Key,
				tokenSecret: requestToken.Secret,
				verifier: authorizationToken.Verifier);

			var headers = new List<KeyValuePair<string, IEnumerable<string>>>();
			headers.AddOauthAuthorizationHeader(oauthHeaderValue);

			using (var response = await httpClient.SendAsync(requestUri, httpMethod, headers)) {
				string responseString = await response.GetStringContentAsync();
				return ParseAccessToken(responseString);
			}
		}

		public async static Task<IHttpResponse> AccessResourceAsync(
			this IHttpClient httpClient,
			Uri resourceUri,
			HttpMethod httpMethod,
			ICredential consumerCredentials,
			ICredential accessToken,
			Dictionary<string, string> parameters) {
			
			string oauthHeaderValue = parameters.GenerateOAuthHeader(
				httpMethod: httpMethod.ToString(),
				consumerKey: consumerCredentials.Key,
				consumerSecret: consumerCredentials.Secret,
				requestUrl: resourceUri,
				token: accessToken.Key,
				tokenSecret: accessToken.Secret);

			var headers = new List<KeyValuePair<string, IEnumerable<string>>>();
			headers.AddOauthAuthorizationHeader(oauthHeaderValue);

			return await httpClient.SendAsync(resourceUri, httpMethod, headers);
		}

		private static AccessToken ParseAccessToken(string responseString) {
			AuthorizationResult result;
			if (!Parser.TryParseQueryString(responseString, out result)) {
				System.Diagnostics.Debugger.Break();
				throw new Exception();
			}

			return new AccessToken {
				Key = result.Key,
				Secret = result.Secret
			};
		}

		private static RequestToken ParseRequestToken(string responseString) {
			AuthorizationResult result;
			if (!Parser.TryParseQueryString(responseString, out result)) {
				System.Diagnostics.Debugger.Break();
				throw new Exception();
			}

			return new RequestToken {
				Key = result.Key,
				Secret = result.Secret,
				IsCallbackConfirmed = result.IsCallbackConfirmed
			};
		}
	}
}
