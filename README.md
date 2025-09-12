# DotnetGeminiSDK 

![NuGet Version](https://img.shields.io/nuget/v/DotnetGeminiSDK) ![NuGet Downloads](https://img.shields.io/nuget/dt/DotnetGeminiSDK)

Welcome to DotnetGeminiSDK, a .NET SDK for interacting with the Google Gemini API. This SDK empowers developers to harness the capabilities of machine learning models to generate creative content effortlessly.

## Table of Contents
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
  - [Text Prompt](#text-prompt)
  - [Stream Text Prompt](#stream-text-prompt)
  - [Multiple Text Prompt](#multiple-text-prompt)
  - [Get Model](#get-model)
  - [Get All Models](#get-models)
  - [Image Prompt](#image-prompt)
  - [Embedded](#embedded)
  - [Batch Embedded](#batch-embedded)
  - [Exception Handling](#exception-handling)
- [Contributing](#contributing)
- [License](#license)
  
# What is Google Gemini?
Google Gemini is an advanced AI platform that offers various interfaces for commands tailored to different use cases. It allows users to interact with machine learning models for generating content and responses to instructions. The platform supports free-form commands, structured commands, and chat-based requests. Additionally, Gemini provides the ability to adjust models for specific tasks, enhancing their performance for particular use cases.

## Installation 📦
Get started by installing the DotnetGeminiSDK NuGet package. Run the following command in the NuGet Package Manager Console:

```sh
Install-Package DotnetGeminiSDK
```

Or, if you prefer using the .NET CLI:

```sh
dotnet add package DotnetGeminiSDK
```

## Configuration ⚙️
To use the Gemini SDK, configure the `GoogleGeminiConfig` object. Add the Gemini client to your service collection using `GeminiServiceExtensions`:

> [!NOTE]
> Only used when using the dependency injection method.

```csharp
using DotnetGeminiSDK;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGeminiClient(config =>
        {
            config.ApiKey = "YOUR_GOOGLE_GEMINI_API_KEY";
            config.ImageBaseUrl = "CURRENTLY_IMAGE_BASE_URL";
            config.TextBaseUrl = "CURRENTLY_IMAGE_BASE_URL";
        });
    }
}
```

## How to use? 🔎
### Dependency Injection

When you incorporate the Gemini client, you can seamlessly inject it into your code for immediate use.

```csharp
using DotnetGeminiSDK.Client.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public class YourClass
{
    private readonly IGeminiClient _geminiClient;

    public YourClass(IGeminiClient geminiClient)
    {
        _geminiClient = geminiClient;
    }

    public async Task Example()
    {
        var response = await _geminiClient.TextPrompt("Text for processing");
        // Process the response as needed
    }
}
```

### Class Instantiation

If you don't want to use dependency injection, you can instantiate the GeminiClient class, as a constructor parameter, place your settings in the GoogleGeminiConfig instance.

```csharp
using DotnetGeminiSDK.Client.Interfaces;

public class YourClass
{
    private readonly GeminiClient _geminiClient;

    public YourClass()
    {
        _geminiClient = new GeminiClient(new GoogleGeminiConfig(){ //Place your settings here });
    }

    public async Task Example()
    {
        var response = await _geminiClient.TextPrompt("Text for processing");
        // Process the response as needed
    }
}
```

## Implemented features 👾

### Core Features
- [x] Text Prompt
- [x] Stream Text Prompt
- [x] Multiple Text Prompt
- [x] Image Prompt
- [x] Counting Tokens
- [x] Get Model
- [x] List Models
- [x] Embedding
- [x] Batch Embedding

### New Features (2025)
- [x] **System Instructions** - Define model behavior
- [x] **Function Calling** - Execute custom functions
- [x] **Latest Models** - Gemini 2.5 Pro, 1.5 Pro, 1.5 Flash
- [x] **Enhanced Error Handling** - Specific exceptions and retry logic
- [x] **Structured Logging** - Better observability
- [x] **Retry Logic** - Automatic retry with exponential backoff
- [x] **Flexible Configuration** - Support for different models and settings
- [x] **Video Support** - Process video files with AI
- [x] **Audio Support** - Process audio files with AI
- [x] **Multimodal Processing** - Handle multiple media types in one request
- [x] **File Upload** - Upload files to Gemini API for processing
- [x] **Caching System** - Intelligent response caching
- [x] **Metrics & Telemetry** - Performance monitoring and analytics
- [x] **Validation Service** - Input validation and model compatibility
- [x] **Enhanced Media Support** - Support for 25+ media types
- [x] **Deterministic Responses** - Seed support for reproducible results
- [x] **JSON Response Mode** - Structured output with schema validation
- [x] **Debugging Support** - SourceLink and PDB symbols for full debugging experience

## Usage 🚀

### ⚠️ **Production Ready Setup**

For production applications, always use dependency injection with `IHttpClientFactory` to avoid connection pool starvation:

```csharp
// ✅ RECOMMENDED: Production setup
var services = new ServiceCollection();
services.AddHttpClient(); // Registers IHttpClientFactory
services.AddGeminiClient(config =>
{
    config.ApiKey = "your-api-key";
    config.TimeoutSeconds = 30;
    config.EnableRetry = true;
    config.MaxRetryAttempts = 3;
    config.EnableLogging = true;
});

var serviceProvider = services.BuildServiceProvider();
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
```

```csharp
// ❌ NOT RECOMMENDED: Legacy usage (can cause connection pool starvation)
var config = new GoogleGeminiConfig { ApiKey = "your-api-key" };
var legacyClient = new GeminiClient(config); // Creates new HttpClient internally
```

### Text Prompt 📝
Prompt the Gemini API with a text message using the `TextPrompt` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.TextPrompt("Write a story about a magic backpack");
```

### Stream Text Prompt 🔁
Prompt the Gemini API with a text message using the `StreamTextPrompt` method:

> [!NOTE]
> This diffears from the text prompt, it receives the response as string and in chunks.

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.StreamTextPrompt("Write a story about a magic backpack", (chunk) => {
  Console.WriteLine("Process your chunk of response here");
});
```

### Multiple Text Prompt 📚
Prompt the Gemini API with multiple text messages using the `TextPrompt` method with a list of `Content` objects:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();

var messages = new List<Content>
{
    new Content
    {
        Parts = new List<Part>
        {
            new Part
            {
                Text = "Write a story about a magic backpack"
            }
        }
    },
    // Add more Content objects as needed
};

var response = await geminiClient.TextPrompt(messages);
```

### Get Model 📒
Get the specific model details of Gemini using `GetModel` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.GetModel("gemini-model-v1");
```

### List all models 🔖
Get all Gemini models using `GetModels` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.GetModels();
```

### Count Tokens 1️⃣
Prompt the Gemini API with a text message using the `CountTokens` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.CountTokens("Write a story about a magic backpack");
```

> [!NOTE]
> You can use list of messages and list of content to call this method too.

### Image Prompt 🖼️
#### Using file
Prompt the Gemini API with an image and a text message using the `ImagePrompt` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var image = File.ReadAllBytes("path/to/your/image.jpg");
var response = await geminiClient.ImagePrompt("Describe this image", image, ImageMimeType.Jpeg);
```

#### Using Base64 String
Prompt the Gemini API with an base64 string and a text message using the `ImagePrompt` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var base64Image = "image-as-base64";
var response = await geminiClient.ImagePrompt("Describe this image in details", base64Image, ImageMimeType.Jpeg);
```

### Embedded 🪡
Prompt the Gemini API with a text message and using embedded technique using the `EmbeddedContentsPrompt` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.EmbeddedContentsPrompt("Write a story about a magic backpack");
```

> [!NOTE]
> You can use list of messages and list of content to call this method too.

### Batch Embedded 🪡
Prompt the Gemini API with a text message and using batch embedded technique using the `BatchEmbeddedContentsPrompt` method:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.EmbeddedContentsPrompt("Write a story about a magic backpack");
```

> [!NOTE]
> You can use list of messages and list of content to call this method too.

### System Instructions 🎯
Define the behavior of the model using system instructions:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.TextPromptWithSystemInstruction(
    "How do I create a REST API in C#?",
    "You are a helpful coding assistant. Always provide code examples in C# and explain the code clearly."
);
```

### Function Calling 🔧
Enable the model to call custom functions:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();

var functions = new List<FunctionDeclaration>
{
    new FunctionDeclaration
    {
        Name = "get_weather",
        Description = "Get the current weather for a location",
        Parameters = new FunctionParameters
        {
            Type = "object",
            Properties = new Dictionary<string, FunctionParameter>
            {
                {
                    "location", new FunctionParameter
                    {
                        Type = "string",
                        Description = "The city and state, e.g. San Francisco, CA"
                    }
                }
            },
            Required = new List<string> { "location" }
        }
    }
};

var response = await geminiClient.TextPromptWithFunctions(
    "What's the weather like in New York?",
    functions
);
```

### Using Latest Models 🚀
Use the most advanced Gemini models:

```csharp
var config = new GoogleGeminiConfig
{
    ApiKey = "your-api-key",
    DefaultTextModel = GeminiModels.Gemini25Pro, // or Gemini15Pro, Gemini15Flash
    EnableLogging = true,
    EnableRetry = true
};

var client = new GeminiClient(config);
var response = await client.TextPrompt("Explain quantum computing");
```

### Enhanced Configuration ⚙️
Configure advanced features:

```csharp
var config = new GoogleGeminiConfig
{
    ApiKey = "your-api-key",
    DefaultTextModel = GeminiModels.Gemini15Pro,
    EnableRetry = true,
    MaxRetryAttempts = 3,
    RetryDelayMs = 1000,
    EnableLogging = true,
    TimeoutSeconds = 30
};
```

### Video Processing 🎥
Process video files with AI:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var videoBytes = File.ReadAllBytes("path/to/video.mp4");
var response = await geminiClient.VideoPrompt(
    "Describe what happens in this video",
    videoBytes,
    MediaMimeType.VideoMp4
);
```

### Audio Processing 🎵
Process audio files with AI:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var audioBytes = File.ReadAllBytes("path/to/audio.mp3");
var response = await geminiClient.AudioPrompt(
    "Transcribe this audio and summarize the key points",
    audioBytes,
    MediaMimeType.AudioMp3
);
```

### Multimodal Processing 🎨
Process multiple media types in one request:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();

var mediaFiles = new List<MediaFile>
{
    new MediaFile
    {
        Content = File.ReadAllBytes("image.jpg"),
        MimeType = MediaMimeType.ImageJpeg,
        FileName = "image.jpg"
    },
    new MediaFile
    {
        Content = File.ReadAllBytes("document.pdf"),
        MimeType = MediaMimeType.ApplicationPdf,
        FileName = "document.pdf"
    }
};

var response = await geminiClient.MultimodalPrompt(
    "Analyze these files and provide insights",
    mediaFiles
);
```

### File Upload & Management 📁
Upload files to Gemini API for processing:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();

// Upload a file
var fileBytes = File.ReadAllBytes("large-document.pdf");
var uploadResponse = await geminiClient.UploadFile(
    fileBytes,
    MediaMimeType.ApplicationPdf,
    "document.pdf"
);

// Get file information
var fileInfo = await geminiClient.GetFileInfo(uploadResponse.File.Uri);

// Use file in prompts
var response = await geminiClient.TextPrompt(
    "Summarize this document",
    systemInstruction: new SystemInstruction
    {
        Parts = new List<Part>
        {
            new Part { Text = "You are a document analysis expert." }
        }
    }
);

// Delete file when done
await geminiClient.DeleteFile(uploadResponse.File.Uri);
```

### Caching & Performance 📈
Enable intelligent caching and performance monitoring:

```csharp
var config = new GoogleGeminiConfig
{
    ApiKey = "your-api-key",
    EnableLogging = true,
    EnableRetry = true
};

// Caching is automatically enabled
var response1 = await client.TextPrompt("What is AI?"); // First call - API request
var response2 = await client.TextPrompt("What is AI?"); // Second call - cached response

// Get performance metrics
var metrics = await client.GetMetricsSummary();
Console.WriteLine($"Total requests: {metrics.Counters["gemini.requests"]}");
Console.WriteLine($"Average response time: {metrics.Timings["gemini.duration"].Average}ms");
```

### Deterministic Responses 🎯
Get reproducible results for testing and consistency:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();

// Create request with seed for deterministic responses
var request = new GeminiMessageRequest
{
    Contents = new List<Content>
    {
        new()
        {
            Parts = new List<Part>
            {
                new() { Text = "Write a short poem about coding." }
            }
        }
    },
    GenerationConfig = new GenerationConfig
    {
        Temperature = 0.7,
        MaxOutputTokens = 100,
        Seed = 42 // This makes responses deterministic
    }
};

// Both calls will return identical responses
var response1 = await geminiClient.TextPrompt(request);
var response2 = await geminiClient.TextPrompt(request);

// Perfect for testing - no more flaky tests!
Console.WriteLine($"Responses are identical: {response1 == response2}");
```

### JSON Response Mode 📋
Get structured JSON responses with schema validation:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();

// Define JSON schema for structured output
var jsonSchema = new
{
    type = "object",
    properties = new
    {
        name = new { type = "string", description = "The person's name" },
        age = new { type = "integer", description = "The person's age" },
        email = new { type = "string", format = "email" },
        hobbies = new { type = "array", items = new { type = "string" } }
    },
    required = new[] { "name", "age", "email" }
};

var request = new GeminiMessageRequest
{
    Contents = new List<Content>
    {
        new()
        {
            Parts = new List<Part>
            {
                new() { Text = "Create a profile for a software developer." }
            }
        }
    },
    GenerationConfig = new GenerationConfig
    {
        Temperature = 0.3,
        MaxOutputTokens = 500,
        ResponseMimeType = "application/json",
        ResponseSchema = jsonSchema
    }
};

var response = await geminiClient.TextPrompt(request);
var jsonResponse = response?.Candidates?[0]?.Content?.Parts?[0]?.Text;

// Parse the structured JSON response
var profile = JsonConvert.DeserializeObject(jsonResponse);
Console.WriteLine($"Name: {profile.name}");
Console.WriteLine($"Age: {profile.age}");
```

#### Using String Schema
You can also provide the schema as a JSON string:

```csharp
var jsonSchemaString = @"{
    ""type"": ""object"",
    ""properties"": {
        ""title"": { ""type"": ""string"" },
        ""author"": { ""type"": ""string"" },
        ""year"": { ""type"": ""integer"" },
        ""rating"": { ""type"": ""number"", ""minimum"": 0, ""maximum"": 10 }
    },
    ""required"": [""title"", ""author"", ""year""]
}";

var request = new GeminiMessageRequest
{
    Contents = new List<Content>
    {
        new()
        {
            Parts = new List<Part>
            {
                new() { Text = "Recommend a science fiction book." }
            }
        }
    },
    GenerationConfig = new GenerationConfig
    {
        ResponseMimeType = "application/json",
        ResponseSchema = jsonSchemaString
    }
};
```

### Validation & Error Handling 🛡️
Comprehensive validation and error handling:

```csharp
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();

try
{
    // This will validate the model and parameters
    var response = await geminiClient.TextPrompt(
        "Hello world",
        generationConfig: new GenerationConfig
        {
            Temperature = 0.7,
            MaxOutputTokens = 1000
        }
    );
}
catch (GeminiModelNotFoundException ex)
{
    Console.WriteLine($"Model not found: {ex.ModelName}");
}
catch (GeminiRateLimitException ex)
{
    Console.WriteLine($"Rate limit exceeded. Retry after: {ex.RetryAfter}");
}
catch (GeminiSafetyException ex)
{
    Console.WriteLine($"Content blocked: {string.Join(", ", ex.BlockedReasons)}");
}
```

## Debugging 🔍

The library includes comprehensive debugging support:

- **SourceLink**: Automatically download source code in Visual Studio
- **PDB Symbols**: Full breakpoint support and step-through debugging
- **Symbol Packages**: Separate `.snupkg` packages for enhanced debugging
- **CI/CD Integration**: Automated symbol package publishing via GitHub Actions

### Quick Setup

1. **Visual Studio**: SourceLink works automatically - just step into library methods
2. **VS Code**: Install C# extension and configure launch.json
3. **Rider**: Enable SourceLink in settings

### Package Structure
- **Main Package**: `DotnetGeminiSDK.x.x.x.nupkg` (includes embedded PDB)
- **Symbol Package**: `DotnetGeminiSDK.x.x.x.snupkg` (enhanced debugging symbols)

For detailed debugging instructions, see [DEBUGGING.md](DEBUGGING.md).

## Production Features 🏭

### **Resilience & Performance**
- **Connection Pooling**: Proper HttpClient management prevents connection pool starvation
- **Retry Policy**: Exponential backoff with Polly for transient failures
- **Circuit Breaker**: Prevents cascading failures during outages
- **Timeout Management**: Configurable timeouts for different scenarios
- **Logging**: Structured logging for monitoring and debugging

### **Error Handling**
- **Specific Exceptions**: `GeminiAuthenticationException`, `GeminiRateLimitException`, etc.
- **Retry Logic**: Automatic retry for retryable errors (5xx, 429, timeouts)
- **Circuit Breaker**: Opens after 5 consecutive failures, resets after 30 seconds

### **Performance Optimizations**
- **Connection Reuse**: HttpClient instances are pooled and reused
- **Caching**: Optional response caching to reduce API calls
- **Metrics**: Built-in metrics collection for monitoring
- **Async/Await**: Full async support for scalability

## Contributing 🤝
Contributions are welcome! Feel free to open issues or pull requests to enhance the SDK.

## License 📜
This project is licensed under the MIT License.
