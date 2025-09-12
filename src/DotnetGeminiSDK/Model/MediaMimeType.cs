using System;

namespace DotnetGeminiSDK.Model
{
    /// <summary>
    /// Supported media MIME types for Gemini API
    /// </summary>
    public enum MediaMimeType
    {
        // Image types
        ImageJpeg,
        ImagePng,
        ImageJpg,
        ImageHeic,
        ImageHeif,
        ImageWebp,
        ImageGif,
        ImageBmp,
        ImageTiff,
        
        // Video types
        VideoMp4,
        VideoAvi,
        VideoMov,
        VideoWmv,
        VideoFlv,
        VideoWebm,
        VideoMkv,
        Video3gp,
        
        // Audio types
        AudioMp3,
        AudioWav,
        AudioAac,
        AudioOgg,
        AudioFlac,
        AudioM4a,
        AudioWma,
        AudioAiff,
        
        // Document types
        ApplicationPdf,
        TextPlain,
        TextHtml,
        TextCsv,
        ApplicationJson,
        ApplicationXml,
        
        // Archive types
        ApplicationZip,
        ApplicationRar,
        Application7z,
        ApplicationTar,
        ApplicationGzip
    }
    
    /// <summary>
    /// Extension methods for MediaMimeType
    /// </summary>
    public static class MediaMimeTypeExtensions
    {
        /// <summary>
        /// Get the MIME type string for the enum value
        /// </summary>
        /// <param name="mimeType">The MIME type enum</param>
        /// <returns>The MIME type string</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the MIME type is not supported</exception>
        public static string GetMimeTypeString(this MediaMimeType mimeType)
        {
            return mimeType switch
            {
                // Image types
                MediaMimeType.ImageJpeg => "image/jpeg",
                MediaMimeType.ImagePng => "image/png",
                MediaMimeType.ImageJpg => "image/jpg",
                MediaMimeType.ImageHeic => "image/heic",
                MediaMimeType.ImageHeif => "image/heif",
                MediaMimeType.ImageWebp => "image/webp",
                MediaMimeType.ImageGif => "image/gif",
                MediaMimeType.ImageBmp => "image/bmp",
                MediaMimeType.ImageTiff => "image/tiff",
                
                // Video types
                MediaMimeType.VideoMp4 => "video/mp4",
                MediaMimeType.VideoAvi => "video/avi",
                MediaMimeType.VideoMov => "video/quicktime",
                MediaMimeType.VideoWmv => "video/x-ms-wmv",
                MediaMimeType.VideoFlv => "video/x-flv",
                MediaMimeType.VideoWebm => "video/webm",
                MediaMimeType.VideoMkv => "video/x-matroska",
                MediaMimeType.Video3gp => "video/3gpp",
                
                // Audio types
                MediaMimeType.AudioMp3 => "audio/mpeg",
                MediaMimeType.AudioWav => "audio/wav",
                MediaMimeType.AudioAac => "audio/aac",
                MediaMimeType.AudioOgg => "audio/ogg",
                MediaMimeType.AudioFlac => "audio/flac",
                MediaMimeType.AudioM4a => "audio/mp4",
                MediaMimeType.AudioWma => "audio/x-ms-wma",
                MediaMimeType.AudioAiff => "audio/aiff",
                
                // Document types
                MediaMimeType.ApplicationPdf => "application/pdf",
                MediaMimeType.TextPlain => "text/plain",
                MediaMimeType.TextHtml => "text/html",
                MediaMimeType.TextCsv => "text/csv",
                MediaMimeType.ApplicationJson => "application/json",
                MediaMimeType.ApplicationXml => "application/xml",
                
                // Archive types
                MediaMimeType.ApplicationZip => "application/zip",
                MediaMimeType.ApplicationRar => "application/x-rar-compressed",
                MediaMimeType.Application7z => "application/x-7z-compressed",
                MediaMimeType.ApplicationTar => "application/x-tar",
                MediaMimeType.ApplicationGzip => "application/gzip",
                
                _ => throw new ArgumentOutOfRangeException(nameof(mimeType), mimeType, "Unsupported media MIME type.")
            };
        }
        
        /// <summary>
        /// Check if the MIME type is an image
        /// </summary>
        /// <param name="mimeType">The MIME type enum</param>
        /// <returns>True if it's an image type</returns>
        public static bool IsImage(this MediaMimeType mimeType)
        {
            return mimeType.ToString().StartsWith("Image");
        }
        
        /// <summary>
        /// Check if the MIME type is a video
        /// </summary>
        /// <param name="mimeType">The MIME type enum</param>
        /// <returns>True if it's a video type</returns>
        public static bool IsVideo(this MediaMimeType mimeType)
        {
            return mimeType.ToString().StartsWith("Video");
        }
        
        /// <summary>
        /// Check if the MIME type is audio
        /// </summary>
        /// <param name="mimeType">The MIME type enum</param>
        /// <returns>True if it's an audio type</returns>
        public static bool IsAudio(this MediaMimeType mimeType)
        {
            return mimeType.ToString().StartsWith("Audio");
        }
        
        /// <summary>
        /// Check if the MIME type is a document
        /// </summary>
        /// <param name="mimeType">The MIME type enum</param>
        /// <returns>True if it's a document type</returns>
        public static bool IsDocument(this MediaMimeType mimeType)
        {
            return mimeType.ToString().StartsWith("Text") || mimeType.ToString().StartsWith("Application");
        }
    }
}
