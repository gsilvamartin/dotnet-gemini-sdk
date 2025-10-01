using System;
using System.Threading.Tasks;

namespace DotnetGeminiSDK.Services
{
    /// <summary>
    /// Interface for retry service
    /// </summary>
    public interface IRetryService
    {
        Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxAttempts = 3, int delayMs = 1000);
    }

    /// <summary>
    /// Default retry service with exponential backoff
    /// </summary>
    public class RetryService : IRetryService
    {
        private readonly ILoggingService _loggingService;

        public RetryService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxAttempts = 3, int delayMs = 1000)
        {
            Exception? lastException = null;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (ShouldRetry(ex, attempt, maxAttempts))
                {
                    lastException = ex;
                    _loggingService.LogWarning($"Attempt {attempt} failed, retrying in {delayMs}ms", ex);
                    
                    if (attempt < maxAttempts)
                    {
                        await Task.Delay(delayMs * attempt); // Exponential backoff
                    }
                }
            }

            throw lastException ?? new Exception("Retry failed with unknown error");
        }

        private static bool ShouldRetry(Exception ex, int attempt, int maxAttempts)
        {
            if (attempt >= maxAttempts) return false;

            // Retry on network errors, timeouts, and rate limits
            return ex is System.Net.Http.HttpRequestException ||
                   ex is TaskCanceledException ||
                   ex.Message.ToLowerInvariant().Contains("timeout") ||
                   ex.Message.ToLowerInvariant().Contains("rate limit") ||
                   ex.Message.ToLowerInvariant().Contains("quota");
        }
    }
}
