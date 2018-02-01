using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using FortniteApi.Data;
using FortniteApi.Response;

namespace FortniteApi
{
    public class FortniteClient : IDisposable
    {
        private static readonly int RateLimitAmount = 1;

        private static readonly TimeSpan RateLimitDuration = TimeSpan.FromSeconds(2);

        private readonly FlurlClient _client;

        private readonly Semaphore _queue;

        private int _rateLimitRemaining;

        private DateTimeOffset _rateLimitResetRemaining;

        public FortniteClient(string apiKey)
        {
            _client = new FlurlClient
            {
                BaseUrl = "https://api.fortnitetracker.com/v1"
            };
            _client.WithHeader("TRN-Api-Key", apiKey);

            _queue = new Semaphore(1, 1);
            _rateLimitRemaining = RateLimitAmount;
            _rateLimitResetRemaining = DateTimeOffset.UtcNow + RateLimitDuration;
        }

        public async Task<ProfileResponse> FindPlayerAsync(Platform platform, string epicNickname)
        {
            try
            {
                _queue.WaitOne();

                if (_rateLimitRemaining == 0)
                {
                    var startTime = DateTimeOffset.UtcNow;
                    var difference = _rateLimitResetRemaining - startTime;

                    if (difference > TimeSpan.Zero)
                    {
                        await Task.Delay(difference);
                    }
                    
                    _rateLimitRemaining = RateLimitAmount;
                    _rateLimitResetRemaining = DateTimeOffset.UtcNow + RateLimitDuration;
                }

                var url = Url.Combine("profile", platform.ToString().ToLower(), epicNickname);

                return await url.WithClient(_client).GetJsonAsync<ProfileResponse>();
            }
            finally
            {
                _rateLimitRemaining--;
                _queue.Release();
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}