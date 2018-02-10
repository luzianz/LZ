using System;

namespace LZ.Security.Cryptography {

	public interface IKeyedHashAlgorithm : IDisposable {

		byte[] Key { get; set; }
		byte[] ComputeHash(byte[] data);
	}
}
