using System;
using System.Threading.Tasks;
using WA = Windows.Security.Authentication.Web;

namespace LZ.Security.OAuth {

	internal static class WebAuthenticationBroker {

		public static async Task<AuthorizationToken> AuthenticateAsync(Uri requestUri) {
			var webResult = await WA.WebAuthenticationBroker.AuthenticateAsync(WA.WebAuthenticationOptions.None, requestUri);

			return Parse(webResult);
		}

		public static async Task<AuthorizationToken> AuthenticateAsync(Uri requestUri, Uri callbackUri) {
			var webResult = await WA.WebAuthenticationBroker.AuthenticateAsync(WA.WebAuthenticationOptions.None, requestUri, callbackUri);

			return Parse(webResult);
		}

		public static Uri GetCurrentApplicationCallbackUri() {
			return WA.WebAuthenticationBroker.GetCurrentApplicationCallbackUri();
		}

		private static AuthorizationToken Parse(WA.WebAuthenticationResult webResult) {
			AuthorizationResult result;
			if (!Parser.TryParseQueryString(webResult.ResponseData, out result)) {
				System.Diagnostics.Debugger.Break();
				throw new Exception();
			}

			return new AuthorizationToken {
				Key = result.Key,
				Secret = result.Secret,
				Verifier = result.Verifier
			};
		}
	}
}
