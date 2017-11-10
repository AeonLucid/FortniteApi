using System.Collections.Generic;
using FortniteApi.Response.Account;
using FortniteApi.Response.Persona;

namespace FortniteApi
{
    internal static class FortniteCache
    {
        public static Dictionary<string, LookupResponse> LookupCache = new Dictionary<string, LookupResponse>();

        public static OAuthToken FortniteToken;
    }
}
