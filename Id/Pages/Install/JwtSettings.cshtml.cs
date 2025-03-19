namespace Id.Pages.Install
{
	/// <summary>
	/// Handles JWT settings for a web page, including loading and saving configuration.
	/// </summary>
	/// <param name="logger">Used for logging information and errors related to JWT settings.</param>
	/// <param name="settings">Provides access to application settings for retrieving and saving JWT configurations.</param>
	/// <param name="_t">Facilitates localization for the JWT settings page.</param>
	/// <param name="status">Checks the installation status to ensure proper configuration before proceeding.</param>
	public class JwtSettingsModel(ILogger<JwtSettingsModel> logger,
		ISettingsService settings,
		IStringLocalizer<JwtSettingsModel> _t,
		IInstallationStatusService status) : PageModel
	{
		// Dependency Injection
		private readonly ILogger<JwtSettingsModel> _logger = logger;

		private readonly ISettingsService _settingsService = settings;
		public readonly IStringLocalizer<JwtSettingsModel> t = _t;
		private readonly IInstallationStatusService _statusService = status;

		/// <summary>
		/// Binds an InputModel property for use in a Razor page. It initializes the property with a new instance.
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; } = new();

		/// <summary>
		/// Input model for the page
		/// </summary>
		/// <permission cref="Issuer">Sets issuer of the JWT Token</permission>
		/// <permission cref="Audience">Sets audience of the JWT Token</permission>
		/// <permission cref="AccessTokenExpiration">Sets expiration time of the access token in minutes</permission>
		/// <permission cref="RefreshTokenExpiration">Sets expiration time of the refresh token in days</permission>
		public class InputModel
		{
			/// <summary>
			/// Gets or sets the issuer of the JWT Token.
			/// </summary>
			public string Issuer { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the audience of the JWT Token.
			/// </summary>
			public string Audience { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the expiration time of the access token in minutes.
			/// </summary>
			public int AccessTokenExpiration { get; set; } = 30; // in minutes

			/// <summary>
			/// Gets or sets the expiration time of the refresh token in days.
			/// </summary>
			public int RefreshTokenExpiration { get; set; } = 7; // in days
		}

		/// <summary>
		/// Get request for the page
		/// </summary>
		/// <returns>Loads the JWT settings page</returns>
		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _statusService.ProperInstallState(InstalationSteps.JwtSettings))
			{
				return RedirectToPage("/");
			}
			return Page();
		}

		/// <summary>
		/// Post request for the page
		/// </summary>
		/// <returns>Redirects to the Cors Settings page</returns>
		public async Task<IActionResult> OnPostAsync()
		{
			if(!await _statusService.ProperInstallState(InstalationSteps.JwtSettings))
			{
				return RedirectToPage("/");
			}
			if(!ModelState.IsValid)
			{
				return Page();
			}

			Models.SettingsModels.IdentificatorSettings settings = await _settingsService.GetSettingsAsync();
			settings.JwtSettings = new()
			{
				Issuer = Input.Issuer,
				Audience = Input.Audience,
				AccessTokenExpiration = Input.AccessTokenExpiration,
				RefreshTokenExpiration = Input.RefreshTokenExpiration,
				IsConfigured = true
			};
			await _settingsService.SetSettingsAsync(settings);
			return RedirectToPage("/Install/CorsSettings");
		}
	}
}