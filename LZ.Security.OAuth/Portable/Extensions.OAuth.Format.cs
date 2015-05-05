namespace LZ.Security.OAuth
{
	using Format.Web;

	public static partial class Extensions
	{
		private static partial class OAuth
		{
			private static class Format
			{
				public static string GetSigningKey(string consumerSecret, string token)
				{
					return string.Format("{0}&{1}", consumerSecret.PercentEncode(), token.PercentEncode());
				}
			}
		}
	}
}
