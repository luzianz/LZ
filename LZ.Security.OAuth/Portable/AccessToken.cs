namespace LZ.Security.OAuth
{
	internal struct AccessToken : ICredential
	{
		#region Properties

		public string Key { get; set; }
		public string Secret { get; set; }

		#endregion
	}
}
