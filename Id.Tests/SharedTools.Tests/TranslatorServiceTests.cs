using Moq.Protected;
using System.Text;

namespace Id.Tests.SharedTools.Tests
{
	public class TranslatorServiceTests : TranslatorTestBase
	{
		private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
		private readonly HttpClient _mockHttpClient;

		public TranslatorServiceTests()
		{
			_mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			_mockHttpClient = new HttpClient(_mockHttpMessageHandler.Object);

			// Replace the service registration with the mock HttpClient
			ServiceCollection services = new ServiceCollection();
			IConfigurationRoot config = new ConfigurationBuilder()
				 .AddInMemoryCollection(new Dictionary<string, string?>
				 {
				{"Translator:Host", "http://fake-translator-service"}
				 })
				 .Build();

			_ = services.AddSingleton<IConfiguration>(config);
			_ = services.AddLogging(builder => builder.AddConsole());
			_ = services.AddSingleton<HttpClient>(_mockHttpClient); // Inject mock client
			_ = services.AddTransient<ITranslatorService, TranslatorService>();

			ServiceProvider serviceProvider = services.BuildServiceProvider();
			ITranslatorService TranslatorService = serviceProvider.GetRequiredService<ITranslatorService>();
		}

		[Fact]
		public async Task GetSupportedLanguagesAsync_Success_ReturnsLanguages()
		{
			// Arrange
			string[] expectedLanguages = new[] { "en", "es" };
			string responseContent = JsonSerializer.Serialize(new List<LibretranslateLanguage>
				{
					 new() { Code = "en" },
					 new() { Code = "es" }
				});

			_ = _mockHttpMessageHandler.Protected()
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
			string[] result = await TranslatorService.GetSupportedLanguagesAsync();

			// Assert
			Assert.Equal(expectedLanguages, result);
		}

		[Fact]
		public async Task TranslateAsync_Success_ReturnsTranslatedText()
		{
			// Arrange
			string expectedTranslatedText = "Hola Mundo";
			string responseContent = JsonSerializer.Serialize(new TranslateResponse
			{
				TranslatedText = expectedTranslatedText
			});

			_ = _mockHttpMessageHandler.Protected()
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
			ApiResponse<string> result = await TranslatorService.TranslateAsync("Hello World", "en", "es");

			// Assert
			Assert.True(result.Successful);
			Assert.Equal(expectedTranslatedText, result.Data);
		}

		[Fact]
		public async Task TranslateAsync_RetriesOnFailure_ReturnsSuccessAfterRetry()
		{
			// Arrange
			string expectedTranslatedText = "Hola Mundo";
			string responseContent = JsonSerializer.Serialize(new TranslateResponse
			{
				TranslatedText = expectedTranslatedText
			});

			int callCount = 0;
			_ = _mockHttpMessageHandler.Protected()
				 .Setup<Task<HttpResponseMessage>>(
					  "SendAsync",
					  ItExpr.IsAny<HttpRequestMessage>(),
					  ItExpr.IsAny<CancellationToken>()
				 )
				 .ReturnsAsync(() =>
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
			ApiResponse<string> result = await TranslatorService.TranslateAsync("Hello World", "en", "es");

			// Assert
			Assert.Equal(3, callCount);
			Assert.True(result.Successful);
		}
	}
}