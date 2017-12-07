using System;
using Newtonsoft.Json;

namespace FortniteApi.Response.Account
{
    public class OAuthToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("expires_at")]
        public DateTimeOffset ExpiresAt { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("refresh_expires")]
        public int RefreshExpires { get; set; }

        [JsonProperty("refresh_expires_at")]
        public DateTimeOffset RefreshExpiresAt { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("internal_client")]
        public bool InternalClient { get; set; }

        [JsonProperty("client_service")]
        public string ClientService { get; set; }

        [JsonProperty("perms")]
        public OAuthPerm[] Perms { get; set; }

        [JsonProperty("app")]
        public string App { get; set; }

        [JsonProperty("in_app_id")]
        public string InAppId { get; set; }
    }

    public class OAuthPerm
    {
        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("action")]
        public int Action { get; set; }
    }
}
