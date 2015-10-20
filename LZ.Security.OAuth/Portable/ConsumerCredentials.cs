namespace LZ.Security.OAuth
{
	public struct ConsumerCredentials : ICredential
	{
		public ConsumerCredentials(string key, string secret)
		{
			this.Key = key;
			this.Secret = secret;
		}

		public string Key
		{
			get; set;
		}

		public string Secret
		{
			get; set;
		}
	}
}
