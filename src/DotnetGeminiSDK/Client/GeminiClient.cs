using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
using DotnetGeminiSDK.Requester;
using DotnetGeminiSDK.Requester.Interfaces;
using DotnetGeminiSDK.Services;
using DotnetGeminiSDK.Exceptions;
using Microsoft.Extensions.Logging;
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
        private readonly ILoggingService _loggingService;
        private readonly IRetryService _retryService;
        private readonly ICacheService _cacheService;
        private readonly IMetricsService _metricsService;
        private readonly IValidationService _validationService;

        /// <summary>
        /// Constructor to configure client using GoogleGeminiConfig
        /// WARNING: This constructor creates a new HttpClient instance which can lead to connection pool starvation.
        /// Use the dependency injection constructor for production applications.
        /// </summary>
        /// <param name="config">Google gemini config model</param>
        [Obsolete("Use the dependency injection constructor instead. This constructor can cause connection pool starvation.")]
        public GeminiClient(GoogleGeminiConfig config)
        {
            _config = config;
            _apiRequester = new ApiRequester(); // Uses legacy constructor
            _loggingService = new ConsoleLoggingService();
            _retryService = new RetryService(_loggingService);
            _cacheService = new MemoryCacheService(_loggingService);
            _metricsService = new ConsoleMetricsService(_loggingService);
            _validationService = new ValidationService(_loggingService);
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
            _loggingService = new ConsoleLoggingService();
            _retryService = new RetryService(_loggingService);
            _cacheService = new MemoryCacheService(_loggingService);
            _metricsService = new ConsoleMetricsService(_loggingService);
            _validationService = new ValidationService(_loggingService);
        }
        
        /// <summary>
        /// Constructor to configure client using dependency injection with all services
        /// </summary>
        /// <param name="config">Google gemini config model</param>
        /// <param name="apiRequester">Api Requester injected instance</param>
        /// <param name="loggingService">Logging service</param>
        /// <param name="retryService">Retry service</param>
        public GeminiClient(GoogleGeminiConfig config, IApiRequester apiRequester, ILoggingService loggingService, IRetryService retryService)
        {
            _config = config;
            _apiRequester = apiRequester;
            _loggingService = loggingService;
            _retryService = retryService;
            _cacheService = new MemoryCacheService(_loggingService);
            _metricsService = new ConsoleMetricsService(_loggingService);
            _validationService = new ValidationService(_loggingService);
        }
        
        /// <summary>
        /// Constructor to configure client using dependency injection with all services
        /// </summary>
        /// <param name="config">Google gemini config model</param>
        /// <param name="apiRequester">Api Requester injected instance</param>
        /// <param name="loggingService">Logging service</param>
        /// <param name="retryService">Retry service</param>
        /// <param name="cacheService">Cache service</param>
        /// <param name="metricsService">Metrics service</param>
        /// <param name="validationService">Validation service</param>
        public GeminiClient(
            GoogleGeminiConfig config, 
            IApiRequester apiRequester, 
            ILoggingService loggingService, 
            IRetryService retryService,
            ICacheService cacheService,
            IMetricsService metricsService,
            IValidationService validationService)
        {
            _config = config;
            _apiRequester = apiRequester;
            _loggingService = loggingService;
            _retryService = retryService;
            _cacheService = cacheService;
            _metricsService = metricsService;
            _validationService = validationService;
        }

        /// <summary>
        /// Constructor to configure client using IHttpClientFactory (recommended for production)
        /// </summary>
        /// <param name="config">Google gemini config model</param>
        /// <param name="httpClientFactory">HTTP client factory for proper connection management</param>
        /// <param name="logger">Logger instance</param>
        public GeminiClient(
            GoogleGeminiConfig config,
            IHttpClientFactory httpClientFactory,
            ILogger<GeminiClient>? logger = null)
        {
            _config = config;
            
            // Create a logger for ApiRequester from the provided logger
            ILogger<ApiRequester>? apiRequesterLogger = null;
            if (logger != null)
            {
                // Use the same logger factory that created the GeminiClient logger
                var loggerFactory = logger.GetType().GetProperty("Factory")?.GetValue(logger) as ILoggerFactory;
                if (loggerFactory != null)
                {
                    apiRequesterLogger = loggerFactory.CreateLogger<ApiRequester>();
                }
            }
            
            _apiRequester = new ApiRequester(httpClientFactory, apiRequesterLogger);
            _loggingService = new ConsoleLoggingService();
            _retryService = new RetryService(_loggingService);
            _cacheService = new MemoryCacheService(_loggingService);
            _metricsService = new ConsoleMetricsService(_loggingService);
            _validationService = new ValidationService(_loggingService);
        }

        /// <summary>
        /// Constructor to configure client using IHttpClientFactory with ILoggerFactory (recommended for production)
        /// </summary>
        /// <param name="config">Google gemini config model</param>
        /// <param name="httpClientFactory">HTTP client factory for proper connection management</param>
        /// <param name="loggerFactory">Logger factory for creating loggers</param>
        public GeminiClient(
            GoogleGeminiConfig config,
            IHttpClientFactory httpClientFactory,
            ILoggerFactory loggerFactory)
        {
            _config = config;
            
            // Create loggers using the provided factory
            var geminiLogger = loggerFactory.CreateLogger<GeminiClient>();
            var apiRequesterLogger = loggerFactory.CreateLogger<ApiRequester>();
            
            _apiRequester = new ApiRequester(httpClientFactory, apiRequesterLogger);
            _loggingService = new ConsoleLoggingService();
            _retryService = new RetryService(_loggingService);
            _cacheService = new MemoryCacheService(_loggingService);
            _metricsService = new ConsoleMetricsService(_loggingService);
            _validationService = new ValidationService(_loggingService);
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
        /// <param name="systemInstruction">A optional system instruction</param>
        /// <param name="tools">A optional tools for function calling</param>
        /// <param name="toolConfig">A optional tool configuration</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GeminiMessageResponse?> TextPrompt(
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

                var promptUrl = $"{_config.TextBaseUrl}:generateContent?key={_config.ApiKey}";
                var request = BuildGeminiRequest(message, generationConfig, safetySetting, systemInstruction, tools, toolConfig);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Sending text prompt", new { message, model = _config.DefaultTextModel });
                }

                var response = _config.EnableRetry 
                    ? await _retryService.ExecuteWithRetryAsync(() => _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken), _config.MaxRetryAttempts, _config.RetryDelayMs)
                    : await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Received text prompt response", new { responseCount = response?.Candidates?.Count });
                }

                return response;
            }
            catch (Exception e)
            {
                if (_config.EnableLogging)
                {
                    _loggingService.LogError("Error in text prompt", e, new { message });
                }
                throw new GeminiException("Unexpected error occurred in text prompt.", e);
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
            SafetySetting? safetySetting = null,
            CancellationToken cancellationToken = default)
        {
            if (!messages.Any()) throw new ArgumentException("Messages cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken);
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
            SafetySetting? safetySetting = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message, generationConfig, safetySetting);

            return await _apiRequester.PostAsync<GeminiCountTokenMessageResponse>(promptUrl, request, cancellationToken);
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
            GenerationConfig? generationConfig = null, SafetySetting? safetySetting = null,
            CancellationToken cancellationToken = default)
        {
            if (!messages.Any()) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

            return await _apiRequester.PostAsync<GeminiCountTokenMessageResponse>(promptUrl, request, cancellationToken);
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
            GenerationConfig? generationConfig = null, SafetySetting? safetySetting = null,
            CancellationToken cancellationToken = default)
        {
            if (!messages.Any()) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:countTokens?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

            return await _apiRequester.PostAsync<GeminiCountTokenMessageResponse>(promptUrl, request, cancellationToken);
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
        /// <param name="generationConfig">An optional generation config</param>
        /// <param name="safetySetting">An optional safety setting</param>
        /// <param name="callback">A callback to be called when the response is received</param>
        /// <param name="useSSE">Whether to use server-side events.</param>
        /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public Task StreamTextPrompt(
            string message,
            Action<string?> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            bool useSSE = false,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:streamGenerateContent?key={_config.ApiKey}{(useSSE ? "&alt=sse" : string.Empty)}";
            var request = BuildGeminiRequest(message, generationConfig, safetySetting);

            return _apiRequester.PostStream(promptUrl, request, callback, cancellationToken);
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
        /// <param name="useSSE">Whether to use server-side events.</param>
        /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public Task StreamTextPrompt(
            List<Content> messages,
            Action<string?> callback,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            bool useSSE = false,
            CancellationToken cancellationToken = default)
        {
            if (!messages.Any()) throw new ArgumentException("Messages cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:streamGenerateContent?key={_config.ApiKey}{(useSSE ? "&alt=sse" : string.Empty)}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

            return _apiRequester.PostStream(promptUrl, request, callback, cancellationToken);
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
        public async Task<GeminiMessageResponse?> ImagePrompt(string message, byte[] image, ImageMimeType mimeType,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");
            if (image.Length == 0) throw new ArgumentException("Image cannot be empty.");

            var mimeTypeString = GetMimeTypeString(mimeType);
            var promptUrl = $"{_config.ImageBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildImageGeminiRequest(message, Convert.ToBase64String(image), mimeTypeString);

            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken);
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
            ImageMimeType mimeType,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");
            if (string.IsNullOrEmpty(base64Image)) throw new ArgumentException("Image cannot be empty.");

            var mimeTypeString = GetMimeTypeString(mimeType);
            var promptUrl = $"{_config.ImageBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildImageGeminiRequest(message, base64Image, mimeTypeString);

            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken);
        }

        /// <summary>
        /// Get a model from Google Gemini API
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#get_model
        /// </summary>
        /// <param name="modelName">Model name to find</param>
        /// <returns></returns>
        /// <returns>A GeminiModelResponse containing the model information</returns>
        public async Task<GeminiModelResponse?> GetModel(string modelName,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(modelName)) throw new ArgumentException("Model name cannot be empty.");

            var modelUrl = $"{_config.ModelBaseUrl}/{modelName}?key={_config.ApiKey}";
            return await _apiRequester.GetAsync<GeminiModelResponse>(modelUrl, cancellationToken);
        }

        /// <summary>
        /// Get all models from Google Gemini API
        ///
        /// REF: https://ai.google.dev/tutorials/rest_quickstart#get_model
        /// </summary>
        /// <returns>A List of GeminiModelResponse containing the model information</returns>
        public async Task<RootGeminiModelResponse?> GetModels(
            CancellationToken cancellationToken = default)
        {
            var modelUrl = $"{_config.ModelBaseUrl}?key={_config.ApiKey}";
            return await _apiRequester.GetAsync<RootGeminiModelResponse>(modelUrl, cancellationToken);
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
            string model = "models/embedding-001",
            CancellationToken cancellationToken = default)
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}:embedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiRootEmbeddingResponse>(promptUrl, request, cancellationToken);
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
            string model = "models/embedding-001",
            CancellationToken cancellationToken = default)
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}:embedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiRootEmbeddingResponse>(promptUrl, request, cancellationToken);
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
            string model = "models/embedding-001",
            CancellationToken cancellationToken = default)
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}:embedContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message);
            return await _apiRequester.PostAsync<GeminiRootEmbeddingResponse>(promptUrl, request, cancellationToken);
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
            string model = "models/embedding-001",
            CancellationToken cancellationToken = default)
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}/{model}/:batchEmbedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiBatchRootEmbeddingResponse>(promptUrl, request, cancellationToken);
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
            string model = "models/embedding-001",
            CancellationToken cancellationToken = default)
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}/{model}/:batchEmbedContent?key={_config.ApiKey}";
            var request = BuildEmbeddedGeminiRequest(message, model);
            return await _apiRequester.PostAsync<GeminiBatchRootEmbeddingResponse>(promptUrl, request, cancellationToken);
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
            string model = "models/embedding-001",
            CancellationToken cancellationToken = default)
        {
            var promptUrl = $"{_config.EmbeddingBaseUrl}/{model}/:batchEmbedContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message);
            return await _apiRequester.PostAsync<GeminiBatchRootEmbeddingResponse>(promptUrl, request, cancellationToken);
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
        /// <param name="systemInstruction">A optional system instruction</param>
        /// <param name="tools">A optional tools for function calling</param>
        /// <param name="toolConfig">A optional tool configuration</param>
        /// <returns>A request containing GeminiMessageRequest</returns>
        private static GeminiMessageRequest BuildGeminiRequest(
            string message,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            List<Tool>? tools = null,
            ToolConfig? toolConfig = null)
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
                SafetySetting = safetySetting,
                SystemInstruction = systemInstruction,
                Tools = tools,
                ToolConfig = toolConfig
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
        /// Send a message with function calling support
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="functions">List of function declarations</param>
        /// <param name="generationConfig">Optional generation config</param>
        /// <param name="safetySetting">Optional safety setting</param>
        /// <param name="systemInstruction">Optional system instruction</param>
        /// <returns>Response with potential function calls</returns>
        public async Task<GeminiMessageResponse?> TextPromptWithFunctions(
            string message,
            List<FunctionDeclaration> functions,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            CancellationToken cancellationToken = default)
        {
            var tools = new List<Tool>
            {
                new Tool { FunctionDeclarations = functions }
            };
            
            var toolConfig = new ToolConfig
            {
                FunctionCallingConfig = new FunctionCallingConfig
                {
                    Mode = "AUTO"
                }
            };
            
            return await TextPrompt(message, generationConfig, safetySetting, systemInstruction, tools, toolConfig, cancellationToken);
        }
        
        /// <summary>
        /// Send a message with system instruction
        /// </summary>
        /// <param name="message">Message to be processed</param>
        /// <param name="systemInstruction">System instruction to define behavior</param>
        /// <param name="generationConfig">Optional generation config</param>
        /// <param name="safetySetting">Optional safety setting</param>
        /// <returns>Response from the model</returns>
        public async Task<GeminiMessageResponse?> TextPromptWithSystemInstruction(
            string message,
            string systemInstruction,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            CancellationToken cancellationToken = default)
        {
            var systemInstructionObj = new SystemInstruction
            {
                Parts = new List<Part>
                {
                    new Part { Text = systemInstruction }
                }
            };
            
            return await TextPrompt(message, generationConfig, safetySetting, systemInstructionObj, null, null, cancellationToken);
        }
        
        /// <summary>
        /// Get available models
        /// </summary>
        /// <returns>List of available model names</returns>
        public string[] GetAvailableModels()
        {
            return GeminiModels.TextModels;
        }
        
        /// <summary>
        /// Get available multimodal models
        /// </summary>
        /// <returns>List of available multimodal model names</returns>
        public string[] GetAvailableMultimodalModels()
        {
            return GeminiModels.MultimodalModels;
        }

        /// <summary>
        /// Send a video prompt to the Gemini API
        /// </summary>
        /// <param name="message">Text message to accompany the video</param>
        /// <param name="videoBytes">Video file as byte array</param>
        /// <param name="mimeType">Video MIME type</param>
        /// <param name="generationConfig">Optional generation config</param>
        /// <param name="safetySetting">Optional safety setting</param>
        /// <param name="systemInstruction">Optional system instruction</param>
        /// <returns>Response from the model</returns>
        public async Task<GeminiMessageResponse?> VideoPrompt(
            string message,
            byte[] videoBytes,
            MediaMimeType mimeType,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");
                if (videoBytes.Length == 0) throw new ArgumentException("Video cannot be empty.");
                if (!mimeType.IsVideo()) throw new ArgumentException("MIME type must be a video type.");

                var startTime = DateTime.UtcNow;
                _metricsService.IncrementCounter("gemini.video_prompt.requests");

                var promptUrl = $"{_config.ImageBaseUrl}:generateContent?key={_config.ApiKey}";
                var request = BuildMultimodalGeminiRequest(message, videoBytes, mimeType, generationConfig, safetySetting, systemInstruction);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Sending video prompt", new { message, videoSize = videoBytes.Length, mimeType = mimeType.GetMimeTypeString() });
                }

                var response = _config.EnableRetry 
                    ? await _retryService.ExecuteWithRetryAsync(() => _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken), _config.MaxRetryAttempts, _config.RetryDelayMs)
                    : await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken);

                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                _metricsService.RecordTiming("gemini.video_prompt.duration", duration);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Received video prompt response", new { responseCount = response?.Candidates?.Count, duration });
                }

                return response;
            }
            catch (Exception e)
            {
                _metricsService.RecordError("gemini.video_prompt.error", e);
                if (_config.EnableLogging)
                {
                    _loggingService.LogError("Error in video prompt", e, new { message, videoSize = videoBytes.Length });
                }
                throw new GeminiException("Unexpected error occurred in video prompt.", e);
            }
        }

        /// <summary>
        /// Send an audio prompt to the Gemini API
        /// </summary>
        /// <param name="message">Text message to accompany the audio</param>
        /// <param name="audioBytes">Audio file as byte array</param>
        /// <param name="mimeType">Audio MIME type</param>
        /// <param name="generationConfig">Optional generation config</param>
        /// <param name="safetySetting">Optional safety setting</param>
        /// <param name="systemInstruction">Optional system instruction</param>
        /// <returns>Response from the model</returns>
        public async Task<GeminiMessageResponse?> AudioPrompt(
            string message,
            byte[] audioBytes,
            MediaMimeType mimeType,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");
                if (audioBytes.Length == 0) throw new ArgumentException("Audio cannot be empty.");
                if (!mimeType.IsAudio()) throw new ArgumentException("MIME type must be an audio type.");

                var startTime = DateTime.UtcNow;
                _metricsService.IncrementCounter("gemini.audio_prompt.requests");

                var promptUrl = $"{_config.ImageBaseUrl}:generateContent?key={_config.ApiKey}";
                var request = BuildMultimodalGeminiRequest(message, audioBytes, mimeType, generationConfig, safetySetting, systemInstruction);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Sending audio prompt", new { message, audioSize = audioBytes.Length, mimeType = mimeType.GetMimeTypeString() });
                }

                var response = _config.EnableRetry 
                    ? await _retryService.ExecuteWithRetryAsync(() => _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken), _config.MaxRetryAttempts, _config.RetryDelayMs)
                    : await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken);

                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                _metricsService.RecordTiming("gemini.audio_prompt.duration", duration);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Received audio prompt response", new { responseCount = response?.Candidates?.Count, duration });
                }

                return response;
            }
            catch (Exception e)
            {
                _metricsService.RecordError("gemini.audio_prompt.error", e);
                if (_config.EnableLogging)
                {
                    _loggingService.LogError("Error in audio prompt", e, new { message, audioSize = audioBytes.Length });
                }
                throw new GeminiException("Unexpected error occurred in audio prompt.", e);
            }
        }

        /// <summary>
        /// Send a multimodal prompt with multiple media types
        /// </summary>
        /// <param name="message">Text message</param>
        /// <param name="mediaFiles">List of media files</param>
        /// <param name="generationConfig">Optional generation config</param>
        /// <param name="safetySetting">Optional safety setting</param>
        /// <param name="systemInstruction">Optional system instruction</param>
        /// <returns>Response from the model</returns>
        public async Task<GeminiMessageResponse?> MultimodalPrompt(
            string message,
            List<MediaFile> mediaFiles,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");
                if (mediaFiles == null || !mediaFiles.Any()) throw new ArgumentException("Media files cannot be empty.");

                var startTime = DateTime.UtcNow;
                _metricsService.IncrementCounter("gemini.multimodal_prompt.requests");

                var promptUrl = $"{_config.ImageBaseUrl}:generateContent?key={_config.ApiKey}";
                var request = BuildMultimodalGeminiRequest(message, mediaFiles, generationConfig, safetySetting, systemInstruction);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Sending multimodal prompt", new { message, mediaCount = mediaFiles.Count });
                }

                var response = _config.EnableRetry 
                    ? await _retryService.ExecuteWithRetryAsync(() => _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken), _config.MaxRetryAttempts, _config.RetryDelayMs)
                    : await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request, cancellationToken);

                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                _metricsService.RecordTiming("gemini.multimodal_prompt.duration", duration);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Received multimodal prompt response", new { responseCount = response?.Candidates?.Count, duration });
                }

                return response;
            }
            catch (Exception e)
            {
                _metricsService.RecordError("gemini.multimodal_prompt.error", e);
                if (_config.EnableLogging)
                {
                    _loggingService.LogError("Error in multimodal prompt", e, new { message, mediaCount = mediaFiles?.Count });
                }
                throw new GeminiException("Unexpected error occurred in multimodal prompt.", e);
            }
        }

        /// <summary>
        /// Upload a file to Gemini API for processing
        /// </summary>
        /// <param name="fileBytes">File content as byte array</param>
        /// <param name="mimeType">File MIME type</param>
        /// <param name="fileName">Original file name</param>
        /// <returns>File upload response with URI</returns>
        public async Task<FileUploadResponse?> UploadFile(
            byte[] fileBytes,
            MediaMimeType mimeType,
            string fileName,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (fileBytes.Length == 0) throw new ArgumentException("File cannot be empty.");
                if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("File name cannot be empty.");

                var startTime = DateTime.UtcNow;
                _metricsService.IncrementCounter("gemini.file_upload.requests");

                var uploadUrl = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={_config.ApiKey}";
                
                // Note: This is a simplified implementation. In a real implementation,
                // you would need to handle multipart form data upload properly.
                var request = new { file = new { name = fileName, mimeType = mimeType.GetMimeTypeString() } };

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Uploading file", new { fileName, fileSize = fileBytes.Length, mimeType = mimeType.GetMimeTypeString() });
                }

                var response = _config.EnableRetry 
                    ? await _retryService.ExecuteWithRetryAsync(() => _apiRequester.PostAsync<FileUploadResponse>(uploadUrl, request, cancellationToken), _config.MaxRetryAttempts, _config.RetryDelayMs)
                    : await _apiRequester.PostAsync<FileUploadResponse>(uploadUrl, request, cancellationToken);

                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                _metricsService.RecordTiming("gemini.file_upload.duration", duration);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("File uploaded successfully", new { fileName, fileUri = response?.File?.Uri, duration });
                }

                return response;
            }
            catch (Exception e)
            {
                _metricsService.RecordError("gemini.file_upload.error", e);
                if (_config.EnableLogging)
                {
                    _loggingService.LogError("Error uploading file", e, new { fileName, fileSize = fileBytes.Length });
                }
                throw new GeminiException("Unexpected error occurred while uploading file.", e);
            }
        }

        /// <summary>
        /// Get file information from Gemini API
        /// </summary>
        /// <param name="fileUri">File URI from upload response</param>
        /// <returns>File information</returns>
        public async Task<FileInfoResponse?> GetFileInfo(string fileUri,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUri)) throw new ArgumentException("File URI cannot be empty.");

                var startTime = DateTime.UtcNow;
                _metricsService.IncrementCounter("gemini.file_info.requests");

                var infoUrl = $"{fileUri}?key={_config.ApiKey}";

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Getting file info", new { fileUri });
                }

                var response = _config.EnableRetry 
                    ? await _retryService.ExecuteWithRetryAsync(() => _apiRequester.GetAsync<FileInfoResponse>(infoUrl, cancellationToken), _config.MaxRetryAttempts, _config.RetryDelayMs)
                    : await _apiRequester.GetAsync<FileInfoResponse>(infoUrl, cancellationToken);

                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                _metricsService.RecordTiming("gemini.file_info.duration", duration);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Retrieved file info", new { fileUri, duration });
                }

                return response;
            }
            catch (Exception e)
            {
                _metricsService.RecordError("gemini.file_info.error", e);
                if (_config.EnableLogging)
                {
                    _loggingService.LogError("Error getting file info", e, new { fileUri });
                }
                throw new GeminiException("Unexpected error occurred while getting file info.", e);
            }
        }

        /// <summary>
        /// Delete a file from Gemini API
        /// </summary>
        /// <param name="fileUri">File URI to delete</param>
        /// <returns>True if deletion was successful</returns>
        public async Task<bool> DeleteFile(string fileUri,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUri)) throw new ArgumentException("File URI cannot be empty.");

                var startTime = DateTime.UtcNow;
                _metricsService.IncrementCounter("gemini.file_delete.requests");

                var deleteUrl = $"{fileUri}?key={_config.ApiKey}";

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("Deleting file", new { fileUri });
                }

                await _apiRequester.DeleteAsync<object>(deleteUrl, cancellationToken);

                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                _metricsService.RecordTiming("gemini.file_delete.duration", duration);

                if (_config.EnableLogging)
                {
                    _loggingService.LogInfo("File deleted successfully", new { fileUri, duration });
                }

                return true;
            }
            catch (Exception e)
            {
                _metricsService.RecordError("gemini.file_delete.error", e);
                if (_config.EnableLogging)
                {
                    _loggingService.LogError("Error deleting file", e, new { fileUri });
                }
                throw new GeminiException("Unexpected error occurred while deleting file.", e);
            }
        }

        /// <summary>
        /// Build a multimodal Gemini request with media content
        /// </summary>
        private static GeminiMessageRequest BuildMultimodalGeminiRequest(
            string message,
            byte[] mediaBytes,
            MediaMimeType mimeType,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null)
        {
            var parts = new List<Part>
            {
                new Part { Text = message },
                new Part
                {
                    InlineData = new InlineData
                    {
                        MimeType = mimeType.GetMimeTypeString(),
                        Data = Convert.ToBase64String(mediaBytes)
                    }
                }
            };

            return new GeminiMessageRequest
            {
                Contents = new List<Content>
                {
                    new Content { Parts = parts }
                },
                GenerationConfig = generationConfig,
                SafetySetting = safetySetting,
                SystemInstruction = systemInstruction
            };
        }

        /// <summary>
        /// Build a multimodal Gemini request with multiple media files
        /// </summary>
        private static GeminiMessageRequest BuildMultimodalGeminiRequest(
            string message,
            List<MediaFile> mediaFiles,
            GenerationConfig? generationConfig = null,
            SafetySetting? safetySetting = null,
            SystemInstruction? systemInstruction = null)
        {
            var parts = new List<Part> { new Part { Text = message } };

            foreach (var mediaFile in mediaFiles)
            {
                parts.Add(new Part
                {
                    InlineData = new InlineData
                    {
                        MimeType = mediaFile.MimeTypeString,
                        Data = Convert.ToBase64String(mediaFile.Content)
                    }
                });
            }

            return new GeminiMessageRequest
            {
                Contents = new List<Content>
                {
                    new Content { Parts = parts }
                },
                GenerationConfig = generationConfig,
                SafetySetting = safetySetting,
                SystemInstruction = systemInstruction
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