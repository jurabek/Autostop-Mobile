using Newtonsoft.Json;

namespace Google.Maps.Places.Autocomplete
{
    public class AutocompleteTerm
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }
    }
}