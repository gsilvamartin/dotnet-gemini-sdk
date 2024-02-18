using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Request
{
    public class TextPart
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}