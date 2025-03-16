using Moq;
using Moq.Protected;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SharedTools.Services;
using SharedTools.Models;
using Xunit;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Text;
using System.Reflection;

namespace SharedTools.Tests.Services
{
    public class TranslatorServiceTests : TranslatorTestBase
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _mockHttpClient;

        public TranslatorServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpClient = new HttpClient(_mockHttpMessageHandler.Object);

            // Inject mock HttpClient using reflection
            var translatorService = (TranslatorService)TranslatorService;
            var clientField = typeof(TranslatorService).GetField("_client",
                BindingFlags.NonPublic | BindingFlags.Instance);
            clientField?.SetValue(translatorService, _mockHttpClient);
        }

        [Fact]
        public async Task GetSupportedLanguagesAsync_Success_ReturnsLanguages()
        {
            // Arrange
            var expectedLanguages = new[] { "en", "es" };
            var responseContent = JsonSerializer.Serialize(new List<LibretranslateLanguage>
            {
                new() { Code = "en" },
                new() { Code = "es" }
            });

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString().Contains("/languages")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await TranslatorService.GetSupportedLanguagesAsync();

            // Assert
            Assert.Equal(expectedLanguages, result);
        }

        [Fact]
        public async Task TranslateAsync_Success_ReturnsTranslatedText()
        {
            // Arrange
            var expectedTranslatedText = "Hola Mundo";
            var responseContent = JsonSerializer.Serialize(new TranslateResponse
            {
                TranslatedText = expectedTranslatedText
            });

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString().Contains("/translate")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await TranslatorService.TranslateAsync("Hello World", "en", "es");

            // Assert
            Assert.True(result.Successful);
            Assert.Equal(expectedTranslatedText, result.Data);
        }

        [Fact]
        public async Task TranslateAsync_RetriesOnFailure_ReturnsSuccessAfterRetry()
        {
            // Arrange
            var expectedTranslatedText = "Hola Mundo";
            var responseContent = JsonSerializer.Serialize(new TranslateResponse
            {
                TranslatedText = expectedTranslatedText
            });

            var callCount = 0;
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() =>
                {
                    callCount++;
                    if (callCount < 3)
                    {
                        throw new HttpRequestException("Simulated error");
                    }
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                    };
                });

            // Act
            var result = await TranslatorService.TranslateAsync("Hello World", "en", "es");

            // Assert
            Assert.Equal(3, callCount);
            Assert.True(result.Successful);
        }
    }
}