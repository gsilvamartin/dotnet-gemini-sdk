using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
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
            SafetySetting? safetySetting = null
        );

        Task<GeminiMessageResponse?> TextPrompt(
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null
        );

        Task<GeminiCountTokenMessageResponse?> CountTokens(
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null
        );

        Task<GeminiCountTokenMessageResponse?> CountTokens(
            List<string> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null
        );

        Task<GeminiCountTokenMessageResponse?> CountTokens(
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null
        );

        Task StreamTextPrompt(
            string message,
            Action<string?> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            bool useSSE = false
        );

        Task StreamTextPrompt(
            List<Content> messages,
            Action<string?> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            bool useSSE = false
        );

        Task<GeminiMessageResponse?> ImagePrompt(string message, byte[] image, ImageMimeType imageMimeType);

        Task<GeminiMessageResponse?> ImagePrompt(string message, string base64Image, ImageMimeType imageMimeType);
        
        Task<GeminiModelResponse?> GetModel(string modelName);

        Task<RootGeminiModelResponse?> GetModels();

        Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(string message, string model = "models/embedding-001");
        
        Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(List<string> message, string model = "models/embedding-001");
        
        Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(List<Content> message, string model = "models/embedding-001");
        
        Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(string message, string model = "models/embedding-001");
        
        Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(List<string> message, string model = "models/embedding-001");
        
        Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(List<Content> message, string model = "models/embedding-001");
    }
}