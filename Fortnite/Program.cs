using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FortniteApi;
using Newtonsoft.Json;

namespace Fortnite
{
    internal class Program
    {
        public static void Main(string[] args) => Run().GetAwaiter().GetResult();

        private static async Task Run()
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
                    var playerResponse = await client.GetPlayerAsync(player);

                    Console.WriteLine(JsonConvert.SerializeObject(playerResponse, Formatting.Indented));
                }
            }

            Console.WriteLine("Finished.");
            Console.ReadKey();
        }
    }
}