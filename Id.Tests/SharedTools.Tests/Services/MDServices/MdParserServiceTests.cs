using SharedTools.Models.MdModels;

namespace Id.Tests.SharedTools.Tests.Services.MdServices;

/// <summary>
/// Tests the conversion of basic Markdown to HTML using specified mappings. It verifies that headers and list items are
/// correctly transformed.
/// </summary>
public class MdParserServiceTests
{
	/// <summary>
	/// Converts basic Markdown input into HTML format using specified mappings for headers and list items. It processes
	/// the input asynchronously.
	/// </summary>
	/// <returns>Returns the converted HTML string.</returns>
	[Fact]
	public async Task ConvertToHtmlAsync_ShouldConvertBasicMarkdown()
	{
		// Arrange
		var mappings = new List<MdElementMapping>
		  {
				new() { MdStart = "#", HtmlElement = "h1" },
				new() { MdStart = "-", HtmlElement = "li" }
		  };

		var configService = Substitute.For<IMdConfigService>();
		configService.GetCombinedMappingsAsync().Returns(mappings);

		IMdParserService parser = new MdParserService(configService);

		string input = """
        # Welcome
        - First item
        Just text
        """;

		// Act
		string html = await parser.ConvertToHtmlAsync(input);

		// Assert
		Assert.Contains("<h1>Welcome</h1>", html);
		Assert.Contains("<li>First item</li>", html);
		Assert.Contains("<p>Just text</p>", html);
	}
}