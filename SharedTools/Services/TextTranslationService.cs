using Microsoft.Extensions.Logging;
using SharedTools.Models;
using System.Text;

namespace SharedTools.Services
{
	/// <summary>
	/// TextTranslationService is a class that is used to translate text.
	/// </summary>
	/// <param name="translator"></param>
	/// <param name="logger"></param>
	public class TextTranslationService(ITranslatorService translator, ILogger<TextTranslationService> logger) : ITextTranslationService
	{
		private readonly ITranslatorService _translator = translator;
		private readonly ILogger<TextTranslationService> _logger = logger;

		/// <summary>
		/// TranslateAndFormatAsync is a method that takes a string input and returns a string output.
		/// </summary>
		/// <param name="input">Text for translation</param>
		/// <param name="sourceLanguage">Source language code</param>
		/// <param name="targetLanguage">Target language code</param>
		/// <returns>Translated file and formated the same way as original</returns>
		public async Task<string> TranslateAndFormatAsync(string input, string sourceLanguage, string targetLanguage)
		{
			if(string.IsNullOrWhiteSpace(input))
			{
				return ""; // Return an empty string if the input is null or whitespace
			}

			StringBuilder translatedBuilder = new StringBuilder();
			string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

			foreach(string line in lines)
			{
				if(string.IsNullOrWhiteSpace(line))
				{
					// Preserve empty lines
					_ = translatedBuilder.AppendLine();
					continue;
				}

				string trimmedLine = line.TrimStart();
				int leadingSpaces = line.Length - trimmedLine.Length;

				if(line.StartsWith("\t")) // Handle lines with links defined by tabs
				{
					string processedLinkLine = await TranslateLinkLineAsync(trimmedLine, sourceLanguage, targetLanguage);
					_ = translatedBuilder.AppendLine(new string('\t', 1) + processedLinkLine);
				}
				else if(leadingSpaces >= 2) // Preserve headers
				{
					// Translate the header content
					ApiResponse<string> translatedHeader = await _translator.TranslateAsync(trimmedLine, sourceLanguage, targetLanguage);
					_ = translatedBuilder.AppendLine(new string(' ', leadingSpaces) + translatedHeader.Data);
				}
				else
				{
					// Translate regular text and preserve indentation
					ApiResponse<string> translatedParagraph = await _translator.TranslateAsync(trimmedLine, sourceLanguage, targetLanguage);
					_ = translatedBuilder.AppendLine(new string(' ', leadingSpaces) + translatedParagraph.Data);
				}
			}

			return translatedBuilder.ToString();
		}

		private async Task<string> TranslateLinkLineAsync(string line, string sourceLanguage, string targetLanguage)
		{
			// Split the line into pre-tab text and link
			string[] parts = line.Split(new[] { '\t' }, 2);
			if(parts.Length < 2)
			{
				// If there's no tab or link, just translate the line
				ApiResponse<string> translatedLine = await _translator.TranslateAsync(line, sourceLanguage, targetLanguage);
				return translatedLine.Data;
			}

			string textBeforeTab = parts[0];
			string linkPart = parts[1];

			// Look for custom link body
			int urlEndIndex = linkPart.IndexOf('[');
			if(urlEndIndex >= 0)
			{
				string url = linkPart.Substring(0, urlEndIndex).Trim();
				int linkBodyEnd = linkPart.IndexOf(']', urlEndIndex);
				if(linkBodyEnd > urlEndIndex)
				{
					string linkBody = linkPart.Substring(urlEndIndex + 1, linkBodyEnd - urlEndIndex - 1).Trim();
					string afterLink = linkPart.Substring(linkBodyEnd + 1).Trim();

					// Translate the text around the link
					ApiResponse<string> translatedTextBeforeTab = await _translator.TranslateAsync(textBeforeTab, sourceLanguage, targetLanguage);
					ApiResponse<string> translatedLinkBody = await _translator.TranslateAsync(linkBody, sourceLanguage, targetLanguage);
					ApiResponse<string> translatedAfterLink = await _translator.TranslateAsync(afterLink, sourceLanguage, targetLanguage);

					// Reconstruct the line with the translated text
					return $"{translatedTextBeforeTab.Data}\t{url}[{translatedLinkBody.Data}] {translatedAfterLink.Data}";
				}
			}

			// Default case: no custom link body
			ApiResponse<string> translatedTextOnly = await _translator.TranslateAsync(textBeforeTab, sourceLanguage, targetLanguage);
			return $"{translatedTextOnly.Data}\t{linkPart.Trim()}";
		}

		private async Task<string> ProcessLinkLineAsync(string line, string sourceLanguage, string targetLanguage)
		{
			string[] parts = line.Split(new[] { '\t' }, 2);
			if(parts.Length < 2)
			{
				return System.Web.HttpUtility.HtmlEncode(await TranslateTextAsync(line, sourceLanguage, targetLanguage)); // No tab found, return as-is
			}

			string textBeforeTab = System.Web.HttpUtility.HtmlEncode(await TranslateTextAsync(parts[0], sourceLanguage, targetLanguage));
			string linkPart = parts[1];

			int urlEndIndex = linkPart.IndexOf('['); // Look for custom link body
			if(urlEndIndex >= 0)
			{
				string url = linkPart.Substring(0, urlEndIndex).Trim();
				int linkBodyEnd = linkPart.IndexOf(']', urlEndIndex);
				if(linkBodyEnd > urlEndIndex)
				{
					string linkBody = linkPart.Substring(urlEndIndex + 1, linkBodyEnd - urlEndIndex - 1).Trim();
					string translatedBody = await TranslateTextAsync(linkBody, sourceLanguage, targetLanguage);
					string afterLink = linkPart.Substring(linkBodyEnd + 1).Trim();

					return $"{textBeforeTab}<a href=\"{System.Web.HttpUtility.HtmlEncode(url)}\" target=\"_blank\">{System.Web.HttpUtility.HtmlEncode(translatedBody)}</a> {System.Web.HttpUtility.HtmlEncode(await TranslateTextAsync(afterLink, sourceLanguage, targetLanguage))}";
				}
			}

			// Default case: no custom link body
			string urlOnly = linkPart.Trim();
			return $"{textBeforeTab}<a href=\"{System.Web.HttpUtility.HtmlEncode(urlOnly)}\" target=\"_blank\">{System.Web.HttpUtility.HtmlEncode(urlOnly)}</a>";
		}

		private async Task<string> TranslateTextAsync(string text, string sourceLanguage, string targetLanguage)
		{
			Models.ApiResponse<string> response = await _translator.TranslateAsync(text, sourceLanguage, targetLanguage);
			return response.Successful ? response.Data : text; // Fallback to the original text if translation fails
		}

		private string ProcessLineForLinks(string line)
		{
			return System.Web.HttpUtility.HtmlEncode(line);
		}

		/// <summary>
		/// TranslateFolder is a method that takes a folder path and returns a list of translated files.
		/// </summary>
		/// <param name="folderPath">Folder where to make translations</param>
		/// <param name="language">Source language</param>
		/// <returns>List of newly created files</returns>
		public async Task<ApiResponse<List<string>>> TranslateFolder(string folderPath, string language)
		{
			ApiResponse<List<string>> result = new();
			List<string> files = new();
			// get list of supported languages
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

			// check if the source language file exists
			string sourceLanguageFilePath = Path.Combine(folderPath, $"{language}.txt");
			if(!File.Exists(sourceLanguageFilePath))
			{
				_logger.LogWarning("Source language file not found: {0}", sourceLanguageFilePath);
				result.Successful = false;
				result.Message = "Source language file not found";
				return result;
			}

			// read the source language file
			string sourceText = await File.ReadAllTextAsync(sourceLanguageFilePath);
			if(string.IsNullOrWhiteSpace(sourceText))
			{
				_logger.LogWarning("Source language file is empty: {0}", sourceLanguageFilePath);
				result.Successful = false;
				result.Message = "Source language file is empty";
				return result;
			}

			// translate and format translations
			foreach(string supportedLanguage in supportedLanguages)
			{
				_logger.LogInformation("Translating {0} to {1}", language, supportedLanguage);
				if(supportedLanguage == language)
				{
					continue;
				}
				string targetLanguageFilePath = Path.Combine(folderPath, $"{supportedLanguage}.txt");
				if(File.Exists(targetLanguageFilePath))
				{
					_logger.LogWarning("Target language file already exists: {0}", targetLanguageFilePath);
					files.Add(targetLanguageFilePath);
				}
				else
				{
					string translatedText = await TranslateAndFormatAsync(sourceText, language, supportedLanguage);
					await File.WriteAllTextAsync(targetLanguageFilePath, translatedText);
					files.Add(targetLanguageFilePath);
					_logger.LogInformation("Translation saved to {0}", targetLanguageFilePath);
				}
			}

			return result;
		}

		/// <summary>
		/// TranslateFolder is a method that takes a folder path and returns a list of translated files.
		/// </summary>
		/// <param name="folderPath">Folder where to make translations - will use en.txt file</param>
		/// <returns>List of translated files</returns>
		public async Task<ApiResponse<List<string>>> TranslateFolder(string folderPath)
		{
			return await TranslateFolder(folderPath, "en");
		}
	}
}