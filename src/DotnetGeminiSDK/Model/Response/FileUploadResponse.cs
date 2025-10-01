using System.Text.Json.Serialization;

namespace DotnetGeminiSDK.Model.Response
{
    /// <summary>
    /// Response from file upload to Gemini API
    /// </summary>
    public class FileUploadResponse
    {
        /// <summary>
        /// The uploaded file information
        /// </summary>
        [JsonPropertyName("file")]
        public FileInfo File { get; set; } = new();
    }
    
    /// <summary>
    /// File information from upload response
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// File name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// File URI for referencing in API calls
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; } = string.Empty;
        
        /// <summary>
        /// File MIME type
        /// </summary>
        [JsonPropertyName("mimeType")]
        public string MimeType { get; set; } = string.Empty;
        
        /// <summary>
        /// File size in bytes
        /// </summary>
        [JsonPropertyName("sizeBytes")]
        public string SizeBytes { get; set; } = string.Empty;
        
        /// <summary>
        /// File creation time
        /// </summary>
        [JsonPropertyName("createTime")]
        public string CreateTime { get; set; } = string.Empty;
        
        /// <summary>
        /// File update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public string UpdateTime { get; set; } = string.Empty;
        
        /// <summary>
        /// File expiration time
        /// </summary>
        [JsonPropertyName("expirationTime")]
        public string ExpirationTime { get; set; } = string.Empty;
        
        /// <summary>
        /// File state
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;
        
        /// <summary>
        /// File display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// Response from getting file information
    /// </summary>
    public class FileInfoResponse
    {
        /// <summary>
        /// The file information
        /// </summary>
        [JsonPropertyName("file")]
        public FileInfo File { get; set; } = new();
    }
}
