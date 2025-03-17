using Moq.Protected;
using System.Text;

namespace Id.Tests.SharedTools.Tests
{
	/// <summary>
	/// Unit tests for the <see cref="TranslatorService"/> class.
	/// These tests verify the behavior of the translation service, ensuring correct
	/// responses and handling of failures.
	/// </summary>
	public class TranslatorServiceTests : TranslatorTestBase
	{
		/// <summary>
		/// Tests whether the GetSupportedLanguagesAsync method successfully returns a list of supported languages.
		/// </summary>
		/// <returns>A task representing the asynchronous unit test.</returns>
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
				 },
				 TranslatorService.Options
			);

			_ = MockHttpMessageHandler.Protected()
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

		/// <summary>
		/// Tests whether the TranslateAsync method retries on failure and eventually returns a successful response.
		/// </summary>
		/// <returns>A task representing the asynchronous unit test.</returns>
		[Fact]
		public async Task TranslateAsync_RetriesOnFailure_ReturnsSuccessAfterRetry()
		{
			// Arrange
			int callCount = 0;
			string responseContent = JsonSerializer.Serialize(
				 new TranslateResponse { TranslatedText = "Hola Mundo" },
				 TranslatorService.Options
			);

			_ = MockHttpMessageHandler.Protected()
				 .Setup<Task<HttpResponseMessage>>(
					  "SendAsync",
					  ItExpr.Is<HttpRequestMessage>(req =>
						req.Method == HttpMethod.Post &&
						req.RequestUri.ToString().Contains("/translate")),
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

		/// <summary>
		/// Tests whether the AutodetectSourceLanguageAndTranslateAsync method successfully translates text with automatic language detection.
		/// </summary>
		/// <returns>A task representing the asynchronous unit test.</returns>
		[Fact]
		public async Task AutodetectSourceLanguageAndTranslateAsync_Success_ReturnsTranslation()
		{
			// Arrange
			string expectedTranslatedText = "Hola Mundo";
			string responseContent = JsonSerializer.Serialize(
				 new TranslateResponse { TranslatedText = expectedTranslatedText },
				 TranslatorService.Options
			);

			_ = MockHttpMessageHandler.Protected()
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
			ApiResponse<TranslateResponse> result = await TranslatorService.AutodetectSourceLanguageAndTranslateAsync("Hello World", "es");

			// Assert
			Assert.True(result.Successful);
			Assert.Equal(expectedTranslatedText, result.Data.TranslatedText);
		}
	}
}