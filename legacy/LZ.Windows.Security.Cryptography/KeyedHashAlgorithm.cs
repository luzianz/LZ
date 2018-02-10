using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using C = LZ.Security.Cryptography;

namespace LZ.Windows.Security.Cryptography {

	public class KeyedHashAlgorithm : C.IKeyedHashAlgorithm {
		private readonly MacAlgorithmProvider alg;

		public KeyedHashAlgorithm(C.HashAlgorithmNames algorithm) {
			switch (algorithm) {
				case C.HashAlgorithmNames.Sha1:
					alg = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
					break;
				case C.HashAlgorithmNames.Sha256:
					alg = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
					break;
				case C.HashAlgorithmNames.Sha384:
					alg = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha384);
					break;
				case C.HashAlgorithmNames.Sha512:
					alg = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha512);
					break;
				case C.HashAlgorithmNames.Md5:
					alg = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacMd5);
					break;
				default:
					throw new NotSupportedException();
			}
		}

		public byte[] Key { get; set; }

		public byte[] ComputeHash(byte[] data) {
			var dataBuffer = CryptographicBuffer.CreateFromByteArray(data);
			var keyBuffer = CryptographicBuffer.CreateFromByteArray(Key);

			var hash = alg.CreateHash(keyBuffer);
			hash.Append(dataBuffer);

			var resultBuffer = hash.GetValueAndReset();
			byte[] result = new byte[resultBuffer.Length];

			CryptographicBuffer.CopyToByteArray(resultBuffer, out result);

			return result;
		}

		public void Dispose() {
		}
	}
}
