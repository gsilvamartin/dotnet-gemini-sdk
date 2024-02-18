using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Requester;
using DotnetGeminiSDK.Requester.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetGeminiSDK
{
    public static class GeminiServiceExtensions
    {
        public static IServiceCollection AddGeminiClient(this IServiceCollection services,
            Action<GoogleGeminiConfig> configure)
        {
            var config = new GoogleGeminiConfig();
            configure?.Invoke(config);

            services.AddSingleton(config);
            services.AddSingleton<IApiRequester, ApiRequester>();
            services.AddSingleton<GeminiClient>();

            return services;
        }
    }
}