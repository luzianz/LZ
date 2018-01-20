using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LZ.Security.OAuth {

	public static partial class HttpExtensions {

		internal async static Task<RequestToken> ObtainRequestTokenAsync(
			this HttpClient httpClient,
			Uri requestUri,
			Uri callbackUri,
			HttpMethod httpMethod,
			ICredential consumerCredentials) {
			var httpRequest = new HttpRequestMessage(httpMethod, requestUri);
			var parameters = new Dictionary<string, string>();

			string oauthHeader = parameters.GenerateOAuthHeader(
				httpMethod.Method,
				consumerCredentials.Key,
				consumerCredentials.Secret,
				requestUri,
				callbackUri);

			httpRequest.Headers.Authorization = new AuthenticationHeaderValue("OAuth", oauthHeader);

			HttpResponseMessage response = await httpClient.SendAsync(httpRequest);
			string responseString = await response.Content.ReadAsStringAsync();

			return ParseRequestToken(responseString);
		}

		internal async static Task<AccessToken> ObtainAccessTokenAsync(
			this HttpClient httpClient,
			Uri requestUri,
			HttpMethod httpMethod,
			ICredential consumerCredentials,
			AuthorizationToken authorizationToken,
			RequestToken requestToken) {
			var httpRequest = new HttpRequestMessage(httpMethod, requestUri);
			var parameters = new Dictionary<string, string>();

			string oauthHeader = parameters.GenerateOAuthHeader(
				httpMethod.Method,
				consumerCredentials.Key,
				consumerCredentials.Secret,
				requestUri,
				token: authorizationToken.Key,
				tokenSecret: requestToken.Secret,
				verifier: authorizationToken.Verifier);

			httpRequest.Headers.Authorization = new AuthenticationHeaderValue("OAuth", oauthHeader);

			HttpResponseMessage response = await httpClient.SendAsync(httpRequest);
			string responseString = await response.Content.ReadAsStringAsync();

			return ParseAccessToken(responseString);
		}

		public async static Task<HttpResponseMessage> AccessResourceAsync(
			this HttpClient httpClient,
			Uri resourceUri,
			HttpMethod httpMethod,
			ICredential consumerCredentials,
			ICredential accessToken,
			Dictionary<string, string> parameters) {
			var httpRequest = new HttpRequestMessage(httpMethod, resourceUri);
			string oauthHeader = parameters.GenerateOAuthHeader(
				httpMethod: httpMethod.Method,
				consumerKey: consumerCredentials.Key,
				consumerSecret: consumerCredentials.Secret,
				requestUrl: resourceUri,
				token: accessToken.Key,
				tokenSecret: accessToken.Secret);

			httpRequest.Headers.Authorization = new AuthenticationHeaderValue("OAuth", oauthHeader);

			return await httpClient.SendAsync(httpRequest);
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
