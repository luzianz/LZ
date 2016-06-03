using System;
using System.Collections.Generic;
using System.Text;
using LZ.Format.Web;
using LZ.Strings;

namespace LZ.Security.OAuth {
	public static partial class Extensions {

		private static void AddOAuthParameters(
			this Dictionary<string, string> parameters,
			string consumerKey,
			Uri callbackUrl = null,
			string token = null,
			string verifier = null) {

			parameters.Add(ParameterNames.ConsumerKey, consumerKey);
			parameters.Add(ParameterNames.Version, ParameterValues.OAuthVersion1);
			parameters.Add(ParameterNames.SignatureMethod, ParameterValues.HMAC_SHA1);
			parameters.Add(ParameterNames.Nonce, ParameterValues.GenerateNonce());
			parameters.Add(ParameterNames.Timestamp, ParameterValues.GetTimestamp());

			#region Optional

			if (callbackUrl != null) {
				parameters.Add(ParameterNames.Callback, callbackUrl.ToString());
			}
			if (token != null) {
				parameters.Add(ParameterNames.Token, token);
			}
			if (verifier != null) {
				parameters.Add(ParameterNames.Verifier, verifier);
			}

			#endregion
		}

		private static string GetSignatureBaseString(
			this IEnumerable<KeyValuePair<string, string>> parameters,
			string httpMethod,
			Uri requestUrl) {

			string urlEncodedParams = parameters.ToEncodedQueryString(sort: true);

			var signatureBaseString = new StringBuilder();   // note that this is a StringBuilder, not a string. You need to call .ToString() for the string.
			signatureBaseString.Append(httpMethod);   // you would never need to percent-encoded this since it is only letters
			signatureBaseString.Append('&');
			signatureBaseString.Append(requestUrl.ToNormalizedString().PercentEncode());
			signatureBaseString.Append('&');
			signatureBaseString.Append(urlEncodedParams.PercentEncode());

			return signatureBaseString.ToString();
		}

		private static void Sign(
			this Dictionary<string, string> parameters,
			string signatureBaseString,
			string consumerSecret,
			string tokenSecret = null) {

			string signingKey = Format.GetSigningKey(consumerSecret, tokenSecret ?? string.Empty);
			string signature = Crypto.GenerateSignature(signingKey, signatureBaseString.ToString());
			parameters.Add(ParameterNames.Signature, signature);
		}

		public static string GenerateOAuthHeader(
			this Dictionary<string, string> parameters,
			string httpMethod,
			string consumerKey,
			string consumerSecret,
			Uri requestUrl,
			Uri callbackUrl = null,
			string token = null,
			string tokenSecret = null,
			string verifier = null) {

			parameters.AddOAuthParameters(consumerKey, callbackUrl, token, verifier);

			string urlEncodedParams = parameters.ToEncodedQueryString(sort: true);

			var signatureBaseString = parameters.GetSignatureBaseString(httpMethod, requestUrl);

			parameters.Sign(signatureBaseString, consumerSecret, tokenSecret);

			return parameters.JoinToString(", ", p => $"{p.Key}=\"{p.Value.PercentEncode()}\"");
		}
	}
}
