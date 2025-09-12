# Changelog

All notable changes to this project will be documented in this file.

## [2.0.0] - 2025-09-XX

### 🚀 Major Features Added

#### **System Instructions**
- Added support for system instructions to define model behavior
- New `TextPromptWithSystemInstruction` method for easy system instruction usage
- System instructions can be used with all text generation methods

#### **Function Calling (Tools)**
- Complete function calling support with `FunctionDeclaration`, `FunctionParameters`, and `FunctionCall` models
- New `TextPromptWithFunctions` method for easy function calling
- Support for custom function execution and responses
- Tool configuration with `ToolConfig` and `FunctionCallingConfig`

#### **Latest Gemini Models**
- Updated to support Gemini 2.5 Pro (most advanced model)
- Added Gemini 1.5 Pro and 1.5 Flash support
- New `GeminiModels` class with all available models
- Backward compatibility with legacy models

#### **Enhanced Error Handling**
- New specific exception types: `GeminiException`, `GeminiAuthenticationException`, `GeminiRateLimitException`, `GeminiBadRequestException`, `GeminiSafetyException`, `GeminiQuotaExceededException`, `GeminiModelNotFoundException`
- Better error context and debugging information

#### **Retry Logic**
- Automatic retry with exponential backoff
- Configurable retry attempts and delays
- Smart retry logic for network errors and rate limits

#### **Structured Logging**
- New `ILoggingService` interface for logging
- Default `ConsoleLoggingService` implementation
- Configurable logging levels and data serialization

#### **Enhanced Configuration**
- New configuration options for models, retry, logging, and timeouts
- Support for different default models (text, image, embedding)
- Flexible configuration for different use cases

#### **Video and Audio Support**
- Complete video processing with `VideoPrompt` method
- Audio processing with `AudioPrompt` method
- Support for 8 video formats (MP4, AVI, MOV, WMV, FLV, WebM, MKV, 3GP)
- Support for 8 audio formats (MP3, WAV, AAC, OGG, FLAC, M4A, WMA, AIFF)

#### **Multimodal Processing**
- `MultimodalPrompt` method for processing multiple media types
- Support for mixed media in single requests
- Enhanced `MediaFile` class for file representation

#### **File Management**
- `UploadFile` method for uploading files to Gemini API
- `GetFileInfo` method for retrieving file information
- `DeleteFile` method for cleaning up uploaded files
- Support for large file processing

#### **Caching System**
- Intelligent response caching with `ICacheService`
- Memory-based cache implementation
- Configurable cache expiration
- Automatic cache invalidation

#### **Metrics and Telemetry**
- Comprehensive metrics collection with `IMetricsService`
- Request counting, timing, and error tracking
- Performance analytics and monitoring
- Console-based metrics reporting

#### **Validation Service**
- Input validation with `IValidationService`
- Model compatibility checking
- Parameter validation for function calling
- Safety settings validation

#### **Deterministic Responses**
- Added `Seed` property to `GenerationConfig` for reproducible results
- Perfect for testing and consistent output generation
- Support for deterministic text generation across multiple calls
- Solves the problem of flaky tests with non-deterministic AI responses

#### **JSON Response Mode**
- Added `ResponseMimeType` property to `GenerationConfig` for structured output
- Added `ResponseSchema` property for JSON schema validation
- Support for both object and string-based schema definitions
- Enables structured data generation with type safety
- Perfect for APIs, data processing, and structured content generation

#### **Debugging Support**
- Added SourceLink support for automatic source code downloading
- Enabled PDB symbol generation for full debugging experience
- Created separate symbol packages (.snupkg) for enhanced debugging
- Integrated debugging support into existing CI/CD pipeline
- Added comprehensive debugging documentation
- Support for Visual Studio, VS Code, and JetBrains Rider
- Enables step-through debugging and breakpoint support
- Automated symbol package publishing via GitHub Actions

#### **Production-Ready HTTP Client Management**
- **CRITICAL FIX**: Replaced direct HttpClient usage with IHttpClientFactory
- **Connection Pool Management**: Prevents connection pool starvation in production
- **Polly Integration**: Added retry policies with exponential backoff
- **Circuit Breaker**: Implemented circuit breaker pattern for resilience
- **Proper DI Registration**: Updated service registration for production use
- **Legacy Support**: Marked old constructors as obsolete with warnings
- **Performance Optimization**: Connection reuse and proper timeout management

### 🔧 Improvements

#### **API Client**
- Updated `GeminiClient` with new constructor overloads
- Enhanced dependency injection support
- Better separation of concerns with service interfaces

#### **Service Extensions**
- Updated `GeminiServiceExtensions` with new service registration
- Support for custom logging and retry services
- Backward compatible service registration

#### **Documentation**
- Comprehensive examples in `GeminiExamples` class
- Updated README with new features and usage examples
- Better code documentation and XML comments

### 📦 New Files

- `Model/GeminiModels.cs` - Model constants and helper methods
- `Model/MediaMimeType.cs` - Media MIME type support with 25+ types
- `Model/MediaFile.cs` - Media file representation
- `Model/Response/FileUploadResponse.cs` - File upload response models
- `Exceptions/GeminiExceptions.cs` - Specific exception types
- `Services/ILoggingService.cs` - Logging service interface and implementation
- `Services/IRetryService.cs` - Retry service interface and implementation
- `Services/ICacheService.cs` - Caching service interface and implementation
- `Services/IMetricsService.cs` - Metrics and telemetry service
- `Services/IValidationService.cs` - Validation service for inputs and models
- `Examples/GeminiExamples.cs` - Usage examples for all features

### 🔄 Breaking Changes

- Updated `TextPrompt` method signature to include new optional parameters
- Updated `IGeminiClient` interface with new methods
- Updated `GoogleGeminiConfig` with new properties (backward compatible with defaults)

### 🐛 Bug Fixes

- Improved error handling and exception management
- Better parameter validation
- Fixed potential null reference issues

### 📈 Performance

- Added retry logic for better reliability
- Improved logging for better observability
- Better resource management

## [1.0.8] - Previous Version

### Features
- Basic text and image prompting
- Token counting
- Model listing
- Embeddings support
- Basic error handling

---

## Migration Guide

### From 1.x to 2.0

#### Basic Usage (No Changes Required)
```csharp
// This still works exactly the same
var response = await client.TextPrompt("Hello world");
```

#### Using New Features
```csharp
// System Instructions
var response = await client.TextPromptWithSystemInstruction(
    "How do I create a REST API?",
    "You are a helpful coding assistant."
);

// Function Calling
var functions = new List<FunctionDeclaration> { /* your functions */ };
var response = await client.TextPromptWithFunctions("What's the weather?", functions);

// Latest Models
var config = new GoogleGeminiConfig
{
    ApiKey = "your-key",
    DefaultTextModel = GeminiModels.Gemini25Pro
};
```

#### Configuration Updates
```csharp
// Old way (still works)
var config = new GoogleGeminiConfig { ApiKey = "your-key" };

// New way with enhanced features
var config = new GoogleGeminiConfig
{
    ApiKey = "your-key",
    DefaultTextModel = GeminiModels.Gemini15Pro,
    EnableRetry = true,
    EnableLogging = true
};
```
