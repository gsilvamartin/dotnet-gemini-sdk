namespace DotnetGeminiSDK.Requester.Interfaces;

/// <summary>
/// Interface for making requests to the API
/// </summary>
public interface IApiRequester
{
    Task<T?> GetAsync<T>(string url);
    Task<T?> PostAsync<T>(string url, object data);
    IObservable<T?> PostStream<T>(string url, object data);
    Task<T?> PutAsync<T>(string url, object data);
    Task<T?> DeleteAsync<T>(string url);
}