using SharedTools.Models.MdModels;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedTools.Services.MdServices;

/// <summary>
/// Converts markdown text to HTML by mapping markdown elements to HTML tags. It handles inline elements like images,
/// links, and formatting.
/// </summary>
public class MdParserService : IMdParserService
{
	private readonly IMdConfigService _configService;
	private List<MdElementMapping>? _mappings;

	/// <summary>
	/// Initializes the Markdown parser service with a configuration service for managing settings.
	/// </summary>
	/// <param name="configService">Provides the necessary configuration settings for the Markdown parser.</param>
	public MdParserService(IMdConfigService configService)
	{
		_configService = configService;
	}

	/// <summary>
	/// Converts the text to the HTML
	/// </summary>
	/// <param name="markdown"></param>
	/// <returns>Task returns the HTML string</returns>
	public async Task<string> ConvertToHtmlAsync(string markdown)
	{
		_mappings ??= await _configService.GetCombinedMappingsAsync();
		StringBuilder htmlBuilder = new();

		string[] lines = markdown.Split('\n', StringSplitOptions.None);

		foreach(string rawLine in lines)
		{
			string line = rawLine.TrimEnd();

			MdElementMapping? match = _mappings
				 .OrderByDescending(m => m.MdStart.Length)
				 .FirstOrDefault(m => line.StartsWith(m.MdStart));

			if(match is not null)
			{
				string content = line.Substring(match.MdStart.Length).Trim();

				if(match.SelfClosing)
				{
					htmlBuilder.Append($"<{match.HtmlElement}");
					if(!string.IsNullOrWhiteSpace(match.CssClass))
						htmlBuilder.Append($" class=\"{match.CssClass}\"");
					htmlBuilder.Append(" />\n");
				}
				else
				{
					string cont = line.Substring(match.MdStart.Length).Trim();
					string parsedContent = ParseInlineElements(cont);

					htmlBuilder.Append($"<{match.HtmlElement}");
					if(!string.IsNullOrWhiteSpace(match.CssClass))
						htmlBuilder.Append($" class=\"{match.CssClass}\"");
					htmlBuilder.Append($">{parsedContent}</{match.HtmlElement}>\n");
				}
			}
			else
			{
				// fallback to <p>
				htmlBuilder.Append($"<p>{line}</p>\n");
			}
		}

		return htmlBuilder.ToString();
	}

	/// <summary>
	/// Converts inline markdown elements in a string to their corresponding HTML representations.
	/// </summary>
	/// <param name="input">A string containing markdown formatted text to be parsed into HTML.</param>
	/// <returns>A string with the markdown elements replaced by HTML tags.</returns>
	internal string ParseInlineElements(string input)
	{
		if(string.IsNullOrWhiteSpace(input)) return input;

		// 1. Obrázky: ![alt](src)
		input = Regex.Replace(input, @"!\[(.*?)\]\((.+?)\)", "<img src=\"$2\" alt=\"$1\" />");

		// 2. Odkazy: [text](url)
		input = Regex.Replace(input, @"\[(.+?)\]\((.+?)\)", "<a href=\"$2\">$1</a>");

		// 3. Tučné
		input = Regex.Replace(input, @"\*\*(.+?)\*\*", "<strong>$1</strong>");

		// 4. Kurzíva
		input = Regex.Replace(input, @"\*(.+?)\*", "<em>$1</em>");

		// 5. Inline kód
		input = Regex.Replace(input, @"`(.+?)`", "<code>$1</code>");

		return input;
	}
}