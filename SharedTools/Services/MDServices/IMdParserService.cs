namespace SharedTools.Services.MdServices;

/// <summary>
/// Converts markdown text into HTML asynchronously. Returns the resulting HTML as a string.
/// </summary>
public interface IMdParserService
{
	/// <summary>
	/// Converts a markdown string into its HTML representation asynchronously.
	/// </summary>
	/// <param name="markdown">The input string formatted in markdown that needs to be converted.</param>
	/// <returns>A task that represents the asynchronous operation, containing the resulting HTML string.</returns>
	Task<string> ConvertToHtmlAsync(string markdown);
}