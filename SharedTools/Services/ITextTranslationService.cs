using SharedTools.Models;

namespace SharedTools.Services
{
	/// <summary>
	/// ITextTranslationService is an interface that is used to translate text.
	/// </summary>
	public interface ITextTranslationService
	{
		/// <summary>
		/// Translate is a method that takes a string input and returns a string output.
		/// </summary>
		/// <param name="input">Original English text</param>
		/// <param name="sourceLanguage">code of the source language</param>
		/// <param name="targetLanguage">code of the target language</param>
		/// <returns></returns>
		Task<string> TranslateAndFormatAsync(string input, string sourceLanguage, string targetLanguage);

		/// <summary>
		/// TranslateFolder is a method that takes a folder path and returns a list of translated files.
		/// </summary>
		/// <param name="folderPath">Path to the folder where en.txt is presented</param>
		/// <returns>List of translated files</returns>
		Task<ApiResponse<List<string>>> TranslateFolder(string folderPath);

		/// <summary>
		/// TranslateFolder is a method that takes a folder path and returns a list of translated files.
		/// </summary>
		/// <param name="folderPath">Path to the folder where translations should be made</param>
		/// <param name="language">Source language from which to translate</param>
		/// <returns>List of translated files</returns>
		Task<ApiResponse<List<string>>> TranslateFolder(string folderPath, string language);
	}
}