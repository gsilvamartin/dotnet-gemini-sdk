using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
using DotnetGeminiSDK.Requester.Interfaces;
using Content = DotnetGeminiSDK.Model.Request.Content;
using Part = DotnetGeminiSDK.Model.Request.Part;

namespace DotnetGeminiSDK.Client
{
    /// <summary>
    /// Class to interact with Google Gemini API
    /// </summary>
    public class GeminiClient : IGeminiClient
    {
        private readonly IApiRequester _apiRequester;
        private readonly GoogleGeminiConfig _config;

        public GeminiClient(GoogleGeminiConfig config, IApiRequester apiRequester)
        {
            _config = config;
            _apiRequester = apiRequester;
        }

        /// <summary>
        /// Send a message to be processed using Google Gemini API,
        /// the method returns a GeminiMessageResponse with all the response fields from api
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-only_input
        /// </summary>
        /// <param name="message"></param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GeminiMessageResponse?> TextPrompt(
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            try
            {
                if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

                var promptUrl = $"{_config.TextBaseUrl}:generateContent?key={_config.ApiKey}";
                var request = BuildGeminiRequest(message, generationConfig, safetySetting);

                return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
            }
            catch (Exception e)
            {
                throw new Exception("Unexpected error occurred.", e);
            }
        }

        /// <summary>
        /// Send multiple messages to be processed using Google Gemini API,
        /// the method returns a GeminiMessageResponse with all the response fields from api
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-only_input
        /// </summary>
        /// <param name="messages">Messages to be processed as content model</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
        /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<GeminiMessageResponse?> TextPrompt(
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            if (!messages.Any()) throw new ArgumentException("Messages cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }

        /// <summary>
        /// Send a message to be processed using Google Gemini API.
        ///
        /// The method returns all the tokens count from the message.
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#count_tokens
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
        /// <returns>Returns a GeminiMessageResponse with the counted tokens</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<GeminiCountTokenMessageResponse?> CountTokens(string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message, generationConfig, safetySetting);

            return await _apiRequester.PostAsync<GeminiCountTokenMessageResponse>(promptUrl, request);
        }

        /// <summary>
        /// Send a message to be processed using Google Gemini API.
        ///
        /// The method returns all the tokens count from the message.
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#count_tokens
        /// </summary>
        /// <param name="messages">Messages to be processed</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
        /// <returns>Returns a GeminiMessageResponse with the counted tokens</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<GeminiCountTokenMessageResponse?> CountTokens(List<string> messages,
            GenerationConfig? generationConfig = null, SafetySetting? safetySetting = null)
        {
            if (!messages.Any()) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

            return await _apiRequester.PostAsync<GeminiCountTokenMessageResponse>(promptUrl, request);
        }

        /// <summary>
        /// Send a message to be processed using Google Gemini API.
        ///
        /// The method returns all the tokens count from the message.
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#count_tokens
        /// </summary>
        /// <param name="messages">Messages to be processed as content model</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
        /// <returns>Returns a GeminiMessageResponse with the counted tokens</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<GeminiCountTokenMessageResponse?> CountTokens(List<Content> messages,
            GenerationConfig? generationConfig = null, SafetySetting? safetySetting = null)
        {
            if (!messages.Any()) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

            return await _apiRequester.PostAsync<GeminiCountTokenMessageResponse>(promptUrl, request);
        }

        /// <summary>
        /// Send a message to be processed using Google Gemini API.
        ///
        /// The method returns a GeminiMessageResponse with all the response fields from api.
        /// The response is streamed as an observable.
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-only_input
        /// </summary>
        /// <param name="message">Message to be processed as content model</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
        /// <param name="callback"> A callback to be called when the response is received</param>
        /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public Task StreamTextPrompt(
            string message,
            Action<string> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:streamGenerateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message, generationConfig, safetySetting);

            return _apiRequester.PostStream(promptUrl, request, callback);
        }

        /// <summary>
        /// Send a message to be processed using Google Gemini API.
        ///
        /// The method returns a GeminiMessageResponse with all the response fields from api.
        /// The response is streamed as an observable.
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-only_input
        /// </summary>
        /// <param name="messages">Messages to be processed as content model</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
        /// <param name="callback"> A callback to be called when the response is received</param>
        /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public Task StreamTextPrompt(
            List<Content> messages,
            Action<string> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            if (!messages.Any()) throw new ArgumentException("Messages cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:streamGenerateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

            return _apiRequester.PostStream(promptUrl, request, callback);
        }

        /// <summary>
        /// Send a message and a image to be processed using Google Gemini API,
        /// the method returns a GeminiMessageResponse with all the response fields from api
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-and-image_input
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="image">Image as array of bytes format</param>
        /// <param name="mimeType">Mime type of the image</param>
        /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<GeminiMessageResponse?> ImagePrompt(string message, byte[] image, ImageMimeType mimeType)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");
            if (image.Length == 0) throw new ArgumentException("Image cannot be empty.");

            var mimeTypeString = GetMimeTypeString(mimeType);
            var promptUrl = $"{_config.ImageBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildImageGeminiRequest(message, Convert.ToBase64String(image), mimeTypeString);

            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }

        /// <summary>
        /// Send a message to be processed using Google Gemini API,
        /// the method returns a GeminiMessageResponse with all the response fields from api
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-and-image_input
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="base64Image">Base64 image to process</param>
        /// <param name="mimeType">Mime type of the image</param>
        /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<GeminiMessageResponse?> ImagePrompt(string message, string base64Image,
            ImageMimeType mimeType)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");
            if (string.IsNullOrEmpty(base64Image)) throw new ArgumentException("Image cannot be empty.");

            var mimeTypeString = GetMimeTypeString(mimeType);
            var promptUrl = $"{_config.ImageBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildImageGeminiRequest(message, base64Image, mimeTypeString);

            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }

        /// <summary>
        /// Get a model from Google Gemini API
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#get_model
        /// </summary>
        /// <param name="modelName">Model name to find</param>
        /// <returns></returns>
        /// <returns>A GeminiModelResponse containing the model information</returns>
        public async Task<GeminiModelResponse?> GetModel(string modelName)
        {
            if (string.IsNullOrEmpty(modelName)) throw new ArgumentException("Model name cannot be empty.");

            var modelUrl = $"{_config.ModelBaseUrl}/{modelName}?key={_config.ApiKey}";
            return await _apiRequester.GetAsync<GeminiModelResponse>(modelUrl);
        }

        /// <summary>
        /// Get all models from Google Gemini API
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#get_model
        /// </summary>
        /// <returns>A List of GeminiModelResponse containing the model information</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<RootGeminiModelResponse> GetModels()
        {
            var modelUrl = $"{_config.ModelBaseUrl}?key={_config.ApiKey}";
            return await _apiRequester.GetAsync<RootGeminiModelResponse>(modelUrl);
        }

        /// <summary>
        /// Build a GeminiMessageRequest object to process image from a string message, base 64 and mimetype
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="base64Image">Base64 image to process</param>
        /// <param name="mimeType">Mime type of the image</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
        /// <returns>A GeminiMessageRequest built to sending for api</returns>
        private static GeminiMessageRequest BuildImageGeminiRequest(
            string message,
            string base64Image,
            string mimeType,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            return new GeminiMessageRequest
            {
                Contents =
                    new List<Content>
                    {
                        new Content
                        {
                            Parts = new List<Part>
                            {
                                new Part
                                {
                                    Text = message,
                                },
                                new Part
                                {
                                    InlineData = new InlineData
                                    {
                                        MimeType = mimeType,
                                        Data = base64Image
                                    }
                                }
                            }
                        }
                    },
                GenerationConfig = generationConfig,
                SafetySetting = safetySetting
            };
        }

        /// <summary>
        /// Build a GeminiMessageRequest object from a string message
        /// </summary>
        /// <param name="messages">Messages to be processed</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
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
        /// <param name="message">Message to be processed</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
        /// <returns>A request containing GeminiMessageRequest</returns>
        private static GeminiMessageRequest BuildGeminiRequest(
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            return new GeminiMessageRequest
            {
                Contents =
                    new List<Content>
                    {
                        new Content
                        {
                            Parts =
                                new List<Part>
                                {
                                    new Part
                                    {
                                        Text = message
                                    }
                                }
                        }
                    },
                GenerationConfig = generationConfig,
                SafetySetting = safetySetting
            };
        }

        /// <summary>
        /// Build a GeminiMessageRequest object from a list of string messages
        /// </summary>
        /// <param name="messages">Messages to be processed</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySetting">A optional safety setting</param>
        /// <returns></returns>
        private static GeminiMessageRequest BuildGeminiRequest(
            IEnumerable<string> messages,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null)
        {
            var content = new Content
            {
                Parts = messages.Select(message => new Part { Text = message }).ToList()
            };

            return new GeminiMessageRequest
            {
                Contents = new List<Content> { content },
                GenerationConfig = generationConfig,
                SafetySetting = safetySetting
            };
        }

        /// <summary>
        /// Get the mime type string from the enum
        /// </summary>
        /// <param name="mimeType">Mime-type as enum</param>
        /// <returns>Return the mime type as string</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static string GetMimeTypeString(ImageMimeType mimeType)
        {
            return mimeType switch
            {
                ImageMimeType.Jpeg => "image/jpeg",
                ImageMimeType.Png => "image/png",
                ImageMimeType.Jpg => "image/jpg",
                ImageMimeType.Heic => "image/heic",
                ImageMimeType.Heif => "image/heif",
                ImageMimeType.Webp => "image/webp",
                _ => throw new ArgumentOutOfRangeException(nameof(mimeType), mimeType, "Invalid image mime type.")
            };
        }
    }
}