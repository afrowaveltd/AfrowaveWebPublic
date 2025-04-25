namespace Id.Services
{
	/// <summary>
	/// ILanguagesManager interface defines methods for managing languages.
	/// </summary>
	public interface ILanguagesManager
	{
		/// <summary>
		/// Gets all languages.
		/// </summary>
		/// <returns>List of LanguageViews</returns>
		Task<ApiResponse<List<LanguageView>>> GetAllLanguagesAsync();

		/// <summary>
		/// Get all languages that can be translated.
		/// </summary>
		/// <returns>List of LanguageViews</returns>
		Task<ApiResponse<List<LanguageView>>> GetAllTranslatableLanguagesAsync();

		/// <summary>
		/// Get all details about a language.
		/// </summary>
		/// <param name="code">Two letters language code</param>
		/// <returns>The language View or 404 if language was not found</returns>
		Task<ApiResponse<LanguageView>> GetLanguageByCodeAsync(string code);
	}
}