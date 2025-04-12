using SharedTools.Models.MdModels;

namespace SharedTools.Services.MdServices
{
	/// <summary>
	/// Converts markdown text to HTML format. Supports both synchronous and asynchronous conversion with optional
	/// settings.
	/// </summary>
	public interface IMdToHtmlConverter
	{
		/// <summary>
		/// Converts a markdown string into a different format based on specified options.
		/// </summary>
		/// <param name="markdown">The input string formatted in markdown that needs to be converted.</param>
		/// <param name="options">Optional settings that influence the conversion process.</param>
		/// <returns>The converted string resulting from the markdown input.</returns>
		string Convert(string markdown, MdConversionOptions? options = null);

		/// <summary>
		/// Converts markdown text to a stream of strings asynchronously.
		/// </summary>
		/// <param name="markdown">The input text formatted in markdown that needs to be converted.</param>
		/// <param name="options">Optional settings that modify the conversion process.</param>
		/// <param name="cancellationToken">Used to signal if the operation should be canceled.</param>
		/// <returns>An asynchronous enumerable collection of converted strings.</returns>
		IAsyncEnumerable<string> StreamConvertAsync(string markdown, MdConversionOptions? options = null, CancellationToken cancellationToken = default);
	}
}