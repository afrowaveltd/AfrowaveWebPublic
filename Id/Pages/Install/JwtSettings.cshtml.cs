namespace Id.Pages.Install
{
	public class JwtSettingsModel(ILogger<JwtSettingsModel> logger,
		ISettingsService settings,
		IStringLocalizer<JwtSettingsModel> _t,
		IInstallationStatusService status) : PageModel
	{
		private readonly ILogger<JwtSettingsModel> _logger = logger;
		private readonly ISettingsService _settingsService = settings;
		public readonly IStringLocalizer<JwtSettingsModel> t = _t;
		private readonly IInstallationStatusService _statusService = status;

		[BindProperty]
		public InputModel Input { get; set; } = new();

		public class InputModel
		{
			public string Issuer { get; set; } = string.Empty;
			public string Audience { get; set; } = string.Empty;

			public int AccessTokenExpiration { get; set; } = 30; // in minutes
			public int RefreshTokenExpiration { get; set; } = 7; // in days
		}

		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _statusService.ProperInstallState(InstalationSteps.JwtSettings))
			{
				return RedirectToPage("/");
			}
			return Page();
		}

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

			var settings = await _settingsService.GetSettingsAsync();
			settings.JwtSettings = new()
			{
				Issuer = Input.Issuer,
				Audience = Input.Audience,
				AccessTokenExpiration = Input.AccessTokenExpiration,
				RefreshTokenExpiration = Input.RefreshTokenExpiration
			};
			await _settingsService.SetSettingsAsync(settings);
			return RedirectToPage("/Install/CorsSettings");
		}
	}
}