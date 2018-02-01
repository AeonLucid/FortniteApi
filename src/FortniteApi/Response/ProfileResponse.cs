using System.Collections.Generic;
using Newtonsoft.Json;

namespace FortniteApi.Response
{
    public class ProfileResponse
    {
        public string AccountId { get; set; }

        public int PlatformId { get; set; }

        public string PlatformName { get; set; }

        public string PlatformNameLong { get; set; }

        public string EpicUserHandle { get; set; }

        /// <summary>
        ///     Use <see cref="Data.Playlist"/> and <see cref="Data.Stat"/> to access the correct stats.
        /// </summary>
        public Dictionary<string, Dictionary<string, ProfileStat>> Stats { get; set; }

        public List<ProfileLifeTimeStat> LifeTimeStats { get; set; }

        public List<ProfileMatch> RecentMatches { get; set; }
    }

    public class ProfileStat
    {
        public string Label { get; set; }

        public string Field { get; set; }

        public string Category { get; set; }

        public int ValueInt { get; set; }

        public double ValueDec { get; set; }

        public string Value { get; set; }

        public int Rank { get; set; }

        public double Percentile { get; set; }

        public string DisplayValue { get; set; }
    }

    public class ProfileLifeTimeStat
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public class ProfileMatch
    {
        public long Id { get; set; }

        public string AccountId { get; set; }

        public string Playlist { get; set; }

        public int Kills { get; set; }

        public int MinutesPlayed { get; set; }

        public int Top1 { get; set; }

        public int Top3 { get; set; }

        public int Top5 { get; set; }

        public int Top6 { get; set; }

        public int Top10 { get; set; }

        public int Top12 { get; set; }

        public int Top25 { get; set; }

        public int Matches { get; set; }

        public string DateCollected { get; set; }

        public int Score { get; set; }

        [JsonProperty("platform")]
        public int PlatformId { get; set; }
    }
}