using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Response;

public class TextPartRes
{
    [JsonProperty("text")] public string Text { get; set; }
}