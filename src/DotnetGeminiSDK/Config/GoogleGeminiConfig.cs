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
        public string TextBaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1/models/gemini-pro";
        public string ImageBaseUrl { get; set; } =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro-vision";
        public string ModelBaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta/models";
    }
}