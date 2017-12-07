using Newtonsoft.Json;

namespace FortniteApi.Response.Fortnite
{
    public class StatsResponse
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonIgnore]
        public string[] NameParts => Name.Split('_');

        [JsonProperty("value", Required = Required.Always)]
        public int Value { get; set; }

        [JsonProperty("window", Required = Required.Always)]
        public string Window { get; set; }

        [JsonProperty("ownerType", Required = Required.Always)]
        public int OwnerType { get; set; }
    }
}
