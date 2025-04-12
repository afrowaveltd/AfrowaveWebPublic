using SharedTools.Models.MdModels;

namespace Id.Tests.SharedTools.Tests.Services.MdServices;

public class MdParserServiceTests
{
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