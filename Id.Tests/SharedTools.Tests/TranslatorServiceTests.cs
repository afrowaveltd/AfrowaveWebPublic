using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace Id.Tests.SharedTools.Tests;

/// <summary>
/// Unit tests for the <see cref="TranslatorService"/> class.
/// These tests verify the behavior of the translation service, ensuring correct
/// responses and handling of failures.
/// </summary>
public class TranslatorServiceTests
{
	private readonly HttpClient _httpClient;
	private readonly IOptions<Translator> _options;
	private readonly ITranslatorService _translatorService;
	private readonly HttpMessageHandler _mockHttpMessageHandler;
	private readonly ILogger<TranslatorService> _mockLogger;
	private readonly IConfiguration _mockConfig;

	public TranslatorServiceTests()
	{
		_mockHttpMessageHandler = Substitute.For<HttpMessageHandler>();
		_mockLogger = Substitute.For<ILogger<TranslatorService>>();
		_mockConfig = Substitute.For<IConfiguration>();

		_httpClient = new HttpClient(new MockHttpMessageHandler(_mockHttpMessageHandler))
		{
			BaseAddress = new Uri("https://translate.afrowave.ltd")
		};

		_options = Options.Create(new Translator
		{
			Host = "https://translate.afrowave.ltd",
			Port = 443
		});
		_translatorService = new TranslatorService(_mockConfig, _mockLogger);
	}

	[Fact]
	public async Task GetSupportedLanguagesAsync_Success_ReturnsLanguages()
	{
		// Arrange
		string[] expectedLanguages = { "en", "es" };
		string responseContent = JsonSerializer.Serialize(
			 new List<LibretranslateLanguage>
			 {
					 new() { Code = "en" },
					 new() { Code = "es" }
			 }
		);

		_ = _mockHttpMessageHandler
			 .SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
			 .Returns(Task.FromResult(new HttpResponseMessage
			 {
				 StatusCode = HttpStatusCode.OK,
				 Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
			 }));

		// Act
		string[] result = await _translatorService.GetSupportedLanguagesAsync();

		// Assert
		Assert.Equal(expectedLanguages, result);
	}

	[Fact]
	public async Task TranslateAsync_RetriesOnFailure_ReturnsSuccessAfterRetry()
	{
		// Arrange
		int callCount = 0;
		string responseContent = JsonSerializer.Serialize(new TranslateResponse { TranslatedText = "Hola Mundo" });

		_ = _mockHttpMessageHandler
			 .SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
			 .Returns(async callInfo =>
			 {
				 callCount++;
				 if(callCount < 3)
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
		ApiResponse<string> result = await _translatorService.TranslateAsync("Hello", "en", "es");

		// Assert
		Assert.Equal(3, callCount);
		Assert.True(result.Successful);
	}

	[Fact]
	public async Task AutodetectSourceLanguageAndTranslateAsync_Success_ReturnsTranslation()
	{
		// Arrange
		string expectedTranslatedText = "Hola Mundo";
		string responseContent = JsonSerializer.Serialize(new TranslateResponse { TranslatedText = expectedTranslatedText });

		_ = _mockHttpMessageHandler
			 .SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
			 .Returns(Task.FromResult(new HttpResponseMessage
			 {
				 StatusCode = HttpStatusCode.OK,
				 Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
			 }));

		// Act
		ApiResponse<TranslateResponse> result = await _translatorService.AutodetectSourceLanguageAndTranslateAsync("Hello World", "es");

		// Assert
		Assert.True(result.Successful);
		Assert.Equal(expectedTranslatedText, result.Data.TranslatedText);
	}

	private class MockHttpMessageHandler : HttpMessageHandler
	{
		private readonly HttpMessageHandler _innerHandler;

		public MockHttpMessageHandler(HttpMessageHandler innerHandler)
		{
			_innerHandler = innerHandler;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return _innerHandler.SendAsync(request, cancellationToken);
		}
	}
}