using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using FortniteApi.Cache;
using FortniteApi.Cache.Handlers;
using FortniteApi.Data;
using FortniteApi.Exceptions;
using FortniteApi.Response.Account;
using FortniteApi.Response.Fortnite;
using FortniteApi.Response.Persona;
using Newtonsoft.Json;

namespace FortniteApi
{
    public class FortniteClient : IDisposable
    {
        private readonly string _email;

        private readonly IFortniteCacheHandler _cacheHandler;

        private readonly FlurlClient _client;

        private FortniteCache _cache;

        private OAuthToken _token;

        private OAuthToken _tokenFortnite;
        
        public FortniteClient(string email, IFortniteCacheHandler cacheHandler = null)
        {
            _email = email;
            _cacheHandler = cacheHandler;
            _client = new FlurlClient();
            _client.Configure(settings =>
            {
                settings.AllowedHttpStatusRange = "400";
            });

            LoadCache();
        }

        #region Cache
        private void LoadCache()
        {
            _cache = _cacheHandler != null ? _cacheHandler.Load(_email) : new FortniteCache();
        }

        private void SaveCache()
        {
            _cacheHandler?.Save(_email, _cache);
        }
        #endregion

        #region Authentication
        public async Task<bool> AuthenticateAsync(string password)
        {
            if (_cache.FortniteToken != null && (
                    _cache.FortniteToken.ExpiresAt.UtcDateTime - TimeSpan.FromHours(1) > DateTime.UtcNow ||
                    _cache.FortniteToken.RefreshExpiresAt.UtcDateTime - TimeSpan.FromHours(1) > DateTime.UtcNow))
            {
                // Fortnite token or refresh token is still active.
                return true;
            }

            _cache.FortniteToken = null;

            if (_token != null)
            {
                throw new FortniteException("This FortniteClient instance is already authenticated with UE.");
            }

            var response = await Fortnite.UrlAuthenticate.WithClient(_client)
                .WithHeader("User-Agent", Fortnite.UserAgent)
                .WithHeader("Pragma", "no-cache")
                .WithBasicAuth("34a02cf8f4414e29b15921876da36f9a", "daafbccc737745039dffe53d94fc76cf") // Constants?
                .PostUrlEncodedAsync(new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"username", _email},
                    {"password", password},
                    {"includePerms", "true"}
                });

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Invalid authorization header:
                //   errors.com.epicgames.account.invalid_client_credentials
                // Invalid email / password:
                //   errors.com.epicgames.account.invalid_account_credentials

                var errorData = JsonConvert.DeserializeObject<OAuthTokenError>(responseContent);

                throw new FortniteAuthException("Unable to authenticate at UE.", errorData);
            }

            _token = JsonConvert.DeserializeObject<OAuthToken>(responseContent);

            return true;
        }

        public async Task<bool> ExchangeTokenAsync()
        {
            if (_cache.FortniteToken != null &&
                _cache.FortniteToken.ExpiresAt.UtcDateTime - TimeSpan.FromMinutes(65) > DateTime.UtcNow)
            {
                // Fortnite token is still active.
                _tokenFortnite = _cache.FortniteToken;
                return true;
            }

            if (_cache.FortniteToken != null &&
                _cache.FortniteToken.RefreshExpiresAt.UtcDateTime - TimeSpan.FromMinutes(65) > DateTime.UtcNow)
            {
                // Fortnite refresh token is still active.
                var responseThree = await Fortnite.UrlAuthenticate.WithClient(_client)
                    .WithHeader("User-Agent", Fortnite.UserAgentFortnite)
                    .WithHeader("Pragma", "no-cache")
                    .WithBasicAuth("ec684b8c687f479fadea3cb2ad83f5c6", "e1f31c211f28413186262d37a13fc84d") // Constants?
                    .PostUrlEncodedAsync(new Dictionary<string, string>
                    {
                        {"grant_type", "refresh_token"},
                        {"refresh_token", _cache.FortniteToken.RefreshToken},
                        {"includePerms", "true"}
                    });

                var responseContentThree = await responseThree.Content.ReadAsStringAsync();

                _tokenFortnite = _cache.FortniteToken = JsonConvert.DeserializeObject<OAuthToken>(responseContentThree);

                return true;
            }

            if (_token == null)
            {
                throw new FortniteException("This FortniteClient instance is not authenticated with UE.");
            }

            if (_tokenFortnite != null)
            {
                throw new FortniteException("This FortniteClient instance has already exchanged their token.");
            }

            var responseOne = await Fortnite.UrlAuthExchange.WithClient(_client)
                .WithHeader("User-Agent", Fortnite.UserAgent)
                .WithHeader("Pragma", "no-cache")
                .WithOAuthBearerToken(_token.AccessToken)
                .GetJsonAsync<OAuthExchangeToken>();

            var responseTwo = await Fortnite.UrlAuthenticate.WithClient(_client)
                .WithHeader("User-Agent", Fortnite.UserAgentFortnite)
                .WithHeader("Pragma", "no-cache")
                .WithBasicAuth("ec684b8c687f479fadea3cb2ad83f5c6", "e1f31c211f28413186262d37a13fc84d") // Constants?
                .PostUrlEncodedAsync(new Dictionary<string, string>
                {
                    {"grant_type", "exchange_code"},
                    {"exchange_code", responseOne.Code},
                    {"includePerms", "true"},
                    {"token_type", "eg1"}
                });

            var responseContent = await responseTwo.Content.ReadAsStringAsync();

            if (!responseTwo.IsSuccessStatusCode)
            {
                var errorData = JsonConvert.DeserializeObject<OAuthTokenError>(responseContent);

                throw new FortniteAuthException("Unable to exchange tokens at Fortnite.", errorData);
            }

            _tokenFortnite = _cache.FortniteToken = JsonConvert.DeserializeObject<OAuthToken>(responseContent);
            
            SaveCache();

            return true;
        }
        #endregion

        #region Fortnite
        public async Task<LookupResponse> LookupPlayerAsync(string displayName)
        {
            if (_tokenFortnite == null)
            {
                throw new FortniteException("This FortniteClient instance is not authenticated with Fortnite.");
            }

            if (_cache.LookupCache.ContainsKey(displayName.ToLower()))
            {
                return _cache.LookupCache[displayName.ToLower()];
            }

            var response = await Fortnite.UrlAccountLookup.WithClient(_client)
                .SetQueryParam("q", displayName)
                .WithHeader("User-Agent", Fortnite.UserAgentFortnite)
                .WithHeader("Pragma", "no-cache")
                .WithOAuthBearerToken(_tokenFortnite.AccessToken)
                .GetAsync();

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<LookupErrorResponse>(responseContent);

                throw new FortniteLookupException("Unable to lookup player, see 'Error' for more details.", error);
            }

            var responseData = JsonConvert.DeserializeObject<LookupResponse>(responseContent);

            _cache.LookupCache.Add(displayName.ToLower(), responseData);

            return responseData;
        }

        public async Task<StatsResponse[]> GetPlayerStatsAsync(string accountId)
        {
            if (_tokenFortnite == null)
            {
                throw new FortniteException("This FortniteClient instance is not authenticated with Fortnite.");
            }

            return await string.Format(Fortnite.UrlFortniteStats, accountId).WithClient(_client)
                .WithHeader("User-Agent", Fortnite.UserAgentFortnite)
                .WithOAuthBearerToken(_tokenFortnite.AccessToken)
                .GetJsonAsync<StatsResponse[]>();
        }

        public async Task<Player> GetPlayerAsync(string displayName)
        {
            var lookup = await LookupPlayerAsync(displayName);
            var stats = await GetPlayerStatsAsync(lookup.Id);

            return new Player(lookup, stats);
        }
        #endregion

        public void Dispose()
        {
            SaveCache();

            _client.Dispose();
        }
    }
}