using System;
using System.Collections.Generic;
using System.Text;
using Windows.Web.Http;
using LZ.Collections;
using LZ.Format.Web;
using LZ.Strings;

namespace LZ.Security.OAuth {

	public static partial class Extensions {

		private static partial class OAuth {

			public static string GenerateOAuthHeader(HttpMethod httpMethod, Uri requestUrl, string consumerKey, string consumerSecret, Uri callbackUrl = null, string token = null, string tokenSecret = null, string verifier = null) {
				var parameters = new List<KeyValuePair<string, string>>();
				parameters.Add(new KeyValuePair<string, string>(ParameterNames.ConsumerKey, consumerKey));
				parameters.Add(new KeyValuePair<string, string>(ParameterNames.Version, ParameterValues.OAuthVersion1));
				parameters.Add(new KeyValuePair<string, string>(ParameterNames.SignatureMethod, ParameterValues.HMAC_SHA1));
				parameters.Add(new KeyValuePair<string, string>(ParameterNames.Nonce, ParameterValues.GenerateNonce()));
				parameters.Add(new KeyValuePair<string, string>(ParameterNames.Timestamp, ParameterValues.GetTimestamp()));

				#region Optional
				if (callbackUrl != null) {
					parameters.Add(new KeyValuePair<string, string>(ParameterNames.Callback, callbackUrl.ToString()));
				}
				if (token != null) {
					parameters.Add(new KeyValuePair<string, string>(ParameterNames.Token, token));
				}
				if (verifier != null) {
					parameters.Add(new KeyValuePair<string, string>(ParameterNames.Verifier, verifier));
				}
				#endregion

				string urlEncodedParams = parameters.ToEncodedQueryString(sort: true);

				var signatureBaseString = new StringBuilder();   // note that this is a StringBuilder, not a string. You need to call .ToString() for the string.
				signatureBaseString.Append(httpMethod.Method);   // you would never need to percent-encoded this since it is only letters
				signatureBaseString.Append('&');
				signatureBaseString.Append(requestUrl.ToNormalizedString().PercentEncode());
				signatureBaseString.Append('&');
				signatureBaseString.Append(urlEncodedParams.PercentEncode());

				#region Sign
				string signingKey = Format.GetSigningKey(consumerSecret, tokenSecret ?? string.Empty);
				string signature = Crypto.GenerateSignature(signingKey, signatureBaseString.ToString());
				parameters.Add(new KeyValuePair<string, string>(ParameterNames.Signature, signature));
				#endregion

				return parameters.JoinToString(", ", p => string.Format("{0}=\"{1}\"", p.Key, p.Value.PercentEncode()));
			}
		}
	}
}
