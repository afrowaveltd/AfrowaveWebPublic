using SharedTools.Models;

namespace SharedTools.Services.MDServices;

/// <summary>
/// A fake implementation of <see cref="ITranslatorService"/> used for testing.
/// It simulates translation by returning placeholder strings.
/// </summary>
public class FakeTranslationService : ITranslatorService
{
	private int _counter = 1;
	private static readonly string[] result = ["en", "cs", "de", "fr"];

	/// <summary>
	/// Translates the given text from a specified source language to a target language.
	/// </summary>
	/// <param name="text"></param>
	/// <param name="sourceLanguage">Two letters code of the source language</param>
	/// <param name="targetLanguage">Two letters code of the target language</param>
	/// <returns>ApiResponse where Data is translated text</returns>
	public Task<ApiResponse<string>> TranslateAsync(string text, string sourceLanguage, string targetLanguage)
	{
		return Task.FromResult(new ApiResponse<string>
		{
			Successful = true,
			Data = $"překlad{_counter++}"
		});
	}

	/// <summary>
	/// Translates the given text by auto-detecting the source language.
	/// </summary>
	/// <param name="text">Text to translate</param>
	/// <param name="targetLanguage">Two letters code of the target language</param>
	/// <returns></returns>
	public Task<ApiResponse<TranslateResponse>> AutodetectSourceLanguageAndTranslateAsync(string text, string targetLanguage)
	{
		return Task.FromResult(new ApiResponse<TranslateResponse>
		{
			Successful = true,
			Data = new TranslateResponse { TranslatedText = $"překlad{_counter++}" }
		});
	}

	/// <summary>
	/// Gets the list of supported language codes.
	/// </summary>
	/// <returns></returns>

	public Task<string[]> GetSupportedLanguagesAsync()
	{
		return Task.FromResult(result);
	}
}