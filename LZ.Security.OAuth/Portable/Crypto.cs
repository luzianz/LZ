using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace LZ.Security.OAuth {

	internal static class Crypto {

		public static string GenerateSignature(string signingKey, string signatureBaseString) {
			IBuffer signingKeyBuffer = CryptographicBuffer.ConvertStringToBinary(signingKey, BinaryStringEncoding.Utf8);
			IBuffer parameterBuffer = CryptographicBuffer.ConvertStringToBinary(signatureBaseString, BinaryStringEncoding.Utf8);
			MacAlgorithmProvider hmacsha1Algorithm = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
			CryptographicHash hashAlgorithm = hmacsha1Algorithm.CreateHash(signingKeyBuffer);
			hashAlgorithm.Append(parameterBuffer);
			IBuffer signatureBuffer = hashAlgorithm.GetValueAndReset();

			return CryptographicBuffer.EncodeToBase64String(signatureBuffer);
		}
	}
}
