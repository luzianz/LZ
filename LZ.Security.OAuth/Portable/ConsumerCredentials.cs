namespace LZ.Security.OAuth {

	public struct ConsumerCredentials : ICredential {

		public ConsumerCredentials(string key, string secret) {
			Key = key;
			Secret = secret;
		}

		public string Key { get; }
		public string Secret { get; }
	}
}
