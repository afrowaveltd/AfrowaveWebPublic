using Moq.Protected;
using System.Text;

namespace Id.Tests.SharedTools.Tests
{
	public class TranslatorServiceTests : TranslatorTestBase
	{
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
				 TranslatorService.Options // ✅ Use class name, not interface
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
	}
}