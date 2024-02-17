namespace DotnetGeminiSDK.Config
{
    public class GoogleGeminiConfig
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1/models/gemini-pro";
    }
}