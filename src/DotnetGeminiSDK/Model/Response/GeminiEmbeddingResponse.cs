using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DotnetGeminiSDK.Model.Response
{
    public class GeminiBatchRootEmbeddingResponse
    {
        [JsonPropertyName("embeddings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<GeminiEmbeddingResponse?> Embedding { get; set; }
    }
    
    public class GeminiRootEmbeddingResponse
    {
        [JsonPropertyName("embedding")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GeminiEmbeddingResponse? Embedding { get; set; }
    }

    public class GeminiEmbeddingResponse
    {
        [JsonPropertyName("values")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<object?> Values { get; set; } 
    }
}