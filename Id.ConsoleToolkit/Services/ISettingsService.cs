namespace Id.ConsoleToolkit.Services
{
	/// <summary>
	/// Service for loading and saving application settings.
	/// </summary>
	public interface ISettingsService
	{
		/// <summary>
		/// Current application settings.
		/// </summary>
		/// <returns>ApplicationSettings data</returns>
		ApplicationSettings Get();

		/// <summary>
		/// Loads the application settings from the settings file.
		/// </summary>
		/// <returns>ApplicationSettings data</returns>
		Task<ApplicationSettings> LoadAsync();

		/// <summary>
		/// Saves the application settings to the settings file.
		/// </summary>
		/// <param name="settings">ApplicationSettings object</param>

		Task SaveAsync(ApplicationSettings settings);
	}
}