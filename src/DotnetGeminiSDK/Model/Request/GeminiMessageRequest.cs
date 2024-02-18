using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Request;

public class Content
{
    [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
    public string? Role { get; set; }

    [JsonProperty("parts")] public List<Part> Parts { get; set; }
}

public class Part
{
    [JsonProperty("text")] public string Text { get; set; }
}

public class GeminiMessageRequest
{
    [JsonProperty("contents")] public List<Content> Contents { get; set; }

    [JsonProperty("generationconfig", NullValueHandling = NullValueHandling.Ignore)]
    public GenerationConfig? GenerationConfig { get; set; }

    [JsonProperty("safetysetting", NullValueHandling = NullValueHandling.Ignore)]
    public SafetySetting? SafetySetting { get; set; }
}

public class GenerationConfig
{
    [JsonProperty("stopSequences")] public List<string> StopSequences { get; set; }

    [JsonProperty("temperature")] public double Temperature { get; set; }

    [JsonProperty("maxOutputTokens")] public int MaxOutputTokens { get; set; }

    [JsonProperty("topP")] public double TopP { get; set; }

    [JsonProperty("topK")] public int TopK { get; set; }
}

public class SafetySetting
{
    [JsonProperty("category")] public string Category { get; set; }

    [JsonProperty("threshold")] public string Threshold { get; set; }
}