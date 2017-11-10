using System;
using System.Threading.Tasks;
using FortniteApi;

namespace Fortnite
{
    internal class Program
    {
        public static void Main(string[] args) => Run().GetAwaiter().GetResult();

        private static async Task Run()
        {
            await DoStuff();
            await DoStuff();
            await DoStuff();
            await DoStuff();

            Console.WriteLine("Finished.");
            Console.ReadKey();
        }

        private static async Task DoStuff()
        {
            using (var client = new FortniteClient())
            {
                Console.WriteLine("Authenticating..");

                await client.AuthenticateAsync(
                    Environment.GetEnvironmentVariable("EG_EMAIL"),
                    Environment.GetEnvironmentVariable("EG_PASSWORD")
                );

                Console.WriteLine("Exchanging tokens..");
                await client.ExchangeTokenAsync();

                Console.WriteLine("Looking up player ids..");
                var players = new[]
                {
                    "AeonLuciid"
                };

                foreach (var player in players)
                {
                    await client.GetPlayerAsync(player);

                    Console.WriteLine($"Looked up {player}");
                }
            }
        }
    }
}