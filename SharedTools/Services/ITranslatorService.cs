using SharedTools.Models;
using System.Text.Json;

namespace SharedTools.Services
{
	/// <summary>
	/// ITranslatorService is an interface that is used to translate text.
	/// </summary>
	public interface ITranslatorService
	{
		JsonSerializerOptions Options { get; }

		/// <summary>
		/// AutodetectSourceLanguageAndTranslateAsync is a method that takes a string input and returns a translated string output.
		/// </summary>
		/// <param name="text">Text for translation</param>
		/// <param name="targetLanguage">Target language code</param>
		/// <returns>TranslateResponse instance</returns>
		Task<ApiResponse<TranslateResponse>> AutodetectSourceLanguageAndTranslateAsync(string text, string targetLanguage);

		/// <summary>
		/// GetSupportedLanguagesAsync is a method that returns a list of supported languages.
		/// </summary>
		/// <returns>List of the language codes for languages supported by libre translate server</returns>
		Task<string[]> GetSupportedLanguagesAsync();

		/// <summary>
		/// TranslateAsync is a method that takes a string input and returns a translated string output.
		/// </summary>
		/// <param name="text">Text for translation</param>
		/// <param name="sourceLanguage">Input language code</param>
		/// <param name="targetLanguage">Target language code</param>
		/// <returns></returns>
		Task<ApiResponse<string>> TranslateAsync(string text, string sourceLanguage, string targetLanguage);
	}
}