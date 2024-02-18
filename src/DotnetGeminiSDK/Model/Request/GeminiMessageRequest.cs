using System.Collections.Generic;
using DotnetGeminiSDK.Model.Response;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Request
{
    public class GeminiMessageRequest
    {
        [JsonProperty("contents")]
        public List<RequestContentPart> Contents { get; set; }
    }
}