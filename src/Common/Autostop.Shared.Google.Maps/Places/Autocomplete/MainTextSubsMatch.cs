using Newtonsoft.Json;

namespace Google.Maps.Places.Autocomplete
{
    public class MainTextSubsMatch
    {
        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }
    }
}