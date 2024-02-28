using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotnetGeminiSDK.Requester.Interfaces;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Requester
{
    /// <summary>
    /// Generic class to make API Requests
    /// </summary>
    public class ApiRequester : IApiRequester
    {
        private readonly HttpClient _httpClient;

        public ApiRequester()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Get request to the API
        /// </summary>
        /// <param name="url">Url to be requested</param>
        /// <typeparam name="T">Return type of method</typeparam>
        /// <returns>The result of deserialized response</returns>
        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Post request to the API
        /// </summary>
        /// <param name="url">Url to be requested</param>
        /// <param name="data">Data containing body to send</param>
        /// <typeparam name="T">Return type of method</typeparam>
        /// <returns>The result of deserialized response</returns>
        public async Task<T> PostAsync<T>(string url, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Post request to the API with a stream response to be observed.
        /// </summary>
        /// <param name="url">Url to be requested</param>
        /// <param name="data">Data containing body to send</param>
        /// <param name="callback"> A callback to be called when the response is received</param>
        public async Task PostStream(string url, object data, Action<string> callback)
        {
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
                throw new Exception("Unhandled error from API " + response.Content.ReadAsStreamAsync().Result);
            }
        }

        /// <summary>
        /// Put request to the API
        /// </summary>
        /// <param name="url">Url to be requested</param>
        /// <param name="data">Data containing body to send</param>
        /// <typeparam name="T">Return type of method</typeparam>
        /// <returns>The result of deserialized response</returns>
        public async Task<T> PutAsync<T>(string url, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Delete request to the API
        /// </summary>
        /// <param name="url">Url to be requested</param>
        /// <typeparam name="T">Return type of method</typeparam>
        /// <returns>The result of deserialized response</returns>
        public async Task<T> DeleteAsync<T>(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Handle the response from the API, deserializing the content
        /// </summary>
        /// <param name="response">The response in HttpResponseMessage format</param>
        /// <typeparam name="T">Return type of method</typeparam>
        /// <returns>The result of deserialized response</returns>
        /// <exception cref="HttpRequestException"></exception>
        private static async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(content);
            }

            return JsonConvert.DeserializeObject<T>(content) ??
                   throw new Exception("Cannot deserialize response from API");
        }
    }
}