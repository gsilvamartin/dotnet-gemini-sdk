using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Requester;
using DotnetGeminiSDK.Requester.Interfaces;
using DotnetGeminiSDK.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Linq;
using System.Net.Http;

namespace DotnetGeminiSDK
{
    /// <summary>
    /// Extension methods for adding the GeminiClient to the service collection.
    /// </summary>
    public static class GeminiServiceExtensions
    {
        /// <summary>
        /// Adds the GeminiClient to the service collection.
        ///
        /// This is the entry point to the Gemini SDK.
        /// You must call it and provide the required configuration to work with the Gemini API.
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <param name="configure">Configuration from the Gemini</param>
        /// <returns></returns>
        public static IServiceCollection AddGeminiClient(
            this IServiceCollection services,
            Action<GoogleGeminiConfig> configure)
        {
            var config = new GoogleGeminiConfig();
            configure.Invoke(config);

            // Configure HttpClient with proper connection pooling and resilience
            services.AddHttpClient("GeminiClient", client =>
            {
                client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
                client.DefaultRequestHeaders.Add("User-Agent", "DotnetGeminiSDK/2.0.0");
                client.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddSingleton(config);
            services.AddSingleton<IApiRequester, ApiRequester>();
            services.AddSingleton<ILoggingService, ConsoleLoggingService>();
            services.AddSingleton<IRetryService, RetryService>();
            services.AddSingleton<ICacheService, MemoryCacheService>();
            services.AddSingleton<IMetricsService, ConsoleMetricsService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IGeminiClient, GeminiClient>();

            return services;
        }
        
        /// <summary>
        /// Adds the GeminiClient to the service collection with custom services.
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <param name="configure">Configuration from the Gemini</param>
        /// <param name="configureServices">Configure custom services</param>
        /// <returns></returns>
        public static IServiceCollection AddGeminiClient(
            this IServiceCollection services,
            Action<GoogleGeminiConfig> configure,
            Action<IServiceCollection>? configureServices = null)
        {
            var config = new GoogleGeminiConfig();
            configure.Invoke(config);

            // Configure HttpClient with proper connection pooling and resilience
            services.AddHttpClient("GeminiClient", client =>
            {
                client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
                client.DefaultRequestHeaders.Add("User-Agent", "DotnetGeminiSDK/2.0.0");
                client.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddSingleton(config);
            services.AddSingleton<IApiRequester, ApiRequester>();
            
            // Add default services if not already registered
            if (!services.Any(s => s.ServiceType == typeof(ILoggingService)))
            {
                services.AddSingleton<ILoggingService, ConsoleLoggingService>();
            }
            
            if (!services.Any(s => s.ServiceType == typeof(IRetryService)))
            {
                services.AddSingleton<IRetryService, RetryService>();
            }
            
            if (!services.Any(s => s.ServiceType == typeof(ICacheService)))
            {
                services.AddSingleton<ICacheService, MemoryCacheService>();
            }
            
            if (!services.Any(s => s.ServiceType == typeof(IMetricsService)))
            {
                services.AddSingleton<IMetricsService, ConsoleMetricsService>();
            }
            
            if (!services.Any(s => s.ServiceType == typeof(IValidationService)))
            {
                services.AddSingleton<IValidationService, ValidationService>();
            }
            
            // Allow custom service configuration
            configureServices?.Invoke(services);
            
            services.AddSingleton<IGeminiClient>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<GoogleGeminiConfig>();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                
                return new GeminiClient(config, httpClientFactory, loggerFactory);
            });

            return services;
        }

        /// <summary>
        /// Configures retry policy with exponential backoff
        /// </summary>
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return Policy
                .Handle<HttpRequestException>()
                .Or<OperationCanceledException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && IsRetryableStatusCode(r.StatusCode))
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        // Log retry attempts if logging is available
                        Console.WriteLine($"Retry {retryCount} after {timespan.TotalMilliseconds}ms. Reason: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
                    });
        }

        /// <summary>
        /// Configures circuit breaker policy to prevent cascading failures
        /// </summary>
        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return Policy
                .Handle<HttpRequestException>()
                .Or<OperationCanceledException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (exception, duration) =>
                    {
                        Console.WriteLine($"Circuit breaker opened for {duration.TotalSeconds} seconds. Reason: {exception.Exception?.Message ?? exception.Result?.StatusCode.ToString()}");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Circuit breaker reset - requests will be allowed again");
                    });
        }

        /// <summary>
        /// Determines if a status code should trigger a retry
        /// </summary>
        private static bool IsRetryableStatusCode(System.Net.HttpStatusCode statusCode)
        {
            return statusCode == System.Net.HttpStatusCode.RequestTimeout ||
                   (int)statusCode == 429 || // TooManyRequests
                   statusCode == System.Net.HttpStatusCode.InternalServerError ||
                   statusCode == System.Net.HttpStatusCode.BadGateway ||
                   statusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                   statusCode == System.Net.HttpStatusCode.GatewayTimeout;
        }
    }
}