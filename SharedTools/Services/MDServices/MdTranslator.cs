using SharedTools.Models;
using SharedTools.Models.MdModels;
using System.Text;

namespace SharedTools.Services.MdServices;

/// <summary>
/// Translates Markdown text while preserving its formatting and structure.
/// </summary>
/// <param name="translator">The translator service</param>
public class MdTranslator(ITranslatorService translator)
{
	private readonly ITranslatorService _translator = translator;

	/// <summary>
	/// Translates a Markdown-formatted text from source language to target language.
	/// Preserves structure and tags while translating actual text content.
	/// </summary>
	public async Task<ApiResponse<string>> TranslateMarkdownPreservingTagsAsync(
	 string markdown,
	 string fromLanguage,
	 string toLanguage,
	 int maxParallel = 5,
	 CancellationToken cancellationToken = default)
	{
		if(string.IsNullOrWhiteSpace(markdown))
		{
			return new ApiResponse<string>
			{
				Successful = true,
				Data = string.Empty
			};
		}

		if(MdValidator.ContainsHtmlArtifacts(markdown))
		{
			return new ApiResponse<string>
			{
				Successful = false,
				Message = "Vstup obsahuje HTML značky – očekává se čistý Markdown."
			};
		}

		string[] lines = markdown.Split('\n');
		List<string> output = new List<string>();
		List<TranslatableSegment> allSegments = new List<TranslatableSegment>();
		Dictionary<int, (int lineIndex, int tokenIndex)> segmentMap = new Dictionary<int, (int lineIndex, int tokenIndex)>();
		int segmentIndex = 0;

		// Phase 1: Tokenize lines and collect translatable segments
		List<MdLine> tokenizedLines = [];

		for(int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			MdLine mdLine = new()
			{
				LineNumber = i,
				OriginalLine = line
			};

			// TODO: později: detekce blokových tagů (#, >, atd.)
			// Prozatím prostý řádek
			List<MdToken> tokens = TokenizeLine(line);
			foreach(MdToken token in tokens)
			{
				if(token.Translate)
				{
					string[] sentences = SentenceSplitter.SplitIntoSentences(token.Text);
					foreach(string sentence in sentences)
					{
						allSegments.Add(new TranslatableSegment
						{
							Index = segmentIndex,
							Text = sentence
						});

						segmentMap[segmentIndex] = (i, mdLine.Tokens.Count);
						segmentIndex++;
					}
				}

				mdLine.Tokens.Add(token);
			}

			tokenizedLines.Add(mdLine);
		}

		// Phase 2: Translate segments in parallel batches
		string[] translations = new string[allSegments.Count];
		int batchSize = maxParallel;

		for(int i = 0; i < allSegments.Count; i += batchSize)
		{
			List<TranslatableSegment> batch = [.. allSegments.Skip(i).Take(batchSize)];
			IEnumerable<Task> tasks = batch.Select(async segment =>
			{
				ApiResponse<string> result = await _translator.TranslateAsync(segment.Text, fromLanguage, toLanguage);
				translations[segment.Index] = result.Successful ? result.Data! : segment.Text;
			});

			await Task.WhenAll(tasks);
		}

		// Phase 3: Rebuild final text
		int currentSegment = 0;

		foreach(MdLine mdLine in tokenizedLines)
		{
			List<string> rebuiltLine = [];

			foreach(MdToken token in mdLine.Tokens)
			{
				if(!token.Translate)
				{
					rebuiltLine.Add(token.Text);
				}
				else
				{
					string[] originalSentences = SentenceSplitter.SplitIntoSentences(token.Text);
					List<string> translatedParts = [];

					foreach(string _ in originalSentences)
					{
						translatedParts.Add(translations[currentSegment++]);
					}

					rebuiltLine.Add(string.Join(" ", translatedParts));
				}
			}

			output.Add(string.Join("", rebuiltLine));
		}

		return new ApiResponse<string>
		{
			Successful = true,
			Data = string.Join('\n', output)
		};
	}

	/// <summary>
	/// Tokenizes a line of Markdown text into translatable and non-translatable segments.
	/// </summary>
	/// <param name="line">The input context</param>
	/// <returns>The list of MdTokens</returns>
	public static List<MdToken> TokenizeLine(string line)
	{
		List<MdToken> tokens = [];
		StringBuilder sb = new();
		int i = 0;

		while(i < line.Length)
		{
			// --- Link: [text](url) ---
			if(line[i] == '[')
			{
				int endBracket = line.IndexOf(']', i);
				if(endBracket != -1 && endBracket + 1 < line.Length && line[endBracket + 1] == '(')
				{
					int endParen = line.IndexOf(')', endBracket + 2);
					if(endParen != -1)
					{
						if(sb.Length > 0)
						{
							tokens.Add(new MdToken { Text = sb.ToString(), Translate = true });
							_ = sb.Clear();
						}

						string linkText = line.Substring(i + 1, endBracket - i - 1);
						string url = line.Substring(endBracket + 2, endParen - endBracket - 2);

						// Translate only the visible link text
						tokens.Add(new MdToken { Text = linkText, Translate = true });

						// Add brackets and url as non-translatable
						tokens.Add(new MdToken { Text = $"({url})", Translate = false });

						i = endParen + 1;
						continue;
					}
				}
			}

			// --- Bold: **text** ---
			if(i + 1 < line.Length && line[i] == '*' && line[i + 1] == '*')
			{
				if(sb.Length > 0)
				{
					tokens.Add(new MdToken { Text = sb.ToString(), Translate = true });
					_ = sb.Clear();
				}

				int end = line.IndexOf("**", i + 2);
				if(end != -1)
				{
					string content = line.Substring(i + 2, end - i - 2);
					tokens.Add(new MdToken { Text = $"**{content}**", Translate = false });
					i = end + 2;
					continue;
				}
			}

			// --- Italic: *text* ---
			if(line[i] == '*')
			{
				if(sb.Length > 0)
				{
					tokens.Add(new MdToken { Text = sb.ToString(), Translate = true });
					_ = sb.Clear();
				}

				int end = line.IndexOf("*", i + 1);
				if(end != -1)
				{
					string content = line.Substring(i + 1, end - i - 1);
					tokens.Add(new MdToken { Text = $"*{content}*", Translate = false });
					i = end + 1;
					continue;
				}
			}

			// --- Inline code: `text` ---
			if(line[i] == '`')
			{
				if(sb.Length > 0)
				{
					tokens.Add(new MdToken { Text = sb.ToString(), Translate = true });
					_ = sb.Clear();
				}

				int end = line.IndexOf('`', i + 1);
				if(end != -1)
				{
					string content = line.Substring(i + 1, end - i - 1);
					tokens.Add(new MdToken { Text = $"`{content}`", Translate = false });
					i = end + 1;
					continue;
				}
			}

			_ = sb.Append(line[i]);
			i++;
		}

		if(sb.Length > 0)
		{
			tokens.Add(new MdToken { Text = sb.ToString(), Translate = true });
		}

		return tokens;
	}
}