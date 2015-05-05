namespace LZ.Security.OAuth
{
    internal struct AuthorizationToken
	{
		#region Properties

		public string Token { get; set; }
        public string Verifier { get; set; }

		#endregion
	}
}
