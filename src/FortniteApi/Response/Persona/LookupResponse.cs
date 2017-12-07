using Newtonsoft.Json;

namespace FortniteApi.Response.Persona
{
    public class LookupResponse
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("displayName", Required = Required.Always)]
        public string DisplayName { get; set; }
    }
}
