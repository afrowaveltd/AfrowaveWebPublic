using SharedTools.Models.MdModels;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedTools.Services.MdServices;

public class MdParserService : IMdParserService
{
	private readonly IMdConfigService _configService;
	private List<MdElementMapping>? _mappings;

	public MdParserService(IMdConfigService configService)
	{
		_configService = configService;
	}

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