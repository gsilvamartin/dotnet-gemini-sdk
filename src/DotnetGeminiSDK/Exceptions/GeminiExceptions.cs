using System;

namespace DotnetGeminiSDK.Exceptions
{
    /// <summary>
    /// Base exception for Gemini API errors
    /// </summary>
    public class GeminiException : Exception
    {
        public int? StatusCode { get; }
        public string? ErrorCode { get; }

        public GeminiException(string message) : base(message) { }
        public GeminiException(string message, Exception innerException) : base(message, innerException) { }
        public GeminiException(string message, int statusCode, string? errorCode = null) : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }

    /// <summary>
    /// Exception thrown when API key is invalid or missing
    /// </summary>
    public class GeminiAuthenticationException : GeminiException
    {
        public GeminiAuthenticationException(string message) : base(message) { }
        public GeminiAuthenticationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when rate limit is exceeded
    /// </summary>
    public class GeminiRateLimitException : GeminiException
    {
        public TimeSpan? RetryAfter { get; }

        public GeminiRateLimitException(string message) : base(message) { }
        public GeminiRateLimitException(string message, TimeSpan? retryAfter) : base(message)
        {
            RetryAfter = retryAfter;
        }
    }

    /// <summary>
    /// Exception thrown when request is invalid
    /// </summary>
    public class GeminiBadRequestException : GeminiException
    {
        public GeminiBadRequestException(string message) : base(message) { }
        public GeminiBadRequestException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when content is blocked by safety filters
    /// </summary>
    public class GeminiSafetyException : GeminiException
    {
        public string[]? BlockedReasons { get; }

        public GeminiSafetyException(string message) : base(message) { }
        public GeminiSafetyException(string message, string[]? blockedReasons) : base(message)
        {
            BlockedReasons = blockedReasons;
        }
    }

    /// <summary>
    /// Exception thrown when quota is exceeded
    /// </summary>
    public class GeminiQuotaExceededException : GeminiException
    {
        public GeminiQuotaExceededException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when model is not found or unavailable
    /// </summary>
    public class GeminiModelNotFoundException : GeminiException
    {
        public string? ModelName { get; }

        public GeminiModelNotFoundException(string message) : base(message) { }
        public GeminiModelNotFoundException(string message, string? modelName) : base(message)
        {
            ModelName = modelName;
        }
    }
}
