using Newtonsoft.Json;

namespace AntiTruble.ClassLibrary
{
    public class MksResponseResult
    {
        public bool Success { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }
}
