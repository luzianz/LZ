namespace LZ.Security.OAuth
{
    internal struct RequestToken : ICredential
	{
		#region Properties

		public bool IsCallbackConfirmed { get; set; }

		#endregion

		#region ICredential

		public string Key { get; set; }
		public string Secret { get; set; }

		#endregion
	}
}
