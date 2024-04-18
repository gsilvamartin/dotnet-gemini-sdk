using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;
using DotnetGeminiSDK.Requester.Interfaces;
using Moq;

namespace DotnetGeminiSDK.Tests
{
    public class GeminiClientTests
    {
        private readonly Mock<IApiRequester> _apiRequesterMock = new Mock<IApiRequester>();
        private readonly GeminiClient _client;

        public GeminiClientTests()
        {
            _client = new GeminiClient(new GoogleGeminiConfig(), _apiRequesterMock.Object);
        }

        [Fact]
        public async Task TextPrompt_WithSingleMessage_ReturnsResponse()
        {
            var message = "Test message";
            var expectedResponse = new GeminiMessageResponse();
            _apiRequesterMock.Setup(r => r.PostAsync<GeminiMessageResponse>(It.IsAny<string>(), It.IsAny<GeminiMessageRequest>()))
                             .ReturnsAsync(expectedResponse);

            var response = await _client.TextPrompt(message);

            Assert.NotNull(response);
            Assert.Equal(expectedResponse, response);
        }

        [Fact]
        public async Task CountTokens_WithSingleMessage_ReturnsResponse()
        {
            var message = "Test message";
            var expectedResponse = new GeminiCountTokenMessageResponse();
            _apiRequesterMock.Setup(r => r.PostAsync<GeminiCountTokenMessageResponse>(It.IsAny<string>(), It.IsAny<GeminiMessageRequest>()))
                             .ReturnsAsync(expectedResponse);

            var response = await _client.CountTokens(message);

            Assert.NotNull(response);
            Assert.Equal(expectedResponse, response);
        }

        [Fact]
        public async Task CountTokens_EmptyMessage_ThrowsArgumentException()
        {
            var message = "";

            await Assert.ThrowsAsync<ArgumentException>(() => _client.CountTokens(message));
        }

        [Fact]
        public async Task CountTokens_NullMessage_ThrowsArgumentException()
        {
            string? message = null;

            await Assert.ThrowsAsync<ArgumentException>(() => _client.CountTokens(message));
        }

        [Fact]
        public async Task ImagePrompt_WithValidMessageAndImage_ReturnsResponse()
        {
            var message = "Test message";
            var image = new byte[] { 0x12, 0x34, 0x56 };
            var mimeType = ImageMimeType.Jpeg;
            var expectedResponse = new GeminiMessageResponse();
            _apiRequesterMock.Setup(r => r.PostAsync<GeminiMessageResponse>(It.IsAny<string>(), It.IsAny<GeminiMessageRequest>()))
                             .ReturnsAsync(expectedResponse);

            var response = await _client.ImagePrompt(message, image, mimeType);

            Assert.NotNull(response);
            Assert.Equal(expectedResponse, response);
        }

        [Fact]
        public async Task ImagePrompt_WithEmptyMessage_ThrowsArgumentException()
        {
            var message = "";
            var image = new byte[] { 0x12, 0x34, 0x56 };
            var mimeType = ImageMimeType.Jpeg;

            await Assert.ThrowsAsync<ArgumentException>(() => _client.ImagePrompt(message, image, mimeType));
        }

        [Fact]
        public async Task ImagePrompt_WithEmptyImage_ThrowsArgumentException()
        {
            const string? message = "Test message";
            var image = Array.Empty<byte>();
            const ImageMimeType mimeType = ImageMimeType.Jpeg;

            await Assert.ThrowsAsync<ArgumentException>(() => _client.ImagePrompt(message, image, mimeType));
        }

        [Fact]
        public async Task ImagePrompt_WithNullMessage_ThrowsArgumentException()
        {
            string? message = null;
            var image = new byte[] { 0x12, 0x34, 0x56 };
            var mimeType = ImageMimeType.Jpeg;

            await Assert.ThrowsAsync<ArgumentException>(() => _client.ImagePrompt(message, image, mimeType));
        }

        [Fact]
        public async Task GetModel_WithValidModelName_ReturnsResponse()
        {
            var modelName = "TestModel";
            var expectedResponse = new GeminiModelResponse();
            _apiRequesterMock.Setup(r => r.GetAsync<GeminiModelResponse>(It.IsAny<string>()))
                             .ReturnsAsync(expectedResponse);

            var response = await _client.GetModel(modelName);

            Assert.NotNull(response);
            Assert.Equal(expectedResponse, response);
        }

        [Fact]
        public async Task GetModel_WithEmptyModelName_ThrowsArgumentException()
        {
            var modelName = "";

            await Assert.ThrowsAsync<ArgumentException>(() => _client.GetModel(modelName));
        }

        [Fact]
        public async Task GetModel_WithNullModelName_ThrowsArgumentException()
        {
            string? modelName = null;

            await Assert.ThrowsAsync<ArgumentException>(() => _client.GetModel(modelName));
        }
        
        [Fact]
        public async Task GetModels_ReturnsResponse()
        {
            var expectedResponse = new RootGeminiModelResponse();
            _apiRequesterMock.Setup(r => r.GetAsync<RootGeminiModelResponse>(It.IsAny<string>()))
                             .ReturnsAsync(expectedResponse);

            var response = await _client.GetModels();

            Assert.NotNull(response);
            Assert.Equal(expectedResponse, response);
        }
    }
}

