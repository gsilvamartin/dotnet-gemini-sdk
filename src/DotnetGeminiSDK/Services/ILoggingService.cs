using System;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Services
{
    /// <summary>
    /// Interface for logging service
    /// </summary>
    public interface ILoggingService
    {
        void LogInfo(string message, object? data = null);
        void LogWarning(string message, object? data = null);
        void LogError(string message, Exception? exception = null, object? data = null);
        void LogDebug(string message, object? data = null);
    }

    /// <summary>
    /// Default console logging service
    /// </summary>
    public class ConsoleLoggingService : ILoggingService
    {
        public void LogInfo(string message, object? data = null)
        {
            Console.WriteLine($"[INFO] {message}");
            if (data != null)
            {
                Console.WriteLine($"[DATA] {JsonConvert.SerializeObject(data, Formatting.Indented)}");
            }
        }

        public void LogWarning(string message, object? data = null)
        {
            Console.WriteLine($"[WARNING] {message}");
            if (data != null)
            {
                Console.WriteLine($"[DATA] {JsonConvert.SerializeObject(data, Formatting.Indented)}");
            }
        }

        public void LogError(string message, Exception? exception = null, object? data = null)
        {
            Console.WriteLine($"[ERROR] {message}");
            if (exception != null)
            {
                Console.WriteLine($"[EXCEPTION] {exception}");
            }
            if (data != null)
            {
                Console.WriteLine($"[DATA] {JsonConvert.SerializeObject(data, Formatting.Indented)}");
            }
        }

        public void LogDebug(string message, object? data = null)
        {
            Console.WriteLine($"[DEBUG] {message}");
            if (data != null)
            {
                Console.WriteLine($"[DATA] {JsonConvert.SerializeObject(data, Formatting.Indented)}");
            }
        }
    }
}
