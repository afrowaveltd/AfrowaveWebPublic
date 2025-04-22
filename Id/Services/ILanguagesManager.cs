
namespace Id.Services
{
	public interface ILanguagesManager
	{
		Task<ApiResponse<List<LanguageView>>> GetAllLanguagesAsync();
		Task<ApiResponse<List<LanguageView>>> GetAllTranslatableLanguagesAsync();
		Task<ApiResponse<LanguageView>> GetLanguageByCodeAsync(string code);
	}
}