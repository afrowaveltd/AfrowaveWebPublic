using Microsoft.Extensions.Logging;
using SharedTools.Models;
using System.Text;
using System.Web;

namespace SharedTools.Services
{
	/// <summary>
	/// Service responsible for translating and formatting text while preserving structure.
	/// </summary>
	public class TextTranslationService(ITranslatorService translator, ILogger<TextTranslationService> logger) : ITextTranslationService
	{
		private readonly ITranslatorService _translator = translator;
		private readonly ILogger<TextTranslationService> _logger = logger;

		/// <summary>
		/// Translates and formats the given text while preserving indentation and spacing.
		/// </summary>
		public async Task<string> TranslateAndFormatAsync(string input, string sourceLanguage, string targetLanguage)
		{
			if(string.IsNullOrWhiteSpace(input))
			{
				return "";
			}

			StringBuilder translatedBuilder = new StringBuilder();
			string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

			foreach(string line in lines)
			{
				if(string.IsNullOrWhiteSpace(line))
				{
					_ = translatedBuilder.Append('\n');
					continue;
				}

				string trimmedLine = line.TrimStart();
				int leadingSpaces = line.Length - trimmedLine.Length;

				if(line.StartsWith("\t"))
				{
					string processedLinkLine = await TranslateLinkLineAsync(trimmedLine, sourceLanguage, targetLanguage);
					_ = translatedBuilder.Append('\t').Append(processedLinkLine).Append('\n');
				}
				else if(leadingSpaces >= 2)
				{
					ApiResponse<string> translatedHeader = await _translator.TranslateAsync(trimmedLine, sourceLanguage, targetLanguage);
					_ = translatedBuilder.Append(new string(' ', leadingSpaces)).Append(translatedHeader.Data).Append('\n');
				}
				else
				{
					ApiResponse<string> translatedParagraph = await _translator.TranslateAsync(trimmedLine, sourceLanguage, targetLanguage);
					_ = translatedBuilder.Append(new string(' ', leadingSpaces)).Append(translatedParagraph.Data).Append('\n');
				}
			}

			return translatedBuilder.ToString();
		}

		/// <summary>
		/// Translates a line containing a link while preserving its structure.
		/// </summary>
		private async Task<string> TranslateLinkLineAsync(string line, string sourceLanguage, string targetLanguage)
		{
			string[] parts = line.Split(new[] { '\t' }, 2);
			if(parts.Length < 2)
			{
				ApiResponse<string> translatedLine = await _translator.TranslateAsync(line, sourceLanguage, targetLanguage);
				return translatedLine.Data ?? string.Empty;
			}

			string textBeforeTab = parts[0];
			string linkPart = parts[1];
			int urlEndIndex = linkPart.IndexOf('[');

			if(urlEndIndex >= 0)
			{
				string url = linkPart.Substring(0, urlEndIndex).Trim();
				int linkBodyEnd = linkPart.IndexOf(']', urlEndIndex);
				if(linkBodyEnd > urlEndIndex)
				{
					string linkBody = linkPart.Substring(urlEndIndex + 1, linkBodyEnd - urlEndIndex - 1).Trim();
					string afterLink = linkPart.Substring(linkBodyEnd + 1).Trim();

					ApiResponse<string> translatedTextBeforeTab = await _translator.TranslateAsync(textBeforeTab, sourceLanguage, targetLanguage);
					ApiResponse<string> translatedLinkBody = await _translator.TranslateAsync(linkBody, sourceLanguage, targetLanguage);
					ApiResponse<string> translatedAfterLink = await _translator.TranslateAsync(afterLink, sourceLanguage, targetLanguage);

					return $"{translatedTextBeforeTab.Data}\t{url}[{translatedLinkBody.Data}] {translatedAfterLink.Data}";
				}
			}

			ApiResponse<string> translatedTextOnly = await _translator.TranslateAsync(textBeforeTab, sourceLanguage, targetLanguage);
			return $"{translatedTextOnly.Data}\t{linkPart.Trim()}";
		}

		/// <summary>
		/// Translates a given text segment.
		/// </summary>
		private async Task<string> TranslateTextAsync(string text, string sourceLanguage, string targetLanguage)
		{
			ApiResponse<string> response = await _translator.TranslateAsync(text, sourceLanguage, targetLanguage);
			return response.Successful ? response.Data ?? string.Empty : text ?? string.Empty;
		}

		/// <summary>
		/// Encodes a line to prevent HTML injection.
		/// </summary>
		private string ProcessLineForLinks(string line)
		{
			return HttpUtility.HtmlEncode(line);
		}

		/// <summary>
		/// Translates all text files in a folder from English.
		/// </summary>
		public async Task<ApiResponse<List<string>>> TranslateFolder(string folderPath)
		{
			return await TranslateFolder(folderPath, "en");
		}

		/// <summary>
		/// Translates all text files in a folder from a given source language.
		/// </summary>
		public async Task<ApiResponse<List<string>>> TranslateFolder(string folderPath, string language)
		{
			ApiResponse<List<string>> result = new();
			List<string> files = new();
			string[] supportedLanguages = await _translator.GetSupportedLanguagesAsync();
			if(!supportedLanguages.Contains(language))
			{
				_logger.LogWarning("Language not supported: {0}", language);
				result.Successful = false;
				result.Message = "Language not supported";
				return result;
			}

			if(!Directory.Exists(folderPath))
			{
				_logger.LogWarning("Folder not found: {0}", folderPath);
				result.Successful = false;
				result.Message = "Folder not found";
				return result;
			}

			string sourceLanguageFilePath = Path.Combine(folderPath, $"{language}.txt");
			if(!File.Exists(sourceLanguageFilePath))
			{
				_logger.LogWarning("Source language file not found: {0}", sourceLanguageFilePath);
				result.Successful = false;
				result.Message = "Source language file not found";
				return result;
			}

			string sourceText = await File.ReadAllTextAsync(sourceLanguageFilePath);
			if(string.IsNullOrWhiteSpace(sourceText))
			{
				_logger.LogWarning("Source language file is empty: {0}", sourceLanguageFilePath);
				result.Successful = false;
				result.Message = "Source language file is empty";
				return result;
			}

			foreach(string supportedLanguage in supportedLanguages)
			{
				if(supportedLanguage == language)
				{
					continue;
				}
				string targetLanguageFilePath = Path.Combine(folderPath, $"{supportedLanguage}.txt");
				if(!File.Exists(targetLanguageFilePath))
				{
					string translatedText = await TranslateAndFormatAsync(sourceText, language, supportedLanguage);
					await File.WriteAllTextAsync(targetLanguageFilePath, translatedText);
					files.Add(targetLanguageFilePath);
				}
			}
			result.Successful = true;
			result.Data = files;
			return result;
		}
	}
}