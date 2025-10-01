using System.Text.Json.Serialization;

namespace DotnetGeminiSDK.Model.Response
{
    public class GeminiCountTokenMessageResponse
    {
        [JsonPropertyName("totalTokens")]
        public long TotalTokens { get; set; }
    }
}