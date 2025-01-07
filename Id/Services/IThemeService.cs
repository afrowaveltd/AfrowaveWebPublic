
namespace Id.Services
{
	public interface IThemeService
	{
		Task EnsureCompleteThemeFilesAsync();
		Task<List<string>> GetThemeNamesAsync(string? userId);
	}
}