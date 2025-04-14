using SharedTools.Models.MdModels;

namespace Id.Tests.SharedTools.Tests.Services.MdServices
{
	/// <summary>
	/// Tests for the MdToHtmlConverter class, verifying Markdown to HTML conversion with various options. Includes checks
	/// for pretty printing and handling of empty input.
	/// </summary>
	public class MdToHtmlConverterTests
	{
		private readonly MdToHtmlConverter _converter = new();

		/// <summary>
		/// Tests the Convert method to ensure it wraps the output in pre tags and formats it with pretty printing. Verifies
		/// the output starts and ends with the correct tags.
		/// </summary>
		[Fact]
		public void Convert_ShouldWrapWithPreTag_AndPrettyPrint()
		{
			// Arrange
			string md = "# Hello **World**";
			MdConversionOptions options = new() { PrettyPrint = true };

			// Act
			string result = _converter.Convert(md, options);

			// Assert
			Assert.StartsWith("<pre>", result);
			Assert.EndsWith("</pre>\n", result);
			Assert.Contains("Hello", result);
		}

		/// <summary>
		/// Tests that the conversion of markdown to another format does not include a newline at the end when pretty printing
		/// is disabled.
		/// </summary>
		[Fact]
		public void Convert_ShouldNotIncludeNewline_WhenPrettyPrintDisabled()
		{
			// Arrange
			string md = "# Header";
			MdConversionOptions options = new() { PrettyPrint = false };

			// Act
			string result = _converter.Convert(md, options);

			// Assert
			Assert.False(result.EndsWith("\n"));
		}

		/// <summary>
		/// Tests the StreamConvertAsync method to ensure it returns chunks for each line of markdown input.
		/// </summary>
		/// <returns>Returns a list of strings representing the converted markdown lines.</returns>
		[Fact]
		public async Task StreamConvertAsync_ShouldReturnChunks_ForEachLine()
		{
			// Arrange
			string md = "First line\nSecond line";
			MdConversionOptions options = new() { PrettyPrint = true };

			// Act
			List<string> chunks = await _converter.StreamConvertAsync(md, options).ToListAsync();

			// Assert
			Assert.Equal(2, chunks.Count);
			Assert.All(chunks, c => Assert.Contains("<p>", c));
			Assert.All(chunks, c => Assert.EndsWith("</p>\n", c));
		}

		/// <summary>
		/// Tests the StreamConvertAsync method to ensure it returns an empty list when provided with an empty input string.
		/// </summary>
		/// <returns>Returns an empty list of strings.</returns>
		[Fact]
		public async Task StreamConvertAsync_ShouldReturnEmpty_ForEmptyInput()
		{
			// Arrange
			string md = "";
			MdConversionOptions options = new();

			// Act
			List<string> chunks = await _converter.StreamConvertAsync(md, options).ToListAsync();

			// Assert
			Assert.Empty(chunks);
		}

		/// <summary>
		/// Tests the ConvertToHtmlAsync method to ensure it correctly converts Markdown links and images to HTML.
		/// </summary>
		/// <returns>Returns a string containing the HTML representation of the provided Markdown input.</returns>
		[Fact]
		public async Task ConvertToHtmlAsync_ShouldSupportLinksAndImages()
		{
			var mappings = new List<MdElementMapping>
	 {
		  new() { MdStart = "### ", HtmlElement = "h3" }
	 };

			var configService = Substitute.For<IMdConfigService>();
			configService.GetCombinedMappingsAsync().Returns(mappings);

			var parser = new MdParserService(configService);

			string input = "### Click [here](https://example.com) and see ![image](https://img.com/pic.png)";
			string result = await parser.ConvertToHtmlAsync(input);

			Assert.Contains(@"<h3>Click <a href=""https://example.com"">here</a> and see <img src=""https://img.com/pic.png"" alt=""image"" /></h3>", result);
		}
	}
}