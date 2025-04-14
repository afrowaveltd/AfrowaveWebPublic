namespace Id.Services;

/// <summary>
/// ITranslationFileService interface is responsible for managing translation files.
/// </summary>
public interface ITranslationFilesManager
{
	/// <summary>
	/// Returns a single translation dictionary for a specific language.
	/// </summary>
	Task<ApiResponse<Dictionary<string, string>>> GetTranslationAsync(string? applicationId, string languageCode);

	/// <summary>
	/// Returns all available translations (by language) for the given application or Locales.
	/// </summary>
	Task<ApiResponse<Dictionary<string, Dictionary<string, string>>>> GetAllTranslationsAsync(string? applicationId);

	/// <summary>
	/// Returns a list of all available languages for the given application or Locales.
	/// </summary>
	/// <param name="appId">Application Id</param>
	/// <param name="languages">List of languages to expert - if empty or null, all languages will be exported</param>
	/// <returns></returns>
	Task<ApiResponse<byte[]>> ExportTranslationsAsZipAsync(string? appId, List<string>? languages = null);
}