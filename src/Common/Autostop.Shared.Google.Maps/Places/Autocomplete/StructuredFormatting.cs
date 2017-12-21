using Newtonsoft.Json;

namespace Google.Maps.Places.Autocomplete
{
    public class StructuredFormatting
    {
        [JsonProperty("main_text")]
        public string MainText { get; set; }

        [JsonProperty("main_text_matched_substrings")]
        public MainTextSubsMatch[] MainTextMatchedSubstrings { get; set; }

        [JsonProperty("secondary_text")]
        public string SecondaryText { get; set; }
    }
}