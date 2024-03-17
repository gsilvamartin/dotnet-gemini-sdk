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

## Implemented features 👾

- [x] Text Prompt
- [x] Stream Text Prompt
- [x] Multiple Text Prompt
- [x] Image Prompt
- [x] Counting Tokens
- [x] Get Model
- [x] List Models
- [ ] Embedding
- [ ] Batch Embedding

## Usage 🚀
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
Get all models using `GetModels` method:

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

## Contributing 🤝
Contributions are welcome! Feel free to open issues or pull requests to enhance the SDK.

## License 📜
This project is licensed under the MIT License.
