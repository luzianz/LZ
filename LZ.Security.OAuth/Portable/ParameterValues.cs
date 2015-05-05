﻿using System;
using System.Linq;

namespace LZ.Security.OAuth
{
	using Format.Conversion;

	internal static class ParameterValues
	{
		public const string OAuthVersion1 = "1.0";
		public const string HMAC_SHA1 = "HMAC-SHA1";

		public static string GenerateNonce()
		{
			//return Guid.NewGuid().ToByteArray().Cast<char>().ToHexadecimal(true);
			return Guid.NewGuid().ToByteArray().Select(b => (char)b).ToHexadecimal(true);
		}

		public static string GetTimestamp()
		{
			return DateTime.UtcNow.ToTimestamp().ToString();
		}
	}
}
