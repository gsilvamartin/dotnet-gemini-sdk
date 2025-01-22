using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
using DotnetGeminiSDK.Requester;
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

        /// <summary>
        /// Constructor to configure client using GoogleGeminiConfig
        /// </summary>
        /// <param name="config">Google gemini config model</param>
        public GeminiClient(GoogleGeminiConfig config)
        {
            _config = config;
            _apiRequester = new ApiRequester();
        }
        
        /// <summary>
        /// Constructor to configure client using dependency injection
        /// </summary>
        /// <param name="config">Google gemini config model</param>
        /// <param name="apiRequester">Api Requester injected instance</param>
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
        /// <param name="safetySettings">A optional safety setting</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GeminiMessageResponse?> TextPrompt(
            string message,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            try
            {
                if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

                var promptUrl = $"{_config.TextBaseUrl}:generateContent?key={_config.ApiKey}";
                var request = BuildGeminiRequest(message, generationConfig, safetySettings);

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
        /// <param name="safetySettings">A optional safety setting</param>
        /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<GeminiMessageResponse?> TextPrompt(
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            if (!messages.Any()) throw new ArgumentException("Messages cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySettings);

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
        /// <param name="safetySettings">A optional safety setting</param>
        /// <returns>Returns a GeminiMessageResponse with the counted tokens</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<GeminiCountTokenMessageResponse?> CountTokens(string message,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message, generationConfig, safetySettings);

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
        /// <param name="safetySettings">A optional safety setting</param>
        /// <returns>Returns a GeminiMessageResponse with the counted tokens</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<GeminiCountTokenMessageResponse?> CountTokens(List<string> messages,
            GenerationConfig? generationConfig = null, List<SafetySetting>? safetySettings = null)
        {
            if (!messages.Any()) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySettings);

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
        /// <param name="safetySettings">A optional safety setting</param>
        /// <returns>Returns a GeminiMessageResponse with the counted tokens</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<GeminiCountTokenMessageResponse?> CountTokens(List<Content> messages,
            GenerationConfig? generationConfig = null, List<SafetySetting>? safetySettings = null)
        {
            if (!messages.Any()) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySettings);

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
        /// <param name="safetySettings">A optional safety setting</param>
        /// <param name="callback"> A callback to be called when the response is received</param>
        /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public Task StreamTextPrompt(
            string message,
            Action<string> callback,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:streamGenerateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message, generationConfig, safetySettings);

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
        /// <param name="safetySettings">A optional safety setting</param>
        /// <param name="callback"> A callback to be called when the response is received</param>
        /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public Task StreamTextPrompt(
            List<Content> messages,
            Action<string> callback,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            if (!messages.Any()) throw new ArgumentException("Messages cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:streamGenerateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySettings);

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
        public async Task<RootGeminiModelResponse?> GetModels()
        {
            var modelUrl = $"{_config.ModelBaseUrl}?key={_config.ApiKey}";
            return await _apiRequester.GetAsync<RootGeminiModelResponse>(modelUrl);
        }

        /// <summary>
        /// Get the embedding of a message using Google Gemini API
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#embedding
        /// </summary>
        /// <param name="message"></param>
        /// <param name="model"></param>
        /// <returns>Return GeminiRootEmbeddingResponse containing the batch response for API</returns>
        public async Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(string message,
            string model = "models/embedding-001")
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}:embedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiRootEmbeddingResponse>(promptUrl, request);
        }


        /// <summary>
        /// Get the embedding of a message using Google Gemini API
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#embedding
        /// </summary>
        /// <param name="message">Messages to be processed</param>
        /// <param name="model">Model to be used</param>
        /// <returns>Return GeminiRootEmbeddingResponse containing the batch response for API</returns>
        public async Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(List<string> message,
            string model = "models/embedding-001")
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}:embedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiRootEmbeddingResponse>(promptUrl, request);
        }

        /// <summary>
        /// Get the embedding of a message using Google Gemini API
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#embedding
        /// </summary>
        /// <param name="message"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GeminiRootEmbeddingResponse?> EmbeddedContentsPrompt(List<Content> message,
            string model = "models/embedding-001")
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}:embedContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message);
            return await _apiRequester.PostAsync<GeminiRootEmbeddingResponse>(promptUrl, request);
        }

        /// <summary>
        /// Get the batch embedding of a message using Google Gemini API
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#embedding
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="model">Model to be used</param>
        /// <returns>Return GeminiBatchRootEmbeddingResponse containing the batch response for API</returns>
        public async Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(string message,
            string model = "models/embedding-001")
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}/{model}/:batchEmbedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiBatchRootEmbeddingResponse>(promptUrl, request);
        }

        /// <summary>
        /// Get the batch embedding of a message using Google Gemini API
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#embedding
        /// </summary>
        /// <param name="message">Messages to be processed</param>
        /// <param name="model">Model to be used</param>
        /// <returns>Return GeminiBatchRootEmbeddingResponse containing the batch response for API</returns>
        public async Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(List<string> message,
            string model = "models/embedding-001")
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}/{model}/:batchEmbedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiBatchRootEmbeddingResponse>(promptUrl, request);
        }

        /// <summary>
        /// Get the batch embedding of a message using Google Gemini API
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#embedding
        /// </summary>
        /// <param name="message">Messages to be processed</param>
        /// <param name="model">Model to be used</param>
        /// <returns>Return GeminiBatchRootEmbeddingResponse containing the batch response for API</returns>
        public async Task<GeminiBatchRootEmbeddingResponse?> BatchEmbeddedContentsPrompt(List<Content> message,
            string model = "models/embedding-001")
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}/{model}/:batchEmbedContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message);
            return await _apiRequester.PostAsync<GeminiBatchRootEmbeddingResponse>(promptUrl, request);
        }

        /// <summary>
        /// Build a GeminiMessageRequest object to process image from a string message, base 64 and mimetype
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="base64Image">Base64 image to process</param>
        /// <param name="mimeType">Mime type of the image</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySettings">A optional safety setting</param>
        /// <returns>A GeminiMessageRequest built to sending for api</returns>
        private static GeminiMessageRequest BuildImageGeminiRequest(
            string message,
            string base64Image,
            string mimeType,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
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
                SafetySettings = safetySettings
            };
        }

        /// <summary>
        /// Build a GeminiMessageRequest object from a string message
        /// </summary>
        /// <param name="messages">Messages to be processed</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySettings">A optional safety setting</param>
        /// <returns>A request containing GeminiMessageRequest</returns>
        private static GeminiMessageRequest BuildGeminiRequest(
            List<Content> messages,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            return new GeminiMessageRequest
            {
                Contents = messages,
                GenerationConfig = generationConfig,
                SafetySettings = safetySettings
            };
        }

        /// <summary>
        /// Build a GeminiMessageRequest object from a string message
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySettings">A optional safety setting</param>
        /// <returns>A request containing GeminiMessageRequest</returns>
        private static GeminiMessageRequest BuildGeminiRequest(
            string message,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
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
                SafetySettings = safetySettings
            };
        }

        /// <summary>
        /// Build a GeminiMessageRequest object from a list of string messages
        /// </summary>
        /// <param name="messages">Messages to be processed</param>
        /// <param name="generationConfig">A optional generation config</param>
        /// <param name="safetySettings">A optional safety setting</param>
        /// <returns></returns>
        private static GeminiMessageRequest BuildGeminiRequest(
            IEnumerable<string> messages,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            var content = new Content
            {
                Parts = messages.Select(message => new Part { Text = message }).ToList()
            };

            return new GeminiMessageRequest
            {
                Contents = new List<Content> { content },
                GenerationConfig = generationConfig,
                SafetySettings = safetySettings
            };
        }

        /// <summary>
        /// Build a GeminiMessageRequest object using a single message and model
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="model">Model to be used</param>
        /// <returns>A GeminiEmbeddedMessageRequest model</returns>
        private static GeminiEmbeddedMessageRequest BuildEmbeddedGeminiRequest(
            string message,
            string model)
        {
            return new GeminiEmbeddedMessageRequest
            {
                Model = model,
                Content = new Content
                {
                    Parts = new List<Part>
                    {
                        new Part
                        {
                            Text = message
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Build a GeminiEmbeddedMessageRequest object from a list of string messages
        /// </summary>
        /// <param name="messages">List of messages</param>
        /// <param name="model">Model to be used</param>
        /// <returns>A GeminiEmbeddedMessageRequest model</returns>
        private static GeminiEmbeddedMessageRequest BuildEmbeddedGeminiRequest(
            IEnumerable<string> messages,
            string model)
        {
            return new GeminiEmbeddedMessageRequest
            {
                Model = model,
                Content = new Content
                {
                    Parts = messages.Select(message => new Part { Text = message }).ToList()
                }
            };
        }

        /// <summary>
        /// Build a GeminiEmbeddedMultipleRequest object from a list of list string messages
        /// </summary>
        /// <param name="messages">List of list messages</param>
        /// <param name="model">List of models</param>
        /// <returns>A GeminiEmbeddedMultipleRequest model</returns>
        public static GeminiEmbeddedMultipleRequest BuildEmbeddedMultipleGeminiRequest(
            IEnumerable<IEnumerable<string>> messages,
            IEnumerable<string> model)
        {
            return new GeminiEmbeddedMultipleRequest
            {
                Requests = messages.Select((message, index) => new GeminiEmbeddedMessageRequest
                {
                    Model = model.ElementAt(index),
                    Content = new Content
                    {
                        Parts = message.Select(m => new Part { Text = m }).ToList()
                    }
                }).ToList()
            };
        }

        /// <summary>
        /// Build a GeminiEmbeddedMultipleRequest object from a list of string messages
        /// </summary>
        /// <param name="messages">List of the messages</param>
        /// <param name="model">List of the models</param>
        /// <returns>A GeminiEmbeddedMultipleRequest model</returns>
        public static GeminiEmbeddedMultipleRequest BuildEmbeddedMultipleGeminiRequest(IEnumerable<string> messages,
            IEnumerable<string> model)
        {
            return new GeminiEmbeddedMultipleRequest
            {
                Requests = messages.Select((message, index) => new GeminiEmbeddedMessageRequest
                {
                    Model = model.ElementAt(index),
                    Content = new Content
                    {
                        Parts = new List<Part>
                        {
                            new Part
                            {
                                Text = message
                            }
                        }
                    }
                }).ToList()
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