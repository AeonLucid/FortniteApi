using Newtonsoft.Json;

namespace FortniteApi.Response.Account
{
    internal class OAuthExchangeToken
    {
        [JsonProperty("expiresInSeconds", Required = Required.Always)]
        public int ExpiresInSeconds { get; set; }

        [JsonProperty("code", Required = Required.Always)]
        public string Code { get; set; }

        [JsonProperty("creatingClientId", Required = Required.Always)]
        public string CreatingClientId { get; set; }
    }
}
