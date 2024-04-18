using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Request
{
    public class GeminiEmbeddedMultipleRequest
    {
        [JsonProperty("requests")]
        public IEnumerable<GeminiEmbeddedMessageRequest> Requests { get; set; }
    }

    public class GeminiEmbeddedMessageRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; }
        
        [JsonProperty("content")]
        public Content Content { get; set; }
    }
}