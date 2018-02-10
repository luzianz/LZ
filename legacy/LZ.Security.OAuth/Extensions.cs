using LZ.Format.Web;
using LZ.Security.Cryptography;
using LZ.Strings;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Convert;
using CommonServiceLocator;

namespace LZ.Security.OAuth {

	public static partial class Extensions {

		private static readonly IFactory<IKeyedHashAlgorithm, HashAlgorithmNames> keyedHashAlgorithmFactory;

		static Extensions() {
			keyedHashAlgorithmFactory = ServiceLocator.Current.GetInstance<IFactory<IKeyedHashAlgorithm, HashAlgorithmNames>>();
		}

		public static void AddOauthAuthorizationHeader(
			this ICollection<KeyValuePair<string, IEnumerable<string>>> headers,
			string headerValue) {
			headers.AddHeader("Authorization", $"OAuth {headerValue}");
		}

		public static void AddHeader(
			this ICollection<KeyValuePair<string, IEnumerable<string>>> headers,
			string headerKey,
			params string[] headerValues) {

			headers.Add(new KeyValuePair<string, IEnumerable<string>>(headerKey, headerValues));
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

			AddOAuthParameters(parameters, consumerKey, callbackUrl, token, verifier);

			string urlEncodedParams = parameters.ToEncodedQueryString(sort: true);

			var signatureBaseString = parameters.GetSignatureBaseString(httpMethod, requestUrl);

			parameters.Sign(signatureBaseString, consumerSecret, tokenSecret);

			return parameters.JoinToString(", ", p => $"{p.Key}=\"{p.Value.PercentEncode()}\"");
		}

		private static void Sign(
			this Dictionary<string, string> parameters,
			string signatureBaseString,
			string consumerSecret,
			string tokenSecret = null) {

			string signingKey = Format.GetSigningKey(consumerSecret, tokenSecret ?? string.Empty);
			string signature = GenerateSignature(signingKey, signatureBaseString.ToString());
			parameters.Add(ParameterNames.Signature, signature);
		}

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

		private static string GenerateSignature(string signingKey, string signatureBaseString) {
			using (var keyedHashAlgorithm = keyedHashAlgorithmFactory.CreateInstance(HashAlgorithmNames.Sha1)) {
				var signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);
				var parameterBytes = Encoding.UTF8.GetBytes(signatureBaseString);
				keyedHashAlgorithm.Key = signingKeyBytes;
				var signatureBytes = keyedHashAlgorithm.ComputeHash(parameterBytes);

				return ToBase64String(signatureBytes);
			}
		}
	}
}
