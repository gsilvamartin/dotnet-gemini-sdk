namespace DotnetGeminiSDK.Model
{
    /// <summary>
    /// Available Gemini models and their capabilities
    /// </summary>
    public static class GeminiModels
    {
        /// <summary>
        /// Gemini 2.5 Pro - Most advanced model with enhanced reasoning and multimodal capabilities
        /// Supports text, images, video, and audio processing with up to 1M token context window
        /// </summary>
        public const string Gemini25Pro = "gemini-2.5-pro";
        
        /// <summary>
        /// Gemini 1.5 Pro - Advanced model with large context window
        /// Supports text, images, video, and audio processing with up to 1M token context window
        /// </summary>
        public const string Gemini15Pro = "gemini-1.5-pro";
        
        /// <summary>
        /// Gemini 1.5 Flash - Fast and efficient model
        /// Optimized for speed and cost-effectiveness with multimodal support
        /// </summary>
        public const string Gemini15Flash = "gemini-1.5-flash";
        
        /// <summary>
        /// Gemini 1.5 Flash 8B - Lightweight version of Flash
        /// Ultra-fast model for simple tasks and high-throughput scenarios
        /// </summary>
        public const string Gemini15Flash8B = "gemini-1.5-flash-8b";
        
        /// <summary>
        /// Gemini 1.0 Pro - Legacy model
        /// Original text generation model (deprecated, use 1.5 Pro instead)
        /// </summary>
        public const string Gemini10Pro = "gemini-pro";
        
        /// <summary>
        /// Gemini 1.0 Pro Vision - Legacy vision model
        /// Original multimodal model for text and images (deprecated, use 1.5 Pro instead)
        /// </summary>
        public const string Gemini10ProVision = "gemini-pro-vision";
        
        /// <summary>
        /// Embedding model for generating vector representations of text
        /// Used for semantic search, similarity, and clustering tasks
        /// </summary>
        public const string Embedding001 = "models/embedding-001";
        
        /// <summary>
        /// Get all available text models
        /// </summary>
        public static readonly string[] TextModels = 
        {
            Gemini25Pro,
            Gemini15Pro,
            Gemini15Flash,
            Gemini15Flash8B,
            Gemini10Pro
        };
        
        /// <summary>
        /// Get all available multimodal models
        /// </summary>
        public static readonly string[] MultimodalModels = 
        {
            Gemini25Pro,
            Gemini15Pro,
            Gemini15Flash,
            Gemini10ProVision
        };
        
        /// <summary>
        /// Get all available embedding models
        /// </summary>
        public static readonly string[] EmbeddingModels = 
        {
            Embedding001
        };
    }
}
