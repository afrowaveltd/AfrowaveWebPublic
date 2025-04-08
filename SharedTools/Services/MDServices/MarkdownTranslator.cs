using SharedTools.Models;
using SharedTools.Models.MDModels;

namespace SharedTools.Services.MDServices;

/// <summary>
/// Translates Markdown text while preserving its formatting and structure.
/// </summary>
/// <param name="translator">The translator service</param>
public class MarkdownTranslator(ITranslatorService translator)
{
	private readonly ITranslatorService _translator = translator;

	/// <summary>
	/// Translates a Markdown-formatted text from source language to target language.
	/// Preserves structure and tags while translating actual text content.
	/// </summary>
	public async Task<string> TranslateMarkdownPreservingTagsAsync(
		 string markdown,
		 string fromLanguage,
		 string toLanguage,
		 int maxParallel = 5,
		 CancellationToken cancellationToken = default)
	{
		if(string.IsNullOrWhiteSpace(markdown))
		{
			return string.Empty;
		}

		string[] lines = markdown.Split('\n');
		List<string> output = new List<string>();
		List<TranslatableSegment> allSegments = new List<TranslatableSegment>();
		Dictionary<int, (int lineIndex, int tokenIndex)> segmentMap = new Dictionary<int, (int lineIndex, int tokenIndex)>();
		int segmentIndex = 0;

		// Phase 1: Tokenize lines and collect translatable segments
		List<MDLine> tokenizedLines = new List<MDLine>();

		for(int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			MDLine mdLine = new MDLine
			{
				LineNumber = i,
				OriginalLine = line
			};

			// TODO: později: detekce blokových tagů (#, >, atd.)
			// Prozatím prostý řádek
			List<MDToken> tokens = TokenizeLine(line);
			foreach(MDToken token in tokens)
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
			List<TranslatableSegment> batch = allSegments.Skip(i).Take(batchSize).ToList();
			IEnumerable<Task> tasks = batch.Select(async segment =>
			{
				ApiResponse<string> result = await _translator.TranslateAsync(segment.Text, fromLanguage, toLanguage);
				translations[segment.Index] = result.Successful ? result.Data! : segment.Text;
			});

			await Task.WhenAll(tasks);
		}

		// Phase 3: Rebuild final text
		int currentSegment = 0;

		foreach(MDLine mdLine in tokenizedLines)
		{
			List<string> rebuiltLine = new List<string>();

			foreach(MDToken token in mdLine.Tokens)
			{
				if(!token.Translate)
				{
					rebuiltLine.Add(token.Text);
				}
				else
				{
					string[] originalSentences = SentenceSplitter.SplitIntoSentences(token.Text);
					List<string> translatedParts = new List<string>();

					foreach(string _ in originalSentences)
					{
						translatedParts.Add(translations[currentSegment++]);
					}

					rebuiltLine.Add(string.Join(" ", translatedParts));
				}
			}

			output.Add(string.Join("", rebuiltLine));
		}

		return string.Join('\n', output);
	}

	private static List<MDToken> TokenizeLine(string line)
	{
		// Jednoduchá verze: celý řádek je překladatelný
		return [new MDToken { Text = line, Translate = true }];
	}
}