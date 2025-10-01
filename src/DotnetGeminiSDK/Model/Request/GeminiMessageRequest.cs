using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DotnetGeminiSDK.Model.Request
{
    public class Content
    {
        [JsonPropertyName("role")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Role { get; set; }

        [JsonPropertyName("parts")] public List<Part> Parts { get; set; }
    }

    public class Part
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("inlineData")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public InlineData? InlineData { get; set; }
    }

    public class GeminiMessageRequest
    {
        [JsonPropertyName("contents")]
        public List<Content> Contents { get; set; }

        [JsonPropertyName("generationConfig")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GenerationConfig? GenerationConfig { get; set; }

        [JsonPropertyName("safetySetting")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SafetySetting? SafetySetting { get; set; }

        [JsonPropertyName("systemInstruction")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SystemInstruction? SystemInstruction { get; set; }

        [JsonPropertyName("tools")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Tool>? Tools { get; set; } 

        [JsonPropertyName("toolConfig")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ToolConfig? ToolConfig { get; set; }
    }

    public class GenerationConfig
    {
        [JsonPropertyName("stopSequences")] public List<string> StopSequences { get; set; }

        [JsonPropertyName("temperature")] public double Temperature { get; set; }

        [JsonPropertyName("maxOutputTokens")] public int MaxOutputTokens { get; set; }

        [JsonPropertyName("topP")] public double TopP { get; set; }

        [JsonPropertyName("topK")] public int TopK { get; set; }

        /// <summary>
        /// The random seed to use for generation. When set, the model will generate deterministic responses.
        /// This is useful for testing and reproducible results.
        /// </summary>
        [JsonPropertyName("seed")]
        public int? Seed { get; set; }

        /// <summary>
        /// The MIME type of the response. Use "application/json" to request JSON responses.
        /// This enables structured output generation with schema validation.
        /// </summary>
        [JsonPropertyName("response_mime_type")]
        public string? ResponseMimeType { get; set; }

        /// <summary>
        /// The JSON schema that defines the expected structure of the response.
        /// Only used when ResponseMimeType is "application/json".
        /// Can be provided as a JObject or JSON string.
        /// </summary>
        [JsonPropertyName("response_schema")]
        public object? ResponseSchema { get; set; }
    }

    public class SafetySetting
    {
        [JsonPropertyName("category")] public string Category { get; set; }

        [JsonPropertyName("threshold")] public string Threshold { get; set; }
    }

    public class InlineData
    {
        [JsonPropertyName("mime_type")] public string MimeType { get; set; }

        [JsonPropertyName("data")] public string Data { get; set; }
    }

    /// <summary>
    /// System instruction to define the behavior of the model
    /// </summary>
    public class SystemInstruction
    {
        [JsonPropertyName("parts")] public List<Part> Parts { get; set; }
    }

    /// <summary>
    /// Tool definition for function calling
    /// </summary>
    public class Tool
    {
        [JsonPropertyName("functionDeclarations")] public List<FunctionDeclaration> FunctionDeclarations { get; set; }
    }

    /// <summary>
    /// Function declaration for tools
    /// </summary>
    public class FunctionDeclaration
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("description")] public string Description { get; set; }

        [JsonPropertyName("parameters")] public FunctionParameters Parameters { get; set; }
    }

    /// <summary>
    /// Function parameters schema
    /// </summary>
    public class FunctionParameters
    {
        [JsonPropertyName("type")] public string Type { get; set; } = "object";

        [JsonPropertyName("properties")] public Dictionary<string, FunctionParameter> Properties { get; set; }

        [JsonPropertyName("required")] public List<string> Required { get; set; }
    }

    /// <summary>
    /// Individual function parameter
    /// </summary>
    public class FunctionParameter
    {
        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("description")] public string Description { get; set; }

        [JsonPropertyName("enum")] public List<string>? Enum { get; set; }
    }

    /// <summary>
    /// Tool configuration
    /// </summary>
    public class ToolConfig
    {
        [JsonPropertyName("functionCallingConfig")] public FunctionCallingConfig FunctionCallingConfig { get; set; }
    }

    /// <summary>
    /// Function calling configuration
    /// </summary>
    public class FunctionCallingConfig
    {
        [JsonPropertyName("mode")] public string Mode { get; set; } = "AUTO";

        [JsonPropertyName("allowedFunctionNames")] public List<string>? AllowedFunctionNames { get; set; }
    }
}