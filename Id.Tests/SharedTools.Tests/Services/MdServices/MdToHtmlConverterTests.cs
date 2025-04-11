using SharedTools.Models.MdModels;

namespace Id.Tests.SharedTools.Tests.Services.MdServices
{
	public class MdToHtmlConverterTests
	{
		private readonly MdToHtmlConverter _converter = new();

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
	}
}