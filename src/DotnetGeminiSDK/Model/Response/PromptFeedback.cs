using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Response;

public class PromptFeedback
{
    [JsonProperty("safetyratings")] public List<SafetyRating> SafetyRatings { get; set; }
}