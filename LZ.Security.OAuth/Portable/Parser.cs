using System;
using System.Collections.Generic;

namespace LZ.Security.OAuth
{
    internal static class Parser
	{
		public static void ParseQuery<T>(string queryString, out T token) where T : new()
		{
			try
			{
				dynamic _token = new T();
				var parameters = new Dictionary<string, string>();
				ParseQuery(queryString, parameters);

				foreach (var kvp in parameters)
				{
					switch (kvp.Key)
					{
						case "oauth_token_secret":
							_token.TokenSecret = parameters[kvp.Key];
							break;

						case "oauth_token":
							_token.Token = parameters[kvp.Key];
							break;

						case "oauth_callback_confirmed":
							_token.IsCallbackConfirmed = bool.Parse(parameters[kvp.Key]);
							break;

						case "oauth_verifier":
							_token.Verifier = parameters[kvp.Key];
							break;
					}
				}
				token = _token;
			}
			catch (KeyNotFoundException)
			{
				throw new FormatException();
			}
		}

        private static void ParseQuery(string queryString, IDictionary<string, string> parameters)
        {
            string[] queryStringWithMaybeUriAndFragment = queryString.Split('#');
            // queryStringAndFragment[1] (fragment) is discarded (if exists)
            string[] queryStringWithMaybeUri = queryStringWithMaybeUriAndFragment[0].Split('?');
            string[] parameterCoupledStrings = queryStringWithMaybeUri[queryStringWithMaybeUri.Length == 2 ? 1 : 0].Split('&');

            foreach (string parameterCoupledString in parameterCoupledStrings)
            {
                string[] parameterPair = parameterCoupledString.Split('=');

                if (parameterPair.Length > 1)
                {
                    parameters.Add(parameterPair[0], parameterPair[1]);
                }
                else if (parameterPair.Length == 1)
                {
                    parameters.Add(parameterPair[0], null);
                }
            }
        }
    }
}
