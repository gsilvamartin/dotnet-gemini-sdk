using System.Text;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Requester.Interfaces;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Requester
{
    public class ApiRequester : IApiRequester
    {
        private readonly HttpClient _httpClient;

        public ApiRequester()
        {
            _httpClient = new HttpClient();
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<T>(response);
        }

        public async Task<T> PostAsync<T>(string url, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            return await HandleResponse<T>(response);
        }

        public async Task<T> PutAsync<T>(string url, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            return await HandleResponse<T>(response);
        }

        public async Task<T> DeleteAsync<T>(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            return await HandleResponse<T>(response);
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request error: {response.StatusCode} - {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}