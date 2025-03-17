using SharedTools.Services;
using Xunit;
using FluentAssertions;
namespace Id.Tests.SharedTools.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="TextToHtmlService"/> class.
    /// </summary>
    public class TextToHtmlServiceTests
    {
        private readonly TextToHtmlService _textToHtmlService = new();

        /// <summary>
        /// Tests whether ConvertTextToHtml returns an empty string when the input is null or whitespace.
        /// </summary>
        [Fact]
        public void ConvertTextToHtml_ShouldReturnEmptyString_WhenInputIsNullOrEmpty()
        {
            _textToHtmlService.ConvertTextToHtml("").Should().BeEmpty();
            _textToHtmlService.ConvertTextToHtml("   ").Should().BeEmpty();
            _textToHtmlService.ConvertTextToHtml(null!).Should().BeEmpty();
        }

        /// <summary>
        /// Tests whether ConvertTextToHtml wraps text in paragraph tags.
        /// </summary>
        [Fact]
        public void ConvertTextToHtml_ShouldWrapTextInParagraphTags()
        {
            string input = "Hello, World!";
            string expected = "<p>Hello, World! </p>\n";

            string result = _textToHtmlService.ConvertTextToHtml(input);

            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests whether ConvertTextToHtml converts newlines to line breaks.
        /// </summary>
        [Fact]
        public void ConvertTextToHtml_ShouldConvertUnorderedList()
        {
            string input = "- Item 1\n- Item 2\n- Item 3";
            string expected = "<ul>\n<li>Item 1</li>\n<li>Item 2</li>\n<li>Item 3</li>\n</ul>\n";

            string result = _textToHtmlService.ConvertTextToHtml(input);

            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests whether ConvertTextToHtml converts headers based on leading spaces.
        /// </summary>
        [Fact]
        public void ConvertTextToHtml_ShouldConvertHeadersBasedOnLeadingSpaces()
        {
            string input = "  Header 1\n    Header 2\n      Header 3";
            string expected = "<h1>Header 1</h1>\n<h2>Header 2</h2>\n<h3>Header 3</h3>\n";

            string result = _textToHtmlService.ConvertTextToHtml(input);

            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests whether ConvertTextToHtml converts URLs to links.
        /// </summary>
        [Fact]
        public void ConvertTextToHtml_ShouldConvertUrlsToLinks()
        {
            string input = "Check out https://example.com";
            string expected = "<p>Check out <a href=\"https://example.com\" target=\"_blank\" rel=\"noopener noreferrer\">https://example.com</a> </p>\n";

            string result = _textToHtmlService.ConvertTextToHtml(input);

            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests whether ConvertTextToHtml converts emails to mailto links.
        /// </summary>
        [Fact]
        public void ConvertTextToHtml_ShouldConvertEmailsToMailtoLinks()
        {
            string input = "Contact me at test@example.com";
            string expected = "<p>Contact me at <a href=\"mailto:test@example.com\">test@example.com</a> </p>\n";

            string result = _textToHtmlService.ConvertTextToHtml(input);

            result.Should().Be(expected);
        }


        /// <summary>
        /// Tests whether ConvertTextToHtml handles complex mixed content.
        /// </summary>
        [Fact]
        public void ConvertTextToHtml_ShouldHandleComplexMixedContent()
        {
            string input = "Hello\n\n- List Item 1\n- List Item 2\n\n  Heading 1\n    Heading 2\n\nhttps://google.com";
            string expected = "<p>Hello </p>\n<ul>\n<li>List Item 1</li>\n<li>List Item 2</li>\n</ul>\n<h1>Heading 1</h1>\n<h2>Heading 2</h2>\n<p><a href=\"https://google.com\" target=\"_blank\" rel=\"noopener noreferrer\">https://google.com</a> </p>\n";

            string result = _textToHtmlService.ConvertTextToHtml(input);

            result.Should().Be(expected);
        }
    }
}
