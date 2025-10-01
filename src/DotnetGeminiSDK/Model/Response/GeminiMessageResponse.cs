using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DotnetGeminiSDK.Model.Response
{
    public class GeminiMessageResponse
    {
        [JsonPropertyName("candidates")] public List<Candidate> Candidates { get; set; }

        [JsonPropertyName("promptFeedback")] public PromptFeedback PromptFeedback { get; set; }


        /// <summary>
        /// Returns all of the response texts from the GeminiMessageResponse
        /// </summary>
        /// <returns>Returns a list<string> with all the response texts from the GeminiMessageResponse</returns>
        public List<string> GetResponseText(){
            var responseList = new List<string>();

            foreach (var candidate in Candidates){
                if (!string.IsNullOrEmpty(candidate?.Content?.Parts[0]?.Text)){
                    responseList.Add(candidate?.Content?.Parts[0]?.Text);
                }
            }

            return responseList;
        }
    }

    public class Candidate
    {
        [JsonPropertyName("content")] public Content Content { get; set; }

        [JsonPropertyName("finishReason")] public string FinishReason { get; set; }

        [JsonPropertyName("index")] public int Index { get; set; }

        [JsonPropertyName("safetyRatings")] public List<SafetyRating> SafetyRatings { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("parts")] public List<Part> Parts { get; set; }

        [JsonPropertyName("role")] public string Role { get; set; }
    }

    public class Part
    {
        [JsonPropertyName("text")] public string Text { get; set; }
        
        [JsonPropertyName("functionCall")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FunctionCall? FunctionCall { get; set; }
        
        [JsonPropertyName("functionResponse")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FunctionResponse? FunctionResponse { get; set; }
    }

    public class PromptFeedback
    {
        [JsonPropertyName("safetyRatings")] public List<SafetyRating> SafetyRatings { get; set; }
    }

    public class SafetyRating
    {
        [JsonPropertyName("category")] public string Category { get; set; }

        [JsonPropertyName("probability")] public string Probability { get; set; }
    }
    
    /// <summary>
    /// Function call from the model
    /// </summary>
    public class FunctionCall
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        
        [JsonPropertyName("args")] public Dictionary<string, object> Args { get; set; }
    }
    
    /// <summary>
    /// Function response to the model
    /// </summary>
    public class FunctionResponse
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        
        [JsonPropertyName("response")] public object Response { get; set; }
    }
}