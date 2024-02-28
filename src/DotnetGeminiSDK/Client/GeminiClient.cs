using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
using DotnetGeminiSDK.Requester.Interfaces;
using Content = DotnetGeminiSDK.Model.Request.Content;
using Part = DotnetGeminiSDK.Model.Request.Part;

namespace DotnetGeminiSDK.Client;

/// <summary>
/// Class to interact with Google Gemini API
/// </summary>
public class GeminiClient : IGeminiClient
{
    private readonly IApiRequester _apiRequester;
    private readonly GoogleGeminiConfig _config;

    public GeminiClient(GoogleGeminiConfig config, IApiRequester apiRequester)
    {
        _config = config;
        _apiRequester = apiRequester;
    }

    /// <summary>
    /// Send a message to be processed using Google Gemini API,
    /// the method returns a GeminiMessageResponse with all the response fields from api
    ///
    /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-only_input
    /// </summary>
    /// <param name="message"></param>
    /// <param name="generationConfig">A optional generation config</param>
    /// <param name="safetySetting">A optional safety setting</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<GeminiMessageResponse?> TextPrompt(
        string message,
        GenerationConfig? generationConfig = null,
        SafetySetting? safetySetting = null)
    {
        try
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message, generationConfig, safetySetting);

            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }
        catch (Exception e)
        {
            throw new Exception("Unexpected error occurred.", e);
        }
    }

    /// <summary>
    /// Send multiple messages to be processed using Google Gemini API,
    /// the method returns a GeminiMessageResponse with all the response fields from api
    ///
    /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-only_input
    /// </summary>
    /// <param name="messages">Messages to be processed as content model</param>
    /// <param name="generationConfig">A optional generation config</param>
    /// <param name="safetySetting">A optional safety setting</param>
    /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<GeminiMessageResponse?> TextPrompt(
        List<Content> messages,
        GenerationConfig? generationConfig = null,
        SafetySetting? safetySetting = null)
    {
        try
        {
            if (!messages.Any()) throw new ArgumentException("Messages cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }
        catch (Exception e)
        {
            throw new Exception("Unexpected error occurred.", e);
        }
    }

    /// <summary>
    /// Send a message to be processed using Google Gemini API.
    ///
    /// The method returns a GeminiMessageResponse with all the response fields from api.
    /// The response is streamed as an observable.
    ///
    /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-only_input
    /// </summary>
    /// <param name="message">Message to be processed as content model</param>
    /// <param name="generationConfig">A optional generation config</param>
    /// <param name="safetySetting">A optional safety setting</param>
    /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Exception"></exception>
    public IObservable<GeminiMessageResponse?> StreamTextPrompt(
        string message,
        GenerationConfig? generationConfig = null,
        SafetySetting? safetySetting = null)
    {
        try
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:streamGenerateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(message, generationConfig, safetySetting);

            return _apiRequester.PostStream<GeminiMessageResponse>(promptUrl, request);
        }
        catch (Exception e)
        {
            throw new Exception("Unexpected error occurred.", e);
        }
    }

    /// <summary>
    /// Send a message to be processed using Google Gemini API.
    ///
    /// The method returns a GeminiMessageResponse with all the response fields from api.
    /// The response is streamed as an observable.
    ///
    /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-only_input
    /// </summary>
    /// <param name="messages">Messages to be processed as content model</param>
    /// <param name="generationConfig">A optional generation config</param>
    /// <param name="safetySetting">A optional safety setting</param>
    /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Exception"></exception>
    public IObservable<GeminiMessageResponse?> StreamTextPrompt(
        List<Content> messages,
        GenerationConfig? generationConfig = null,
        SafetySetting? safetySetting = null)
    {
        try
        {
            if (!messages.Any()) throw new ArgumentException("Messages cannot be empty.");

            var promptUrl = $"{_config.TextBaseUrl}:streamGenerateContent?key={_config.ApiKey}";
            var request = BuildGeminiRequest(messages, generationConfig, safetySetting);

            return _apiRequester.PostStream<GeminiMessageResponse>(promptUrl, request);
        }
        catch (Exception e)
        {
            throw new Exception("Unexpected error occurred.", e);
        }
    }

    /// <summary>
    /// Send a message and a image to be processed using Google Gemini API,
    /// the method returns a GeminiMessageResponse with all the response fields from api
    ///
    /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-and-image_input
    /// </summary>
    /// <param name="message">Message to be processed</param>
    /// <param name="image">Image as array of bytes format</param>
    /// <param name="mimeType">Mime type of the image</param>
    /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<GeminiMessageResponse?> ImagePrompt(string message, byte[] image, ImageMimeType mimeType)
    {
        try
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");
            if (image.Length == 0) throw new ArgumentException("Image cannot be empty.");

            var mimeTypeString = GetMimeTypeString(mimeType);
            var promptUrl = $"{_config.ImageBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildImageGeminiRequest(message, Convert.ToBase64String(image), mimeTypeString);

            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }
        catch (Exception e)
        {
            throw new Exception("Unexpected error occurred.", e);
        }
    }

    /// <summary>
    /// Send a message to be processed using Google Gemini API,
    /// the method returns a GeminiMessageResponse with all the response fields from api
    ///
    /// REF: https://ai.google.dev/tutorials/rest_quickstart#text-and-image_input
    /// </summary>
    /// <param name="message">Message to be processed</param>
    /// <param name="base64Image">Base64 image to process</param>
    /// <param name="mimeType">Mime type of the image</param>
    /// <returns>Returns a GeminiMessageResponse with all the response fields from api</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<GeminiMessageResponse?> ImagePrompt(string message, string base64Image, ImageMimeType mimeType)
    {
        try
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty.");
            if (string.IsNullOrEmpty(base64Image)) throw new ArgumentException("Image cannot be empty.");

            var mimeTypeString = GetMimeTypeString(mimeType);
            var promptUrl = $"{_config.ImageBaseUrl}:generateContent?key={_config.ApiKey}";
            var request = BuildImageGeminiRequest(message, base64Image, mimeTypeString);

            return await _apiRequester.PostAsync<GeminiMessageResponse>(promptUrl, request);
        }
        catch (Exception e)
        {
            throw new Exception("Unexpected error occurred.", e);
        }
    }

    /// <summary>
    /// Build a GeminiMessageRequest object to process image from a string message, base 64 and mimetype
    /// </summary>
    /// <param name="message">Message to be processed</param>
    /// <param name="base64Image">Base64 image to process</param>
    /// <param name="mimeType">Mime type of the image</param>
    /// <param name="generationConfig">A optional generation config</param>
    /// <param name="safetySetting">A optional safety setting</param>
    /// <returns>A GeminiMessageRequest built to sending for api</returns>
    private static GeminiMessageRequest BuildImageGeminiRequest(
        string message,
        string base64Image,
        string mimeType,
        GenerationConfig? generationConfig = null,
        SafetySetting? safetySetting = null)
    {
        return new GeminiMessageRequest
        {
            Contents =
            [
                new Content
                {
                    Parts =
                    [
                        new Part
                        {
                            Text = message,
                        },
                        new Part
                        {
                            InlineData = new InlineData
                            {
                                MimeType = mimeType,
                                Data = base64Image
                            }
                        }
                    ]
                }
            ],
            GenerationConfig = generationConfig,
            SafetySetting = safetySetting
        };
    }

    /// <summary>
    /// Build a GeminiMessageRequest object from a string message
    /// </summary>
    /// <param name="messages">Messages to be processed</param>
    /// <param name="generationConfig">A optional generation config</param>
    /// <param name="safetySetting">A optional safety setting</param>
    /// <returns>A request containing GeminiMessageRequest</returns>
    private static GeminiMessageRequest BuildGeminiRequest(
        List<Content> messages,
        GenerationConfig? generationConfig = null,
        SafetySetting? safetySetting = null)
    {
        return new GeminiMessageRequest
        {
            Contents = messages,
            GenerationConfig = generationConfig,
            SafetySetting = safetySetting
        };
    }

    /// <summary>
    /// Build a GeminiMessageRequest object from a string message
    /// </summary>
    /// <param name="message">Message to be processed</param>
    /// <param name="generationConfig">A optional generation config</param>
    /// <param name="safetySetting">A optional safety setting</param>
    /// <returns>A request containing GeminiMessageRequest</returns>
    private static GeminiMessageRequest BuildGeminiRequest(
        string message,
        GenerationConfig? generationConfig = null,
        SafetySetting? safetySetting = null)
    {
        return new GeminiMessageRequest
        {
            Contents =
            [
                new Content
                {
                    Parts =
                    [
                        new Part
                        {
                            Text = message
                        }
                    ]
                }
            ],
            GenerationConfig = generationConfig,
            SafetySetting = safetySetting
        };
    }

    /// <summary>
    /// Get the mime type string from the enum
    /// </summary>
    /// <param name="mimeType">Mime-type as enum</param>
    /// <returns>Return the mime type as string</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static string GetMimeTypeString(ImageMimeType mimeType)
    {
        return mimeType switch
        {
            ImageMimeType.Jpeg => "image/jpeg",
            ImageMimeType.Png => "image/png",
            ImageMimeType.Jpg => "image/jpg",
            ImageMimeType.Heic => "image/heic",
            ImageMimeType.Heif => "image/heif",
            ImageMimeType.Webp => "image/webp",
            _ => throw new ArgumentOutOfRangeException(nameof(mimeType), mimeType, "Invalid image mime type.")
        };
    }
}