using SharedTools.Models.MdModels;

namespace Id.Tests.SharedTools.Tests.Services.MdServices;

/// <summary>
/// Unit tests for the <see cref="MdTranslator"/> class.
/// </summary>
public class MdTokenTests
{
	/// <summary>
	/// Tests the TokenizeLine method to ensure it correctly splits a line into tokens.
	/// </summary>
	[Fact]
	public void TokenizeLine_ShouldSplitBoldItalicAndCode()
	{
		// Arrange
		string input = "This is **bold** and *italic* and `code`.";

		// Act
		List<MdToken> tokens = MdTranslator.TokenizeLine(input);

		// Assert
		Assert.Equal(7, tokens.Count);

		Assert.Equal("This is ", tokens[0].Text);
		Assert.True(tokens[0].Translate);

		Assert.Equal("**bold**", tokens[1].Text);
		Assert.False(tokens[1].Translate);

		Assert.Equal(" and ", tokens[2].Text);
		Assert.True(tokens[2].Translate);

		Assert.Equal("*italic*", tokens[3].Text);
		Assert.False(tokens[3].Translate);

		Assert.Equal(" and ", tokens[4].Text);
		Assert.True(tokens[4].Translate);

		Assert.Equal("`code`", tokens[5].Text);
		Assert.False(tokens[5].Translate);

		Assert.Equal(".", tokens[6].Text);
		Assert.True(tokens[6].Translate);
	}

	/// <summary>
	/// Tests the TokenizeLine method to ensure it correctly handles a line with a Markdown link.
	/// </summary>
	[Fact]
	public void TokenizeLine_ShouldHandleMarkdownLinkCorrectly()
	{
		// Arrange
		string input = "Click [here to learn more](https://example.com)";

		// Act
		List<MdToken> tokens = MdTranslator.TokenizeLine(input);

		// Assert
		Assert.Equal(3, tokens.Count);

		Assert.Equal("Click ", tokens[0].Text);
		Assert.True(tokens[0].Translate);

		Assert.Equal("here to learn more", tokens[1].Text);
		Assert.True(tokens[1].Translate);

		Assert.Equal("(https://example.com)", tokens[2].Text);
		Assert.False(tokens[2].Translate);
	}
}