using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DotnetGeminiSDK.Model.Request
{
    public class GeminiEmbeddedMultipleRequest
    {
        [JsonPropertyName("requests")]
        public IEnumerable<GeminiEmbeddedMessageRequest> Requests { get; set; }
    }

    public class GeminiEmbeddedMessageRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }
        
        [JsonPropertyName("content")]
        public Content Content { get; set; }
    }
}