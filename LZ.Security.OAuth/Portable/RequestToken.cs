namespace LZ.Security.OAuth
{
    internal struct RequestToken : ICredential
	{
		#region ICredential
		public string Key { get; set; }
		public string Secret { get; set; }
		#endregion

		public bool IsCallbackConfirmed { get; set; }
	}
}
