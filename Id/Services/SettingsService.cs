using Id.Models.SettingsModels;

namespace Id.Services
{
	public class SettingsService : ISettingsService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly Lazy<Task<IdentificatorSettings>> _lazySettings;
		private readonly string idPath = string.Empty;
		private readonly ILogger<SettingsService> _logger;
		private readonly string settingsPath = string.Empty;

		public SettingsService(IServiceScopeFactory serviceScopeFactory, ILogger<SettingsService> logger)
		{
			_serviceScopeFactory = serviceScopeFactory;
			_lazySettings = new Lazy<Task<IdentificatorSettings>>(() => LoadSettingsAsync());
			_logger = logger;
			string projectPath = AppDomain.CurrentDomain.BaseDirectory
									  .Substring(0, AppDomain.CurrentDomain.BaseDirectory
									  .IndexOf("bin"));

			idPath = Path.Combine(projectPath, "Settings", "id.json");
			settingsPath = Path.Combine(projectPath, "Settings", "settings.json");
		}

		public Task<IdentificatorSettings> GetSettingsAsync() => _lazySettings.Value;

		private async Task<IdentificatorSettings> LoadSettingsAsync()
		{
			IdentificatorSettings settings = new();
			if(File.Exists(settingsPath))
			{
				var settingsJson = await File.ReadAllTextAsync(settingsPath);
				try
				{
					settings = JsonSerializer.Deserialize<IdentificatorSettings>(settingsJson) ?? new();
					return settings;
				}
				catch(Exception e)
				{
					_logger.LogError(e, "Error loading settings");
					return settings;
				}
			}
			return settings;
		}

		public async Task SetApplicationId(string id)
		{
			try
			{
				var settings = await GetSettingsAsync();
				settings.ApplicationId = id;
				await SetSettingsAsync(settings);
			}
			catch(Exception e)
			{
				_logger.LogError(e, "Error setting application id");
			}
		}

		public async Task SetSettingsAsync(IdentificatorSettings settings)
		{
			string json = JsonSerializer.Serialize(settings);
			try
			{
				await File.WriteAllTextAsync(settingsPath, json);
			}
			catch(Exception e)
			{
				_logger.LogError(e, "Error saving settings");
			}
		}
	}
}