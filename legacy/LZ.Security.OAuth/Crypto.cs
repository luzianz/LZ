#if (WIN81 || WPA81)
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
#elif (NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD2_0)
using System.Security.Cryptography;
using System.Text;
using static System.Convert;

namespace LZ.Security.OAuth {

	internal static class Crypto {
		
		public static string GenerateSignature(string signingKey, string signatureBaseString) {

			var signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);
			var parameterBytes = Encoding.UTF8.GetBytes(signatureBaseString);
			using (var hasher = new HMACSHA1(signingKeyBytes)) {
				var signatureBytes = hasher.ComputeHash(parameterBytes);
				return ToBase64String(signatureBytes);
			}
		}
	}
}
#endif