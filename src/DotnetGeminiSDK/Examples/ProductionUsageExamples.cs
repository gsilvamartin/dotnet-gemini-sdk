using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using System.Net.Http;
using DotnetGeminiSDK;
using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Config;

namespace DotnetGeminiSDK.Examples
{
    /// <summary>
    /// Examples showing how to use the SDK in production scenarios
    /// </summary>
    public static class ProductionUsageExamples
    {
        /// <summary>
        /// Example of proper production setup using dependency injection
        /// </summary>
        public static async Task ProductionSetupExample()
        {
            // Create service collection for proper DI setup
            var services = new ServiceCollection();
            
            // Add HttpClient services
            services.AddHttpClient();
            
            // Add logging (simplified for netstandard compatibility)
            services.AddLogging();
            
            // Add Gemini SDK with proper configuration
            services.AddGeminiClient(config =>
            {
                config.ApiKey = "your-api-key-here";
                config.TimeoutSeconds = 30;
                config.EnableRetry = true;
                config.MaxRetryAttempts = 3;
                config.EnableLogging = true;
            });

            // Build service provider
            var serviceProvider = services.BuildServiceProvider();

                // Get the client from DI (uses ILoggerFactory internally)
                var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
            
            // Use the client
            var response = await geminiClient.TextPrompt("Hello, how are you?");
            Console.WriteLine(response?.Candidates?[0]?.Content?.Parts?[0]?.Text);
        }

        /// <summary>
        /// Example of using the SDK in a console application with proper error handling
        /// </summary>
        public static async Task ConsoleApplicationExample()
        {
            // Setup services
            var services = new ServiceCollection();
            
            // Add HttpClient
            services.AddHttpClient();
            
            // Add logging (simplified for netstandard compatibility)
            services.AddLogging();
            
            // Add Gemini SDK
            services.AddGeminiClient(config =>
            {
                config.ApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") 
                    ?? throw new InvalidOperationException("GEMINI_API_KEY environment variable is required");
                config.TimeoutSeconds = 30;
                config.EnableRetry = true;
                config.MaxRetryAttempts = 3;
                config.EnableLogging = true;
            });
            
            var serviceProvider = services.BuildServiceProvider();
            var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
            
            try
            {
                var response = await geminiClient.TextPrompt("What is the capital of France?");
                Console.WriteLine($"Response: {response?.Candidates?[0]?.Content?.Parts?[0]?.Text}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Example showing the difference between production and legacy usage
        /// </summary>
        public static void UsageComparisonExample()
        {
            // ❌ LEGACY USAGE (NOT RECOMMENDED FOR PRODUCTION)
            // This creates new HttpClient instances and can cause connection pool starvation
            var config = new GoogleGeminiConfig { ApiKey = "your-api-key" };
            var legacyClient = new DotnetGeminiSDK.Client.GeminiClient(config); // Creates new HttpClient internally
            
            // ✅ PRODUCTION USAGE (RECOMMENDED)
            // This uses IHttpClientFactory for proper connection management
            var services = new ServiceCollection();
            services.AddHttpClient(); // Registers IHttpClientFactory
            services.AddGeminiClient(cfg => cfg.ApiKey = "your-api-key");
            
            var serviceProvider = services.BuildServiceProvider();
            var productionClient = serviceProvider.GetRequiredService<IGeminiClient>();
            
            // The production client will:
            // - Use connection pooling
            // - Have retry policies with exponential backoff
            // - Have circuit breaker protection
            // - Proper logging and metrics
            // - Better error handling
        }

        /// <summary>
        /// Example of custom configuration for high-throughput scenarios
        /// </summary>
        public static void HighThroughputConfigurationExample()
        {
            var services = new ServiceCollection();
            
            // Configure HttpClient for high throughput
            services.AddHttpClient("GeminiClient", client =>
            {
                client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
                client.Timeout = TimeSpan.FromSeconds(60);
                client.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new System.Net.Http.HttpClientHandler
            {
                MaxConnectionsPerServer = 100 // Increase connection pool size
                // Note: PooledConnectionLifetime and PooledConnectionIdleTimeout are not available in netstandard
            });
            
            // Add Gemini SDK with custom configuration
            services.AddGeminiClient(config =>
            {
                config.ApiKey = "your-api-key";
                config.TimeoutSeconds = 60;
                config.EnableRetry = true;
                config.MaxRetryAttempts = 5; // More retries for high throughput
                config.EnableLogging = true;
            });
        }

        /// <summary>
        /// Example of using the client with manual configuration and ILoggerFactory
        /// </summary>
        public static async Task ManualConfigurationWithLoggerExample()
        {
            // Create configuration
            var config = new GoogleGeminiConfig
            {
                ApiKey = "your-api-key-here",
                TimeoutSeconds = 30,
                EnableRetry = true,
                MaxRetryAttempts = 3,
                EnableLogging = true
            };

            // Create HttpClientFactory and LoggerFactory
            var services = new ServiceCollection();
            services.AddHttpClient("GeminiClient", client =>
            {
                client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
                client.DefaultRequestHeaders.Add("User-Agent", "DotnetGeminiSDK/2.0.0");
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            services.AddLogging();
            
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            // Create client with proper configuration and logging
            var geminiClient = new GeminiClient(config, httpClientFactory, loggerFactory);

            // Use the client
            var response = await geminiClient.TextPrompt("Hello, how are you?");
            Console.WriteLine(response?.Candidates?[0]?.Content?.Parts?[0]?.Text);
        }
    }
}
