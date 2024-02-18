using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Response
{
    public class GeminiMessageResponse
    {
        [JsonProperty("candidates")] public List<Candidate> Candidates { get; set; }

        [JsonProperty("promptfeedback")] public PromptFeedback PromptFeedback { get; set; }
    }
}