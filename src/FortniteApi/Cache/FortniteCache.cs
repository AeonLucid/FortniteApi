using System.Collections.Generic;
using FortniteApi.Response.Account;
using FortniteApi.Response.Persona;

namespace FortniteApi.Cache
{
    public class FortniteCache
    {
        public Dictionary<string, LookupResponse> LookupCache = new Dictionary<string, LookupResponse>();

        public OAuthToken FortniteToken;
    }
}
