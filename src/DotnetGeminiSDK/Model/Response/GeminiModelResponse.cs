using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Response
{
    public class RootGeminiModelResponse
    {
        [JsonProperty("models", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<GeminiModelResponse?> GeminiModelResponses { get; set; } = new List<GeminiModelResponse>();
    }

    public class GeminiModelResponse
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("displayName", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("inputTokenLimit", NullValueHandling = NullValueHandling.Ignore)]
        public int InputTokenLimit { get; set; }

        [JsonProperty("outputTokenLimit", NullValueHandling = NullValueHandling.Ignore)]
        public int OutputTokenLimit { get; set; }

        [JsonProperty("supportedGenerationMethods", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SupportedGenerationMethods { get; set; }

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public double Temperature { get; set; }

        [JsonProperty("topP", NullValueHandling = NullValueHandling.Ignore)]
        public double TopP { get; set; }

        [JsonProperty("topK", NullValueHandling = NullValueHandling.Ignore)]
        public double TopK { get; set; }
    }
}