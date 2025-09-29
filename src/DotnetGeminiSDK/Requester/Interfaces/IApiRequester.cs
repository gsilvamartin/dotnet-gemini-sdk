using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetGeminiSDK.Requester.Interfaces
{
    /// <summary>
    /// Interface for making requests to the API
    /// </summary>
    public interface IApiRequester
    {
        Task<T> GetAsync<T>(string url, CancellationToken cancellationToken = default);
        Task<T> PostAsync<T>(string url, object data, CancellationToken cancellationToken = default);
        Task PostStream(string url, object data, Action<string?> callback, CancellationToken cancellationToken = default);
        Task<T> PutAsync<T>(string url, object data, CancellationToken cancellationToken = default);
        Task<T> DeleteAsync<T>(string url, CancellationToken cancellationToken = default);
    }
}