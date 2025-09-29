using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Content = DotnetGeminiSDK.Model.Request.Content;

namespace DotnetGeminiSDK.Client.Interfaces
{
    /// <summary>
    /// The GeminiClient interface provides a set of methods to interact with Google Gemini API.
    /// </summary>
    public interface IGeminiClient
    {
        Task<GeminiMessageResponse?> TextPrompt(
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null,
            CancellationToken cancellationToken = default
        );

        Task<GeminiMessageResponse?> TextPrompt(
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            CancellationToken cancellationToken = default
        );

        Task<GeminiCountTokenMessageResponse?> CountTokens(
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            CancellationToken cancellationToken = default
        );

        Task<GeminiCountTokenMessageResponse?> CountTokens(
            List<string> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            CancellationToken cancellationToken = default
        );

        Task<GeminiCountTokenMessageResponse?> CountTokens(
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            CancellationToken cancellationToken = default
        );

        Task StreamTextPrompt(
            string message,
            Action<string?> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            bool useSSE = false,
            CancellationToken cancellationToken = default
        );

        Task StreamTextPrompt(
            List<Content> messages,
            Action<string?> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            bool useSSE = false,
            CancellationToken cancellationToken = default
        );

        Task<GeminiMessageResponse?> ImagePrompt(string message, byte[] image, ImageMimeType imageMimeType, CancellationToken cancellationToken = default);

        Task<GeminiMessageResponse?> ImagePrompt(string message, string base64Image, ImageMimeType imageMimeType, CancellationToken cancellationToken = default);
        
        Task<GeminiModelResponse?> GetModel(string modelName, CancellationToken cancellationToken = default);

        Task<RootGeminiModelResponse?> GetModels(CancellationToken cancellationToken = default);

        Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(string message, string model = "models/embedding-001", CancellationToken cancellationToken = default);
        
        Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(List<string> message, string model = "models/embedding-001", CancellationToken cancellationToken = default);
        
        Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(List<Content> message, string model = "models/embedding-001", CancellationToken cancellationToken = default);
        
        Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(string message, string model = "models/embedding-001", CancellationToken cancellationToken = default);
        
        Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(List<string> message, string model = "models/embedding-001", CancellationToken cancellationToken = default);
        
        Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(List<Content> message, string model = "models/embedding-001", CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Send a message with function calling support
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="functions">List of function declarations</param>
        /// <param name="generationConfig">Optional generation config</param>
        /// <param name="safetySetting">Optional safety setting</param>
        /// <param name="systemInstruction">Optional system instruction</param>
        /// <returns>Response with potential function calls</returns>
        Task<GeminiMessageResponse?> TextPromptWithFunctions(
            string message,
            List<FunctionDeclaration> functions,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Send a message with system instruction
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="systemInstruction">System instruction to define behavior</param>
        /// <param name="generationConfig">Optional generation config</param>
        /// <param name="safetySetting">Optional safety setting</param>
        /// <returns>Response from the model</returns>
        Task<GeminiMessageResponse?> TextPromptWithSystemInstruction(
            string message,
            string systemInstruction,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Get available models
        /// </summary>
        /// <returns>List of available model names</returns>
        string[] GetAvailableModels();
        
        /// <summary>
        /// Get available multimodal models
        /// </summary>
        /// <returns>List of available multimodal model names</returns>
        string[] GetAvailableMultimodalModels();
        
        /// <summary>
        /// Send a video prompt to the Gemini API
        /// </summary>
        /// <param name="message">Text message to accompany the video</param>
        /// <param name="videoBytes">Video file as byte array</param>
        /// <param name="mimeType">Video MIME type</param>
        /// <param name="generationConfig">Optional generation config</param>
        /// <param name="safetySetting">Optional safety setting</param>
        /// <param name="systemInstruction">Optional system instruction</param>
        /// <returns>Response from the model</returns>
        Task<GeminiMessageResponse?> VideoPrompt(
            string message,
            byte[] videoBytes,
            MediaMimeType mimeType,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Send an audio prompt to the Gemini API
        /// </summary>
        /// <param name="message">Text message to accompany the audio</param>
        /// <param name="audioBytes">Audio file as byte array</param>
        /// <param name="mimeType">Audio MIME type</param>
        /// <param name="generationConfig">Optional generation config</param>
        /// <param name="safetySetting">Optional safety setting</param>
        /// <param name="systemInstruction">Optional system instruction</param>
        /// <returns>Response from the model</returns>
        Task<GeminiMessageResponse?> AudioPrompt(
            string message,
            byte[] audioBytes,
            MediaMimeType mimeType,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Send a multimodal prompt with multiple media types
        /// </summary>
        /// <param name="message">Text message</param>
        /// <param name="mediaFiles">List of media files</param>
        /// <param name="generationConfig">Optional generation config</param>
        /// <param name="safetySetting">Optional safety setting</param>
        /// <param name="systemInstruction">Optional system instruction</param>
        /// <returns>Response from the model</returns>
        Task<GeminiMessageResponse?> MultimodalPrompt(
            string message,
            List<MediaFile> mediaFiles,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Upload a file to Gemini API for processing
        /// </summary>
        /// <param name="fileBytes">File content as byte array</param>
        /// <param name="mimeType">File MIME type</param>
        /// <param name="fileName">Original file name</param>
        /// <returns>File upload response with URI</returns>
        Task<FileUploadResponse?> UploadFile(
            byte[] fileBytes,
            MediaMimeType mimeType,
            string fileName,
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Get file information from Gemini API
        /// </summary>
        /// <param name="fileUri">File URI from upload response</param>
        /// <returns>File information</returns>
        Task<FileInfoResponse?> GetFileInfo(string fileUri, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete a file from Gemini API
        /// </summary>
        /// <param name="fileUri">File URI to delete</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteFile(string fileUri, CancellationToken cancellationToken = default);
    }
}