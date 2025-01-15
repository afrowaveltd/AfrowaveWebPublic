
namespace Id.Services
{
	public interface IThemeService
	{
		Task EnsureCompleteThemeFilesAsync();
		string GetCssFolderPath();
		Task<List<string>> GetThemeNamesAsync(string? userId);
	}
}