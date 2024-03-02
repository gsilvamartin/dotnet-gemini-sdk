using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Response
{
    public class GeminiCountTokenMessageResponse
    {
        [JsonProperty("totalTokens")]
        public long TotalTokens { get; set; }
    }
}