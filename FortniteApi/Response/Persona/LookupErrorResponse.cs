using Newtonsoft.Json;

namespace FortniteApi.Response.Persona
{
    public class LookupErrorResponse
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("messageVars")]
        public string[] MessageVars { get; set; }

        [JsonProperty("numericErrorCode")]
        public int NumericErrorCode { get; set; }

        [JsonProperty("originatingService")]
        public string OriginatingService { get; set; }

        [JsonProperty("intent")]
        public string Intent { get; set; }
    }
}
