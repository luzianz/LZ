using System;
using System.Threading.Tasks;
using WA = Windows.Security.Authentication.Web;

namespace LZ.Security.OAuth
{
	internal static class WebAuthenticationBroker
	{
		public static async Task<AuthorizationToken> AuthenticateAsync(Uri requestUri)
		{
			var result = await WA.WebAuthenticationBroker.AuthenticateAsync(WA.WebAuthenticationOptions.None, requestUri);

			AuthorizationToken authorizationToken;
			Parser.ParseQuery(result.ResponseData, out authorizationToken);

			return authorizationToken;
		}

		public static async Task<AuthorizationToken> AuthenticateAsync(Uri requestUri, Uri callbackUri)
		{
			var result = await WA.WebAuthenticationBroker.AuthenticateAsync(WA.WebAuthenticationOptions.None, requestUri, callbackUri);

			AuthorizationToken authorizationToken;
			Parser.ParseQuery(result.ResponseData, out authorizationToken);

			return authorizationToken;
		}

		public static Uri GetCurrentApplicationCallbackUri()
		{
			return WA.WebAuthenticationBroker.GetCurrentApplicationCallbackUri();
		}
	}
}
