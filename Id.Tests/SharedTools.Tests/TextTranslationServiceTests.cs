namespace Id.Tests.SharedTools.Tests;

/// <summary>
/// Unit tests for the <see cref="TextTranslationService"/> class.
/// </summary>
public class TextTranslationServiceTests
{
	private readonly ITranslatorService _translatorMock;
	private readonly ILogger<TextTranslationService> _loggerMock;
	private readonly TextTranslationService _textTranslationService;

	/// <summary>
	/// Initializes a new instance of the <see cref="TextTranslationServiceTests"/> class.
	/// </summary>
	public TextTranslationServiceTests()
	{
		_translatorMock = Substitute.For<ITranslatorService>();
		_loggerMock = Substitute.For<ILogger<TextTranslationService>>();
		_textTranslationService = new TextTranslationService(_translatorMock, _loggerMock);
	}

	/// <summary>
	/// Tests whether TranslateAndFormatAsync returns an empty string when the input is null or whitespace.
	/// </summary>
	/// <returns>true if success</returns>
	[Fact]
	public async Task TranslateAndFormatAsync_ShouldReturnEmpty_WhenInputIsNullOrWhitespace()
	{
		string result = await _textTranslationService.TranslateAndFormatAsync("", "en", "es");
		_ = result.Should().BeEmpty();

		result = await _textTranslationService.TranslateAndFormatAsync("   ", "en", "es");
		_ = result.Should().BeEmpty();

		result = await _textTranslationService.TranslateAndFormatAsync(null!, "en", "es");
		_ = result.Should().BeEmpty();
	}

	/// <summary>
	/// Tests whether TranslateAndFormatAsync preserves empty lines.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task TranslateAndFormatAsync_ShouldPreserveFormatting()
	{
		// Arrange
		string input = "  Hello\n\n  Welcome to translation!";
		_ = _translatorMock.TranslateAsync("Hello", "en", "es")
			 .Returns(new ApiResponse<string> { Data = "Hola", Successful = true });
		_ = _translatorMock.TranslateAsync("Welcome to translation!", "en", "es")
			 .Returns(new ApiResponse<string> { Data = "¡Bienvenido a la traducción!", Successful = true });

		// Act
		string result = await _textTranslationService.TranslateAndFormatAsync(input, "en", "es");

		// Assert
		_ = result.Should().Be("  Hola\n\n  ¡Bienvenido a la traducción!\n");
	}

	/// <summary>
	/// Tests whether TranslateAndFormatAsync handles links.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task TranslateAndFormatAsync_ShouldHandleLinks()
	{
		// Arrange
		string input = "\thttps://example.com";
		_ = _translatorMock.TranslateAsync("https://example.com", "en", "es")
			 .Returns(new ApiResponse<string> { Data = "https://ejemplo.com", Successful = true });

		// Act
		string result = await _textTranslationService.TranslateAndFormatAsync(input, "en", "es");

		// Assert
		_ = result.Should().Be("\thttps://ejemplo.com\n");
	}

	/// <summary>
	/// Tests whether TranslateAndFormatAsync handles headers.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task TranslateFolder_ShouldFail_WhenLanguageNotSupported()
	{
		// Arrange
		_ = _translatorMock.GetSupportedLanguagesAsync().Returns(new string[] { "es", "fr" });

		// Act
		ApiResponse<List<string>> result = await _textTranslationService.TranslateFolder("test_folder", "de");

		// Assert
		_ = result.Successful.Should().BeFalse();
		_ = result.Message.Should().Be("Language not supported");
	}
}