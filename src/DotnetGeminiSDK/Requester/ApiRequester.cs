using System.Reactive.Linq;
using System.Text;
using DotnetGeminiSDK.Requester.Interfaces;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Requester;

/// <summary>
/// Generic class to make API Requests
/// </summary>
public class ApiRequester : IApiRequester
{
    private readonly HttpClient _httpClient = new();

    /// <summary>
    /// Get request to the API
    /// </summary>
    /// <param name="url">Url to be requested</param>
    /// <typeparam name="T">Return type of method</typeparam>
    /// <returns>The result of deserialized response</returns>
    public async Task<T?> GetAsync<T>(string url)
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
    public async Task<T?> PostAsync<T>(string url, object data)
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
    /// <typeparam name="T">Return type of method</typeparam>
    /// <returns>Observable post stream result</returns>
    public IObservable<T?> PostStream<T>(string url, object data)
    {
        return Observable.Create<T?>(async observer =>
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(responseStream);

                while (!reader.EndOfStream)
                {
                    observer.OnNext(await HandleResponse<T>(reader));
                }

                observer.OnCompleted();
            }
            else
            {
                observer.OnError(new Exception($"Request error: {response.Content.ReadAsStringAsync().Result}"));
            }
        });
    }

    /// <summary>
    /// Put request to the API
    /// </summary>
    /// <param name="url">Url to be requested</param>
    /// <param name="data">Data containing body to send</param>
    /// <typeparam name="T">Return type of method</typeparam>
    /// <returns>The result of deserialized response</returns>
    public async Task<T?> PutAsync<T>(string url, object data)
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
    public async Task<T?> DeleteAsync<T>(string url)
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
    private static async Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(content);
        }

        return JsonConvert.DeserializeObject<T>(content);
    }

    /// <summary>
    /// Handle the response from the API, deserializing the content from a stream
    /// </summary>
    /// <param name="streamReader">The stream reader</param>
    /// <typeparam name="T">Return type of method</typeparam>
    /// <returns></returns>
    private static async Task<T?> HandleResponse<T>(TextReader streamReader)
    {
        var content = await streamReader.ReadLineAsync();

        return JsonConvert.DeserializeObject<T>(content);
    }
}