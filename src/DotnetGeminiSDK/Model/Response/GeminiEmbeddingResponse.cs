using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Response
{
    public class GeminiBatchRootEmbeddingResponse
    {
        [JsonProperty("embeddings", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<GeminiEmbeddingResponse?> Embedding { get; set; }
    }
    
    public class GeminiRootEmbeddingResponse
    {
        [JsonProperty("embedding", NullValueHandling = NullValueHandling.Ignore)]
        public GeminiEmbeddingResponse? Embedding { get; set; }
    }

    public class GeminiEmbeddingResponse
    {
        [JsonProperty("values", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<object?> Values { get; set; } 
    }
}