namespace LZ.Security.OAuth
{
	internal struct AccessToken : ICredential
	{
		public string Key { get; set; }
		public string Secret { get; set; }
	}
}
