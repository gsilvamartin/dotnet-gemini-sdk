using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Response;

public class ContentPart
{
    [JsonProperty("parts")] public List<TextPartRes> Parts { get; set; }
}