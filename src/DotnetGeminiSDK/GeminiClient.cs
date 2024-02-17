using DotnetGeminiSDK.Config;

namespace DotnetGeminiSDK
{
    public class GeminiClient
    {
        private readonly GoogleGeminiConfig _config;

        public GeminiClient(GoogleGeminiConfig config)
        {
            _config = config;
        }

        public string SendMessage(string message)
        {
            try
            {
                var promptUrl = $"{_config.BaseUrl}:generateContent?key=${_config.BaseUrl}";
            }
            catch (Exception e)
            {
                throw new Exception("Unexpected error occurred.", e);
            }
        }
    }
}