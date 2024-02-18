namespace DotnetGeminiSDK.Config
{
    public class GoogleGeminiConfig
    {
        public string ApiKey { get; set; }
        public string TextBaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1/models/gemini-pro";
        public string ImageBaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1/models/gemini-pro";
    }
}