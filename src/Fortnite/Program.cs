using System;
using System.IO;
using System.Threading.Tasks;
using FortniteApi;
using FortniteApi.Cache.Handlers;

namespace Fortnite
{
    internal class Program
    {
        public static async Task Main()
        {
            await DoStuff();

            Console.WriteLine("Finished.");
            Console.ReadKey();
        }

        private static async Task DoStuff()
        {
            var rootDir = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            var cacheHandler = new FileCacheHandler(Path.Combine(rootDir, "cache"));

            using (var client = new FortniteClient(Environment.GetEnvironmentVariable("EG_EMAIL"), cacheHandler))
            {
                Console.WriteLine("Authenticating..");
                await client.AuthenticateAsync(Environment.GetEnvironmentVariable("EG_PASSWORD"));

                Console.WriteLine("Exchanging tokens..");
                await client.ExchangeTokenAsync();

                Console.WriteLine("Looking up player ids..");
                var players = new[]
                {
                    "AeonLuciid",
                    "AeonLucidUtil"
                };

                foreach (var player in players)
                {
                    Console.WriteLine($"Looking up {player}");

                    await client.GetPlayerAsync(player);
                }
            }
        }
    }
}