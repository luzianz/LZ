using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace LZ.Security.OAuth
{
	public static partial class Extensions
    {
		internal async static Task<RequestToken> ObtainRequestTokenAsync(
			this HttpClient httpClient, 
			Uri requestUri, 
			Uri callbackUri, 
			HttpMethod httpMethod, 
			ICredential consumerCredentials)
		{
			var httpRequest = new HttpRequestMessage(httpMethod, requestUri);
			string oauthHeader = OAuth.GenerateOAuthHeader(
				httpMethod: httpMethod,
				requestUrl: requestUri,
				consumerKey: consumerCredentials.Key,
				consumerSecret: consumerCredentials.Secret,
				callbackUrl: callbackUri);

            httpRequest.Headers.Authorization = new HttpCredentialsHeaderValue("OAuth", oauthHeader);

			HttpResponseMessage response = await httpClient.SendRequestAsync(httpRequest);
			string responseString = await response.Content.ReadAsStringAsync();

            RequestToken requestToken;
            Parser.ParseQuery(responseString, out requestToken);
            
			return requestToken;
		}

		internal async static Task<AccessToken> ObtainAccessTokenAsync(
			this HttpClient httpClient, 
			Uri requestUri, 
			HttpMethod httpMethod, 
			ICredential consumerCredentials, 
			AuthorizationToken authorizationToken, 
			RequestToken requestToken)
		{
			var httpRequest = new HttpRequestMessage(httpMethod, requestUri);
			string oauthHeader = OAuth.GenerateOAuthHeader(
				httpMethod: httpMethod,
				requestUrl: requestUri,
				consumerKey: consumerCredentials.Key,
				consumerSecret: consumerCredentials.Secret,
				token: authorizationToken.Token,
				tokenSecret: requestToken.Secret,
				verifier: authorizationToken.Verifier);

			httpRequest.Headers.Authorization = new HttpCredentialsHeaderValue("OAuth", oauthHeader);

			HttpResponseMessage response = await httpClient.SendRequestAsync(httpRequest);
			string responseString = await response.Content.ReadAsStringAsync();

			AccessToken accessToken;
			Parser.ParseQuery(responseString, out accessToken);

			return accessToken;
        }

		public async static Task<HttpResponseMessage> AccessResourceAsync(
			this HttpClient httpClient,
			Uri resourceUri,
			HttpMethod httpMethod,
			ICredential consumerCredentials,
			ICredential accessToken)
		{
			var httpRequest = new HttpRequestMessage(httpMethod, resourceUri);
			string oauthHeader = OAuth.GenerateOAuthHeader(
				httpMethod: httpMethod,
				requestUrl: resourceUri,
				consumerKey: consumerCredentials.Key,
				consumerSecret: consumerCredentials.Secret,
				token: accessToken.Key,
				tokenSecret: accessToken.Secret);

			httpRequest.Headers.Authorization = new HttpCredentialsHeaderValue("OAuth", oauthHeader);

			return await httpClient.SendRequestAsync(httpRequest);
		}
	}
}
