using SharedTools.Models.MdModels;
using System.Runtime.CompilerServices;

namespace SharedTools.Services.MdServices
{
	/// <summary>
	/// A simple implementation of <see cref="IMdToHtmlConverter"/> that converts Markdown to HTML.
	/// </summary>
	public class MdToHtmlConverter : IMdToHtmlConverter
	{
		/// <summary>
		/// Converts the given Markdown string to HTML.
		/// </summary>
		/// <param name="markdown">MD text</param>
		/// <param name="options">Classes to be added to the result</param>
		/// <returns>HTML string</returns>
		public string Convert(string markdown, MdConversionOptions? options = null)
		{
			_ = options ?? new MdConversionOptions();
			return $"<pre>{System.Net.WebUtility.HtmlEncode(markdown)}</pre>{(options.PrettyPrint ? "\n" : string.Empty)}";
		}

		/// <summary>
		/// Converts the given Markdown string to HTML asynchronously.
		/// </summary>
		/// <param name="markdown">MD text</param>
		/// <param name="options">Classes to be added to the result</param>
		/// <param name="cancellationToken">The cancellation token</param>
		/// <returns>HTML stream</returns>
		public async IAsyncEnumerable<string> StreamConvertAsync(string markdown, MdConversionOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			_ = options ?? new MdConversionOptions();

			using StringReader reader = new StringReader(markdown);
			string? line;
			while((line = await reader.ReadLineAsync()) != null)
			{
				cancellationToken.ThrowIfCancellationRequested();
				yield return $"<p>{System.Net.WebUtility.HtmlEncode(line)}</p>{(options.PrettyPrint ? "\n" : string.Empty)}";
				await Task.Delay(50, cancellationToken);
			}
		}
	}
}