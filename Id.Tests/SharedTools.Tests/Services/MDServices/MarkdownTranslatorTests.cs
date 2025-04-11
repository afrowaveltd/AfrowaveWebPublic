namespace Id.Tests.SharedTools.Tests.Services.MdServices;

/// <summary>
/// Tests for the <see cref="MdTranslator"/> class.
/// </summary>
public class MarkdownTranslatorTests
{
	/// <summary>
	/// Tests the <see cref="MdTranslator.TranslateMarkdownPreservingTagsAsync(string, string, string, int, CancellationToken)"/> method.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task TranslateMarkdownPreservingTagsAsync_TranslatesLines_WithFakeService()
	{
		// Arrange
		MdTranslator translator = new MdTranslator(new FakeTranslationService());

		string input = """
            # Welcome
            This is a **cool** app. It is blazing fast.
            - Easy to use
            - Multilingual
            """;

		// Act
		ApiResponse<string> result = await translator.TranslateMarkdownPreservingTagsAsync(input, "en", "cs");

		// Assert
		Assert.NotNull(result.Data);
		Assert.Contains("překlad1", result.Data);
		Assert.Contains("překlad2", result.Data);
		Assert.Contains("překlad3", result.Data);
		Assert.DoesNotContain("This is", result.Data); // original text should not be present
	}

	/// <summary>
	/// Tests the <see cref="MdTranslator.TranslateMarkdownPreservingTagsAsync(string, string, string, int, CancellationToken)"/> method with empty input.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task TranslateMarkdown_ShouldFail_WhenHtmlDetected()
	{
		// Arrange
		string input = "<div>Not markdown</div>";
		MdTranslator translator = new(new FakeTranslationService());

		// Act
		ApiResponse<string> result = await translator.TranslateMarkdownPreservingTagsAsync(input, "en", "cs");

		// Assert
		Assert.False(result.Successful);
		Assert.Null(result.Data);
		Assert.Contains("HTML", result.Message);
	}
}