using System;
using System.Threading.Tasks;

namespace DotnetGeminiSDK.Services
{
    /// <summary>
    /// Interface for cache service
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Get a value from cache
        /// </summary>
        /// <typeparam name="T">Type of the cached value</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Cached value or default if not found</returns>
        Task<T?> GetAsync<T>(string key) where T : class;
        
        /// <summary>
        /// Set a value in cache
        /// </summary>
        /// <typeparam name="T">Type of the value to cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to cache</param>
        /// <param name="expiration">Expiration time (optional)</param>
        /// <returns>Task representing the operation</returns>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        
        /// <summary>
        /// Remove a value from cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Task representing the operation</returns>
        Task RemoveAsync(string key);
        
        /// <summary>
        /// Clear all cache entries
        /// </summary>
        /// <returns>Task representing the operation</returns>
        Task ClearAsync();
        
        /// <summary>
        /// Check if a key exists in cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>True if key exists</returns>
        Task<bool> ExistsAsync(string key);
    }
    
    /// <summary>
    /// In-memory cache service implementation
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, CacheItem> _cache = new();
        private readonly ILoggingService _loggingService;
        
        /// <summary>
        /// Initialize memory cache service
        /// </summary>
        /// <param name="loggingService">Logging service</param>
        public MemoryCacheService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }
        
        /// <inheritdoc/>
        public Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                if (_cache.TryGetValue(key, out var item) && !item.IsExpired())
                {
                    _loggingService.LogDebug($"Cache hit for key: {key}");
                    return Task.FromResult((T?)item.Value);
                }
                
                _loggingService.LogDebug($"Cache miss for key: {key}");
                return Task.FromResult<T?>(default);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting from cache: {key}", ex);
                return Task.FromResult<T?>(default);
            }
        }
        
        /// <inheritdoc/>
        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var item = new CacheItem
                {
                    Value = value,
                    ExpiresAt = expiration.HasValue ? DateTime.UtcNow.Add(expiration.Value) : null
                };
                
                _cache.AddOrUpdate(key, item, (k, v) => item);
                _loggingService.LogDebug($"Cached value for key: {key}");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error setting cache: {key}", ex);
                return Task.CompletedTask;
            }
        }
        
        /// <inheritdoc/>
        public Task RemoveAsync(string key)
        {
            try
            {
                _cache.TryRemove(key, out _);
                _loggingService.LogDebug($"Removed from cache: {key}");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error removing from cache: {key}", ex);
                return Task.CompletedTask;
            }
        }
        
        /// <inheritdoc/>
        public Task ClearAsync()
        {
            try
            {
                _cache.Clear();
                _loggingService.LogDebug("Cleared all cache entries");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error clearing cache", ex);
                return Task.CompletedTask;
            }
        }
        
        /// <inheritdoc/>
        public Task<bool> ExistsAsync(string key)
        {
            try
            {
                var exists = _cache.TryGetValue(key, out var item) && !item.IsExpired();
                return Task.FromResult(exists);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error checking cache existence: {key}", ex);
                return Task.FromResult(false);
            }
        }
        
        private class CacheItem
        {
            public object? Value { get; set; }
            public DateTime? ExpiresAt { get; set; }
            
            public bool IsExpired()
            {
                return ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
            }
        }
    }
}
