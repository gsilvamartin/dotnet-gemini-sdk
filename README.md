# DotnetGeminiSDK 🚀

Welcome to DotnetGeminiSDK, a .NET SDK for interacting with the Google Gemini API. This SDK empowers developers to harness the capabilities of machine learning models to generate creative content effortlessly.

## Table of Contents
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
  - [Text Prompt](#text-prompt)
  - [Multiple Text Prompt](#multiple-text-prompt)
  - [Image Prompt](#image-prompt)
  - [Exception Handling](#exception-handling)
- [Contributing](#contributing)
- [License](#license)

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

```
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

## Usage 🚀
### Text Prompt 📝
Prompt the Gemini API with a text message using the `TextPrompt` method:

```
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var response = await geminiClient.TextPrompt("Write a story about a magic backpack");
```

### Multiple Text Prompt 📚
Prompt the Gemini API with multiple text messages using the `TextPrompt` method with a list of `Content` objects:

```
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

### Image Prompt 🖼️
Prompt the Gemini API with an image and a text message using the `ImagePrompt` method:

```
var geminiClient = serviceProvider.GetRequiredService<IGeminiClient>();
var base64Image = Convert.ToBase64String(File.ReadAllBytes("path/to/your/image.jpg"));
var response = await geminiClient.ImagePrompt("Describe this image", base64Image, ImageMimeType.Jpeg);
```

## Contributing 🤝
Contributions are welcome! Feel free to open issues or pull requests to enhance the SDK.

## License 📜
This project is licensed under the MIT License.
