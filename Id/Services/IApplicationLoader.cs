namespace Id.Services
{
	/// <summary>
	/// Interface for the application loader
	/// </summary>
	public interface IApplicationLoader
	{
		/// <summary>
		/// Apply migrations
		/// </summary>
		/// <returns></returns>
		Task ApplyMigrations();

		/// <summary>
		/// Gets the supported cultures
		/// </summary>
		/// <returns>Array of supported cultures</returns>
		string[] GetSupportedCultures();

		/// <summary>
		/// Seed the database with countries
		/// </summary>
		/// <returns></returns>
		Task SeedCountriesAsync();

		/// <summary>
		/// Seed the database with languages
		/// </summary>
		/// <returns></returns>
		Task SeedLanguagesAsync();

		/// <summary>
		/// Translates the language names to all supported languages
		/// </summary>
		/// <returns></returns>
		Task TranslateLanguageNamesAsync();

		/// <summary>
		/// Translates the static assets
		/// </summary>
		/// <returns></returns>
		Task<List<ApiResponse<List<string>>>> TranslateStaticAssetsAsync();
	}
}