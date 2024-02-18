using System;
using System.Collections.Generic;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
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

        /// <summary>
        /// Send a message to the Google Gemini API
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public GeminiMessageResponse? SendMessage(string message)
        {
            try
            {
                var promptUrl = $"{_config.BaseUrl}:generateContent?key={_config.ApiKey}";
                var request = BuildGeminiRequest(message);

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
        /// <param name="message"></param>
        /// <returns>A request containing GeminiMessageRequest</returns>
        private static GeminiMessageRequest BuildGeminiRequest(string message)
        {
            return new GeminiMessageRequest
            {
                Contents =
                [
                    new RequestContentPart
                    {
                        Parts =
                        [
                            new TextPart
                            {
                                Text = message
                            }
                        ]
                    }
                ]
            };
        }
    }
}