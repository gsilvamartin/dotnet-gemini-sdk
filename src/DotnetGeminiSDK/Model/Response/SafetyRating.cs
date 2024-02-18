using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Response
{
    public class SafetyRating
    {
        [JsonProperty("category")] public string Category { get; set; }

        [JsonProperty("probability")] public string Probability { get; set; }
    }
}