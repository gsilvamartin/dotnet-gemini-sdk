using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Requester.Interfaces;

namespace DotnetGeminiSDK
{
    public class GeminiClient
    {
        private readonly IApiRequester _apiRequester;
        private readonly GoogleGeminiConfig _config;

        public GeminiClient(GoogleGeminiConfig config, IApiRequester apiRequester)
        {
            _config = config;
            _apiRequester = apiRequester;
        }

        public string SendMessage(string message)
        {
            try
            {
                var promptUrl = $"{_config.BaseUrl}:generateContent?key=${_config.BaseUrl}";
                var request = BuildGeminiRequest(message);
                
                var response = _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request).Result;
            }
            catch (Exception e)
            {
                throw new Exception("Unexpected error occurred.", e);
            }
        }

        private GeminiMessageRequest BuildGeminiRequest(string message)
        {
            return new GeminiMessageRequest
            {
                Contents = new List<ContentPart>
                {
                    new ContentPart
                    {
                        Parts = new List<TextPart>
                        {
                            new TextPart
                            {
                                Text = message
                            }
                        }
                    }
                }
            };
        }
    }
}