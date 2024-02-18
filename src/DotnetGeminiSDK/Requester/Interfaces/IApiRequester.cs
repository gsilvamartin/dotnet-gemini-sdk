using System.Threading.Tasks;

namespace DotnetGeminiSDK.Requester.Interfaces
{
    public interface IApiRequester
    {
        Task<T?> GetAsync<T>(string url);
        Task<T?> PostAsync<T>(string url, object data);
        Task<T?> PutAsync<T>(string url, object data);
        Task<T?> DeleteAsync<T>(string url);
    }
}