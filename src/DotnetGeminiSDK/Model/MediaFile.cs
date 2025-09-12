using System;

namespace DotnetGeminiSDK.Model
{
    /// <summary>
    /// Represents a media file for multimodal processing
    /// </summary>
    public class MediaFile
    {
        /// <summary>
        /// File content as byte array
        /// </summary>
        public byte[] Content { get; set; } = Array.Empty<byte>();
        
        /// <summary>
        /// MIME type of the file
        /// </summary>
        public MediaMimeType MimeType { get; set; }
        
        /// <summary>
        /// Original file name
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        
        /// <summary>
        /// File URI (if uploaded to Gemini API)
        /// </summary>
        public string? FileUri { get; set; }
        
        /// <summary>
        /// File size in bytes
        /// </summary>
        public long Size => Content.Length;
        
        /// <summary>
        /// Check if the file is uploaded to Gemini API
        /// </summary>
        public bool IsUploaded => !string.IsNullOrEmpty(FileUri);
        
        /// <summary>
        /// Get the MIME type string
        /// </summary>
        public string MimeTypeString => MimeType.GetMimeTypeString();
        
        /// <summary>
        /// Check if this is an image file
        /// </summary>
        public bool IsImage => MimeType.IsImage();
        
        /// <summary>
        /// Check if this is a video file
        /// </summary>
        public bool IsVideo => MimeType.IsVideo();
        
        /// <summary>
        /// Check if this is an audio file
        /// </summary>
        public bool IsAudio => MimeType.IsAudio();
        
        /// <summary>
        /// Check if this is a document file
        /// </summary>
        public bool IsDocument => MimeType.IsDocument();
    }
}
