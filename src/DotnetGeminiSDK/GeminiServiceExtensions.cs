using DotnetGeminiSDK.Config;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetGeminiSDK
{
    public static class GeminiServiceExtensions
    {
        public static IServiceCollection AddGeminiClient(this IServiceCollection services,
            Action<GoogleGeminiConfig> configure)
        {
            var builder = GeminiClient.InitializeGeminiSDK();
            configure?.Invoke(builder);
            var client = builder.Build();

            services.AddSingleton(client);

            return services;
        }
    }
}