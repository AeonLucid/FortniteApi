using System.Collections.Generic;
using System.Linq;
using FortniteApi.Data.Stats;
using FortniteApi.Exceptions;
using FortniteApi.Response.Fortnite;
using FortniteApi.Response.Persona;

namespace FortniteApi.Data
{
    public class Player
    {
        public Player(LookupResponse lookup, IEnumerable<StatsResponse> stats)
        {
            Id = lookup.Id;
            DisplayName = lookup.DisplayName;
            Playlists = stats.GroupBy(x => x.NameParts[(int) NamePart.Playlist]).ToDictionary(group =>
            {
                switch (group.Key)
                {
                    case "p2":
                        return Playlist.Solo;
                    case "p10":
                        return Playlist.Duo;
                    case "p9":
                        return Playlist.Squad;
                    default:
                        throw new FortniteException($"Unknown playlist '{group.Key}'.");
                }
            }, y => y.Select(x => new PlaylistStat(x)).ToArray());
        }

        public string Id { get; }

        public string DisplayName { get; }

        public Dictionary<Playlist, PlaylistStat[]> Playlists { get; }
    }
}
