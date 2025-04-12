namespace Id.Tests.SharedTools.Tests.Services.MdServices;

/// <summary>
/// Tests the ParseInlineElements method of MdParserService for converting inline markdown syntax to HTML. It checks
/// various input cases.
/// </summary>
public class MdParserService_InlineTests
{
	/// <summary>
	/// Parses inline markdown syntax and converts it to HTML elements.
	/// </summary>
	/// <param name="input">The string containing markdown syntax to be converted into HTML.</param>
	/// <param name="expected">The string representing the expected HTML output after conversion.</param>

	[Theory]
	[InlineData("Hello **world**", "Hello <strong>world</strong>")]
	[InlineData("*italic* text", "<em>italic</em> text")]
	[InlineData("`inline` code", "<code>inline</code> code")]
	public void ParseInlineElements_ShouldConvertInlineSyntax(string input, string expected)
	{
		var dummyConfig = Substitute.For<IMdConfigService>();
		var service = new MdParserService(dummyConfig);

		string result = service.ParseInlineElements(input);

		Assert.Equal(expected, result);
	}
}