using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Response
{
    public class Content
    {
        [JsonProperty("parts")] public List<ContentPart> Parts { get; set; }

        [JsonProperty("role")] public string Role { get; set; }
    }
}