using System;
using Newtonsoft.Json;

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
        [JsonProperty("file")]
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
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// File URI for referencing in API calls
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; } = string.Empty;
        
        /// <summary>
        /// File MIME type
        /// </summary>
        [JsonProperty("mimeType")]
        public string MimeType { get; set; } = string.Empty;
        
        /// <summary>
        /// File size in bytes
        /// </summary>
        [JsonProperty("sizeBytes")]
        public string SizeBytes { get; set; } = string.Empty;
        
        /// <summary>
        /// File creation time
        /// </summary>
        [JsonProperty("createTime")]
        public string CreateTime { get; set; } = string.Empty;
        
        /// <summary>
        /// File update time
        /// </summary>
        [JsonProperty("updateTime")]
        public string UpdateTime { get; set; } = string.Empty;
        
        /// <summary>
        /// File expiration time
        /// </summary>
        [JsonProperty("expirationTime")]
        public string ExpirationTime { get; set; } = string.Empty;
        
        /// <summary>
        /// File state
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; } = string.Empty;
        
        /// <summary>
        /// File display name
        /// </summary>
        [JsonProperty("displayName")]
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
        [JsonProperty("file")]
        public FileInfo File { get; set; } = new();
    }
}
