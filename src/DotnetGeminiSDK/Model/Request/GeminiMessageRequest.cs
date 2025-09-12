using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Request
{
    public class Content
    {
        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public string? Role { get; set; }

        [JsonProperty("parts")] public List<Part> Parts { get; set; }
    }

    public class Part
    {
        [JsonProperty("text")] public string Text { get; set; }

        [JsonProperty("inlineData", NullValueHandling = NullValueHandling.Ignore)]
        public InlineData? InlineData { get; set; }
    }

    public class GeminiMessageRequest
    {
        [JsonProperty("contents")] public List<Content> Contents { get; set; }

        [JsonProperty("generationConfig", NullValueHandling = NullValueHandling.Ignore)]
        public GenerationConfig? GenerationConfig { get; set; }

        [JsonProperty("safetySetting", NullValueHandling = NullValueHandling.Ignore)]
        public SafetySetting? SafetySetting { get; set; }
        
        [JsonProperty("systemInstruction", NullValueHandling = NullValueHandling.Ignore)]
        public SystemInstruction? SystemInstruction { get; set; }
        
        [JsonProperty("tools", NullValueHandling = NullValueHandling.Ignore)]
        public List<Tool>? Tools { get; set; }
        
        [JsonProperty("toolConfig", NullValueHandling = NullValueHandling.Ignore)]
        public ToolConfig? ToolConfig { get; set; }
    }

    public class GenerationConfig
    {
        [JsonProperty("stopSequences")] public List<string> StopSequences { get; set; }

        [JsonProperty("temperature")] public double Temperature { get; set; }

        [JsonProperty("maxOutputTokens")] public int MaxOutputTokens { get; set; }

        [JsonProperty("topP")] public double TopP { get; set; }

        [JsonProperty("topK")] public int TopK { get; set; }

        /// <summary>
        /// The random seed to use for generation. When set, the model will generate deterministic responses.
        /// This is useful for testing and reproducible results.
        /// </summary>
        [JsonProperty("seed")]
        public int? Seed { get; set; }

        /// <summary>
        /// The MIME type of the response. Use "application/json" to request JSON responses.
        /// This enables structured output generation with schema validation.
        /// </summary>
        [JsonProperty("response_mime_type")]
        public string? ResponseMimeType { get; set; }

        /// <summary>
        /// The JSON schema that defines the expected structure of the response.
        /// Only used when ResponseMimeType is "application/json".
        /// Can be provided as a JObject or JSON string.
        /// </summary>
        [JsonProperty("response_schema")]
        public object? ResponseSchema { get; set; }
    }

    public class SafetySetting
    {
        [JsonProperty("category")] public string Category { get; set; }

        [JsonProperty("threshold")] public string Threshold { get; set; }
    }

    public class InlineData
    {
        [JsonProperty("mime_type")] public string MimeType { get; set; }

        [JsonProperty("data")] public string Data { get; set; }
    }
    
    /// <summary>
    /// System instruction to define the behavior of the model
    /// </summary>
    public class SystemInstruction
    {
        [JsonProperty("parts")] public List<Part> Parts { get; set; }
    }
    
    /// <summary>
    /// Tool definition for function calling
    /// </summary>
    public class Tool
    {
        [JsonProperty("functionDeclarations")] public List<FunctionDeclaration> FunctionDeclarations { get; set; }
    }
    
    /// <summary>
    /// Function declaration for tools
    /// </summary>
    public class FunctionDeclaration
    {
        [JsonProperty("name")] public string Name { get; set; }
        
        [JsonProperty("description")] public string Description { get; set; }
        
        [JsonProperty("parameters")] public FunctionParameters Parameters { get; set; }
    }
    
    /// <summary>
    /// Function parameters schema
    /// </summary>
    public class FunctionParameters
    {
        [JsonProperty("type")] public string Type { get; set; } = "object";
        
        [JsonProperty("properties")] public Dictionary<string, FunctionParameter> Properties { get; set; }
        
        [JsonProperty("required")] public List<string> Required { get; set; }
    }
    
    /// <summary>
    /// Individual function parameter
    /// </summary>
    public class FunctionParameter
    {
        [JsonProperty("type")] public string Type { get; set; }
        
        [JsonProperty("description")] public string Description { get; set; }
        
        [JsonProperty("enum")] public List<string>? Enum { get; set; }
    }
    
    /// <summary>
    /// Tool configuration
    /// </summary>
    public class ToolConfig
    {
        [JsonProperty("functionCallingConfig")] public FunctionCallingConfig FunctionCallingConfig { get; set; }
    }
    
    /// <summary>
    /// Function calling configuration
    /// </summary>
    public class FunctionCallingConfig
    {
        [JsonProperty("mode")] public string Mode { get; set; } = "AUTO";
        
        [JsonProperty("allowedFunctionNames")] public List<string>? AllowedFunctionNames { get; set; }
    }
}