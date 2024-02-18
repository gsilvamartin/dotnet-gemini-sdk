using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
using DotnetGeminiSDK.Requester.Interfaces;
using Content = DotnetGeminiSDK.Model.Request.Content;
using Part = DotnetGeminiSDK.Model.Request.Part;

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

        /// <summary>
        /// Send a message to the Google Gemini API
        /// </summary>
        /// <param name="message"></param>
        /// <param name="generationConfig"></param>
        /// <param name="safetySetting"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public GeminiMessageResponse? SendMessage(
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            try
            {
                if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

                var promptUrl = $"{_config.TextBaseUrl}:generateContent?key={_config.ApiKey}";
                var request = BuildGeminiRequest(message, generationConfig, safetySetting);

                return _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request).Result;
            }
            catch (Exception e)
            {
                throw new Exception("Unexpected error occurred.", e);
            }
        }

        public GeminiMessageResponse? SendMessages(
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            try
            {
                if (!messages.Any()) throw new ArgumentException("Messages cannot be empty.");

                var promptUrl = $"{_config.TextBaseUrl}:generateContent?key={_config.ApiKey}";
                var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

                return _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request).Result;
            }
            catch (Exception e)
            {
                throw new Exception("Unexpected error occurred.", e);
            }
        }

        /// <summary>
        /// Build a GeminiMessageRequest object from a string message
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="generationConfig"></param>
        /// <param name="safetySetting"></param>
        /// <returns>A request containing GeminiMessageRequest</returns>
        private static GeminiMessageRequest BuildGeminiRequest(
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            return new GeminiMessageRequest
            {
                Contents = messages,
                GenerationConfig = generationConfig,
                SafetySetting = safetySetting
            };
        }

        /// <summary>
        /// Build a GeminiMessageRequest object from a string message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="generationConfig"></param>
        /// <param name="safetySetting"></param>
        /// <returns>A request containing GeminiMessageRequest</returns>
        private static GeminiMessageRequest BuildGeminiRequest(
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            return new GeminiMessageRequest
            {
                Contents =
                [
                    new Content
                    {
                        Parts =
                        [
                            new Part
                            {
                                Text = message
                            }
                        ]
                    }
                ],
                GenerationConfig = generationConfig,
                SafetySetting = safetySetting
            };
        }
    }
}