using FortniteApi.Data.Stats;
using FortniteApi.Response.Fortnite;

namespace FortniteApi.Data
{
    public class PlaylistStat
    {
        public PlaylistStat(StatsResponse response)
        {
            StatType = response.NameParts[(int) NamePart.StatType];
            Value = response.Value;
        }

        public string StatType { get; }

        public int Value { get; set; }
    }
}
