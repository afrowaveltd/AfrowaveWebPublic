using System.Text.RegularExpressions;

namespace SharedTools.Services.MdServices;

/// <summary>
/// This class provides methods to validate Markdown content.
/// </summary>
public static class MdValidator
{
	private static readonly Regex HtmlTagPattern = new(@"<[^>]+>", RegexOptions.Compiled);
	private static readonly Regex HtmlEntityPattern = new(@"&[a-z]+;", RegexOptions.Compiled);
	private static readonly Regex InlineStylePattern = new(@"style\s*=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
	private static readonly Regex DangerousTagsPattern = new(@"<(script|iframe|embed|object)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

	/// <summary>
	/// Determines if the input contains patterns that indicate it may be raw or unsafe HTML.
	/// </summary>
	public static bool ContainsHtmlArtifacts(string input)
	{
		return HtmlTagPattern.IsMatch(input)
			 || HtmlEntityPattern.IsMatch(input)
			 || InlineStylePattern.IsMatch(input)
			 || DangerousTagsPattern.IsMatch(input);
	}
}