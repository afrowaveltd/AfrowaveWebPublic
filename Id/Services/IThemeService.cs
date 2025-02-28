namespace Id.Services
{
	/// <summary>
	/// Service to handle themes.
	/// </summary>
	public interface IThemeService
	{
		/// <summary>
		/// Ensure that the complete theme files are present.
		/// </summary>
		/// <returns></returns>
		Task EnsureCompleteThemeFilesAsync();

		/// <summary>
		/// Get the CSS file path.
		/// </summary>
		/// <returns>full path for the CSS folder</returns>
		string GetCssFolderPath();

		/// <summary>
		/// Get names of all themes.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <returns>List of names of all themes for given user</returns>
		Task<List<string>> GetThemeNamesAsync(string? userId);
	}
}