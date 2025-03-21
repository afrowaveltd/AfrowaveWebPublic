using SharedTools.Models;

namespace SharedTools.Services;

/// <summary>
/// Interface for language translation services.
/// </summary>
public interface ITranslatorService
{
	/// <summary>
	/// Translates given text by auto-detecting the source language.
	/// </summary>
	/// <param name="text">The text to translate.</param>
	/// <param name="targetLanguage">The target language code (e.g., "cs", "en").</param>
	/// <returns>Translated text and additional metadata wrapped in <see cref="ApiResponse{T}"/>.</returns>
	Task<ApiResponse<TranslateResponse>> AutodetectSourceLanguageAndTranslateAsync(string text, string targetLanguage);

	/// <summary>
	/// Gets the list of supported language codes.
	/// </summary>
	/// <returns>Array of language codes supported by the translation service.</returns>
	Task<string[]> GetSupportedLanguagesAsync();

	/// <summary>
	/// Translates text from a specified source language to a target language.
	/// </summary>
	/// <param name="text">Text to be translated.</param>
	/// <param name="sourceLanguage">Language code of the source text.</param>
	/// <param name="targetLanguage">Language code of the target text.</param>
	/// <returns>Translated text wrapped in <see cref="ApiResponse{T}"/>.</returns>
	Task<ApiResponse<string>> TranslateAsync(string text, string sourceLanguage, string targetLanguage);
}