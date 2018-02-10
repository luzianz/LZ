using System;
using C = System.Security.Cryptography;

namespace LZ.Security.Cryptography {

	public class KeyedHashAlgorithm : IDisposable {

		private readonly C.KeyedHashAlgorithm alg;

		public KeyedHashAlgorithm(HashAlgorithmNames algorithm) {
			switch (algorithm) {
				case HashAlgorithmNames.Sha1:
					alg = new C.HMACSHA1();
					break;
				case HashAlgorithmNames.Sha256:
					alg = new C.HMACSHA256();
					break;
				case HashAlgorithmNames.Sha384:
					alg = new C.HMACSHA384();
					break;
				case HashAlgorithmNames.Sha512:
					alg = new C.HMACSHA512();
					break;
				case HashAlgorithmNames.Md5:
					alg = new C.HMACMD5();
					break;
				default:
					throw new NotSupportedException();
			}
		}

		public byte[] Key {
			get {
				return alg.Key;
			}
			set {
				alg.Key = value;
			}
		}

		public byte[] ComputeHash(byte[] data) {
			return alg.ComputeHash(data);
		}

		public void Dispose() {
			alg.Dispose();
		}
	}
}