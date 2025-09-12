using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotnetGeminiSDK.Requester.Interfaces;
using DotnetGeminiSDK.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;

namespace DotnetGeminiSDK.Requester
{
    /// <summary>
    /// Generic class to make API Requests with resilience and proper connection management
    /// </summary>
    public class ApiRequester : IApiRequester
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiRequester>? _logger;
        private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

        public ApiRequester(IHttpClientFactory httpClientFactory, ILogger<ApiRequester>? logger = null)
        {
            _httpClient = httpClientFactory.CreateClient("GeminiClient");
            _logger = logger;
            
            // Configure retry policy with exponential backoff
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .Or<OperationCanceledException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && IsRetryableStatusCode(r.StatusCode))
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        _logger?.LogWarning("Retry {RetryCount} after {Delay}ms. Reason: {Reason}", 
                            retryCount, timespan.TotalMilliseconds, outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                    });
        }

        /// <summary>
        /// Legacy constructor for backward compatibility - creates HttpClient directly
        /// WARNING: This constructor creates a new HttpClient instance which can lead to connection pool starvation.
        /// Use the IHttpClientFactory constructor for production applications.
        /// </summary>
        [Obsolete("Use ApiRequester(IHttpClientFactory, ILogger<ApiRequester>) constructor instead. This constructor can cause connection pool starvation.")]
        public ApiRequester()
        {
            _httpClient = new HttpClient();
            _logger = null;
            
            // Basic retry policy for legacy constructor
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .Or<OperationCanceledException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && IsRetryableStatusCode(r.StatusCode))
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static bool IsRetryableStatusCode(System.Net.HttpStatusCode statusCode)
        {
            return statusCode == System.Net.HttpStatusCode.RequestTimeout ||
                   (int)statusCode == 429 || // TooManyRequests
                   statusCode == System.Net.HttpStatusCode.InternalServerError ||
                   statusCode == System.Net.HttpStatusCode.BadGateway ||
                   statusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                   statusCode == System.Net.HttpStatusCode.GatewayTimeout;
        }

        /// <summary>
        /// Get request to the API with retry policy
        /// </summary>
        /// <param name="url">Url to be requested</param>
        /// <typeparam name="T">Return type of method</typeparam>
        /// <returns>The result of deserialized response</returns>
        public async Task<T> GetAsync<T>(string url)
        {
            _logger?.LogDebug("Making GET request to {Url}", url);
            
            var response = await _retryPolicy.ExecuteAsync(async () => await _httpClient.GetAsync(url));
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Post request to the API with retry policy
        /// </summary>
        /// <param name="url">Url to be requested</param>
        /// <param name="data">Data containing body to send</param>
        /// <typeparam name="T">Return type of method</typeparam>
        /// <returns>The result of deserialized response</returns>
        public async Task<T> PostAsync<T>(string url, object data)
        {
            _logger?.LogDebug("Making POST request to {Url}", url);
            
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _retryPolicy.ExecuteAsync(async () => await _httpClient.PostAsync(url, content));
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Post request to the API with a stream response to be observed.
        /// Note: Streaming requests don't use retry policy to avoid breaking the stream.
        /// </summary>
        /// <param name="url">Url to be requested</param>
        /// <param name="data">Data containing body to send</param>
        /// <param name="callback"> A callback to be called when the response is received</param>
        public async Task PostStream(string url, object data, Action<string> callback)
        {
            _logger?.LogDebug("Making streaming POST request to {Url}", url);
            
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                int bytesRead;
                var buffer = new byte[8192];
                using var responseStream = await response.Content.ReadAsStreamAsync();

                while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    var chunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    callback(chunk);
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger?.LogError("Streaming request failed with status {StatusCode}: {Error}", response.StatusCode, errorContent);
                throw new GeminiException($"Streaming request failed: {response.StatusCode} - {errorContent}");
            }
        }

        /// <summary>
        /// Put request to the API with retry policy
        /// </summary>
        /// <param name="url">Url to be requested</param>
        /// <param name="data">Data containing body to send</param>
        /// <typeparam name="T">Return type of method</typeparam>
        /// <returns>The result of deserialized response</returns>
        public async Task<T> PutAsync<T>(string url, object data)
        {
            _logger?.LogDebug("Making PUT request to {Url}", url);
            
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _retryPolicy.ExecuteAsync(async () => await _httpClient.PutAsync(url, content));
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Delete request to the API with retry policy
        /// </summary>
        /// <param name="url">Url to be requested</param>
        /// <typeparam name="T">Return type of method</typeparam>
        /// <returns>The result of deserialized response</returns>
        public async Task<T> DeleteAsync<T>(string url)
        {
            _logger?.LogDebug("Making DELETE request to {Url}", url);
            
            var response = await _retryPolicy.ExecuteAsync(async () => await _httpClient.DeleteAsync(url));
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Handle the response from the API, deserializing the content
        /// </summary>
        /// <param name="response">The response in HttpResponseMessage format</param>
        /// <typeparam name="T">Return type of method</typeparam>
        /// <returns>The result of deserialized response</returns>
        /// <exception cref="GeminiException"></exception>
        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogError("API request failed with status {StatusCode}: {Content}", response.StatusCode, content);
                
                // Map HTTP status codes to specific Gemini exceptions
                throw response.StatusCode switch
                {
                    System.Net.HttpStatusCode.Unauthorized => new GeminiAuthenticationException("Invalid API key or authentication failed"),
                    System.Net.HttpStatusCode.Forbidden => new GeminiAuthenticationException("API key does not have permission for this operation"),
                    _ when (int)response.StatusCode == 429 => new GeminiRateLimitException("Rate limit exceeded"),
                    System.Net.HttpStatusCode.BadRequest => new GeminiBadRequestException($"Bad request: {content}"),
                    System.Net.HttpStatusCode.NotFound => new GeminiModelNotFoundException("Model not found"),
                    System.Net.HttpStatusCode.ServiceUnavailable => new GeminiException("Service temporarily unavailable"),
                    System.Net.HttpStatusCode.RequestTimeout => new GeminiException("Request timeout"),
                    _ => new GeminiException($"API request failed with status {response.StatusCode}: {content}")
                };
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(content) ??
                       throw new GeminiException("Cannot deserialize response from API");
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex, "Failed to deserialize API response: {Content}", content);
                throw new GeminiException($"Failed to deserialize API response: {ex.Message}");
            }
        }
    }
}