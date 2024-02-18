using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Response
{
    public class Candidate
    {
        [JsonProperty("content")] public Content Content { get; set; }

        [JsonProperty("finishreason")] public string FinishReason { get; set; }

        [JsonProperty("index")] public int Index { get; set; }

        [JsonProperty("safetyratings")] public List<SafetyRating> SafetyRatings { get; set; }
    }
}