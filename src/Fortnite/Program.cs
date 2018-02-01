using System;
using System.Threading.Tasks;
using FortniteApi;
using FortniteApi.Data;

namespace Fortnite
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.Title = "Fortnite";
            Console.ForegroundColor = ConsoleColor.White;

            await DoStuff();

            Console.WriteLine("Finished.");
            Console.ReadKey();
        }

        private static async Task DoStuff()
        {
            using (var client = new FortniteClient(Environment.GetEnvironmentVariable("TRN_KEY")))
            {
                Console.WriteLine("Looking up player ids..");
                var players = new[]
                {
                    "AeonLuciid",
                    "AeonLucidUtil"
                };

                foreach (var player in players)
                {
                    Console.WriteLine($"Looking up {player}");

                    var response = await client.FindPlayerAsync(Platform.Pc, player);

                    var soloKills = response.Stats[Playlist.Solo][Stat.Kills].ValueInt;
                }
            }
        }
    }
}