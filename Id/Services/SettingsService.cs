using Id.Models.SettingsModels;

namespace Id.Services
{
	/// <summary>
	/// Service to handle settings.
	/// </summary>
	public class SettingsService : ISettingsService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly Lazy<Task<IdentificatorSettings>> _lazySettings;
		private readonly string idPath = string.Empty;
		private readonly ILogger<SettingsService> _logger;
		private readonly string settingsPath = string.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="SettingsService"/> class.
		/// </summary>
		/// <param name="serviceScopeFactory">Service scope factory</param>
		/// <param name="logger">Logger service</param>
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

		/// <summary>
		/// Get the application ID.
		/// </summary>
		/// <returns>Application ID</returns>
		public async Task<string> GetApplicationIdAsync()
		{
			IdentificatorSettings settings = await GetSettingsAsync();
			return settings.ApplicationId;
		}

		/// <summary>
		/// Get the settings.
		/// </summary>
		/// <returns>IdentificatorSettings</returns>
		public Task<IdentificatorSettings> GetSettingsAsync() => _lazySettings.Value;

		/// <summary>
		/// Get the login rules.
		/// </summary>
		/// <returns>LoginRules</returns>
		public async Task<LoginRules> GetLoginRulesAsync()
		{
			IdentificatorSettings settings = await GetSettingsAsync();
			return settings.LoginRules;
		}

		/// <summary>
		/// Get the password rules.
		/// </summary>
		/// <returns>PasswordRules</returns>
		public async Task<PasswordRules> GetPasswordRulesAsync()
		{
			IdentificatorSettings settings = await GetSettingsAsync();
			return settings.PasswordRules;
		}

		/// <summary>
		/// Get the cookie settings.
		/// </summary>
		/// <returns>CookieSetting</returns>
		public async Task<CookieSettings> GetCookieSettingsAsync()
		{
			IdentificatorSettings settings = await GetSettingsAsync();
			return settings.CookieSettings;
		}

		/// <summary>
		/// Get the JWT settings.
		/// </summary>
		/// <returns>JwtSettings</returns>
		public async Task<JwtSettings> GetJwtSettingsAsync()
		{
			IdentificatorSettings settings = await GetSettingsAsync();
			return settings.JwtSettings;
		}

		/// <summary>
		/// Get the CORS settings.
		/// </summary>
		/// <returns>CorsSettings</returns>
		public async Task<CorsSettings> GetCorsSettingsAsync()
		{
			IdentificatorSettings settings = await GetSettingsAsync();
			return settings.CorsSettings;
		}

		private async Task<IdentificatorSettings> LoadSettingsAsync()
		{
			IdentificatorSettings settings = new();
			if(File.Exists(settingsPath))
			{
				string settingsJson = await File.ReadAllTextAsync(settingsPath);
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

		/// <summary>
		/// Set the application ID.
		/// </summary>
		/// <param name="id">New application ID for the authenticator</param>
		/// <returns></returns>
		public async Task SetApplicationId(string id)
		{
			try
			{
				IdentificatorSettings settings = await GetSettingsAsync();
				settings.ApplicationId = id;
				await SetSettingsAsync(settings);
			}
			catch(Exception e)
			{
				_logger.LogError(e, "Error setting application id");
			}
		}

		/// <summary>
		/// Set the settings.
		/// </summary>
		/// <param name="settings">IdentificatorSettings</param>
		/// <returns></returns>
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