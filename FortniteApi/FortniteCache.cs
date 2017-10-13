using System.Collections.Generic;
using FortniteApi.Response.Persona;

namespace FortniteApi
{
    public static class FortniteCache
    {
        public static Dictionary<string, LookupResponse> LookupCache = new Dictionary<string, LookupResponse>();
    }
}
