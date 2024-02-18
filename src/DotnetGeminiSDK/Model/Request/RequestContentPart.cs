using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Request;

public class RequestContentPart
{
    [JsonProperty("parts")] public List<TextPart> Parts { get; set; }
}