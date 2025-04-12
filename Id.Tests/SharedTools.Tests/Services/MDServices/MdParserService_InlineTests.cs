namespace Id.Tests.SharedTools.Tests.Services.MdServices;

public class MdParserService_InlineTests
{
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