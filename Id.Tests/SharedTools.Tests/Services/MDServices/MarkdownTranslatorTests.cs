using SharedTools.Services.MDServices;

namespace Id.Tests.SharedTools.Tests.Services.MDServices;

public class MarkdownTranslatorTests
{
	[Fact]
	public async Task TranslateMarkdownPreservingTagsAsync_TranslatesLines_WithFakeService()
	{
		// Arrange
		MarkdownTranslator translator = new MarkdownTranslator(new FakeTranslationService());

		string input = """
            # Welcome
            This is a **cool** app. It is blazing fast.
            - Easy to use
            - Multilingual
            """;

		// Act
		string result = await translator.TranslateMarkdownPreservingTagsAsync(input, "en", "cs");

		// Assert
		Assert.NotNull(result);
		Assert.Contains("překlad1", result);
		Assert.Contains("překlad2", result);
		Assert.Contains("překlad3", result);
		Assert.DoesNotContain("This is", result); // original text should not be present
	}
}