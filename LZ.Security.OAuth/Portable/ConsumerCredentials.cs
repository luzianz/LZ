namespace LZ.Security.OAuth
{
	public struct ConsumerCredentials : ICredential
	{
		private string _Key;
		private string _Secret;

		public ConsumerCredentials(string key, string secret)
		{
			_Key = key;
			_Secret = secret;
		}

		public string Key { get { return _Key; } }
		public string Secret { get { return _Secret; } }
	}
}
