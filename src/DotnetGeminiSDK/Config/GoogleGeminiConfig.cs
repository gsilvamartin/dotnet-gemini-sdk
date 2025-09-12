namespace DotnetGeminiSDK.Config
{
    /// <summary>
    /// Configuration for the Gemini client
    ///
    /// If you don't have an API key, you can get one from the Google AI Studio.
    /// </summary>
    public class GoogleGeminiConfig
    {
        public string ApiKey { get; set; }
        public string TextBaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro";
        public string ImageBaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro";
        public string ModelBaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta/models";
        public string EmbeddingBaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta/models";
        
        /// <summary>
        /// Default model to use for text generation
        /// </summary>
        public string DefaultTextModel { get; set; } = "gemini-1.5-pro";
        
        /// <summary>
        /// Default model to use for image processing
        /// </summary>
        public string DefaultImageModel { get; set; } = "gemini-1.5-pro";
        
        /// <summary>
        /// Default model to use for embeddings
        /// </summary>
        public string DefaultEmbeddingModel { get; set; } = "models/embedding-001";
        
        /// <summary>
        /// Enable retry logic for failed requests
        /// </summary>
        public bool EnableRetry { get; set; } = true;
        
        /// <summary>
        /// Maximum number of retry attempts
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;
        
        /// <summary>
        /// Delay between retry attempts in milliseconds
        /// </summary>
        public int RetryDelayMs { get; set; } = 1000;
        
        /// <summary>
        /// Enable structured logging
        /// </summary>
        public bool EnableLogging { get; set; } = false;
        
        /// <summary>
        /// Request timeout in seconds
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;
    }
}