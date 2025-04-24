namespace Id.ConsoleToolkit.Services
{
	/// <summary>
	/// Service for loading and saving application settings.
	/// </summary>
	public class SettingsService : ISettingsService
	{
		private static readonly string SettingsPath = Path.Combine(AppContext.BaseDirectory, "settings.json");

		/// <summary>
		/// Current application settings.
		/// </summary>
		public static ApplicationSettings Current { get; private set; } = new();

		/// <summary>
		/// Loads the application settings from the settings file.
		/// </summary>
		/// <returns>the application settings</returns>
		public async Task<ApplicationSettings> LoadAsync()
		{
			if(!File.Exists(SettingsPath))
			{
				Current = new ApplicationSettings(); // default settings
				await SaveAsync(Current);
				return Current;
			}

			string json = await File.ReadAllTextAsync(SettingsPath);
			try
			{
				Current = JsonSerializer.Deserialize<ApplicationSettings>(json) ?? new();
			}
			catch(JsonException ex)
			{
				Console.WriteLine($"Error deserializing settings: {ex.Message}");
				Current = new ApplicationSettings();
				await SaveAsync(Current);// default settings
			}
			Current = JsonSerializer.Deserialize<ApplicationSettings>(json) ?? new();
			return Current;
		}

		/// <summary>
		/// Saves the application settings to the settings file.
		/// </summary>
		/// <param name="settings"></param>
		/// <returns></returns>
		public async Task SaveAsync(ApplicationSettings settings)
		{
			string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
			await File.WriteAllTextAsync(SettingsPath, json);
			Current = settings;
		}

		public ApplicationSettings Get()
		{
			return Current;
		}
	}
}