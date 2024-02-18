using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Requester;
using DotnetGeminiSDK.Requester.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetGeminiSDK;

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

        services.AddSingleton(config);
        services.AddSingleton<IApiRequester, ApiRequester>();
        services.AddSingleton<IGeminiClient, GeminiClient>();

        return services;
    }
}