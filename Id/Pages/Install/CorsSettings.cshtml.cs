using Id.Models.SettingsModels;

namespace Id.Pages.Install
{
	public class CorsSettingsModel(ILogger<CorsSettingsModel> logger,
		 ISettingsService settings,
		 IStringLocalizer<CorsSettingsModel> _t,
		 IInstallationStatusService status) : PageModel
	{
		private readonly ILogger<CorsSettingsModel> _logger = logger;
		private readonly ISettingsService _settingsService = settings;
		public readonly IStringLocalizer<CorsSettingsModel> t = _t;
		private readonly IInstallationStatusService _statusService = status;

		[BindProperty]
		public InputModel Input { get; set; } = new();

		public class InputModel
		{
			public CorsPolicyMode PolicyMode { get; set; } = CorsPolicyMode.AllowAll;
			public List<string> AllowedOrigins { get; set; } = new();
			public List<string> AllowedMethods { get; set; } = new();
			public List<string> AllowedHeaders { get; set; } = new();
			public bool AllowCredentials { get; set; } = false;
			public bool CorsConfigured { get; set; } = false;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _statusService.ProperInstallState(InstalationSteps.CorsSettings))
			{
				return RedirectToPage("/");
			}

			var settings = await _settingsService.GetSettingsAsync();
			var cors = settings.CorsSettings ?? new CorsSettings();

			Input = new InputModel
			{
				PolicyMode = cors.PolicyMode,
				AllowedOrigins = cors.AllowedOrigins,
				AllowedMethods = cors.AllowedMethods,
				AllowedHeaders = cors.AllowedHeaders,
				AllowCredentials = cors.AllowCredentials,
				CorsConfigured = cors.CorsConfigured
			};

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if(!await _statusService.ProperInstallState(InstalationSteps.CorsSettings))
			{
				return RedirectToPage("/");
			}
			if(!ModelState.IsValid)
			{
				return Page();
			}

			var settings = await _settingsService.GetSettingsAsync();
			settings.CorsSettings = new CorsSettings
			{
				PolicyMode = Input.PolicyMode,
				AllowedOrigins = Input.AllowedOrigins,
				AllowedMethods = Input.AllowedMethods,
				AllowedHeaders = Input.AllowedHeaders,
				AllowCredentials = Input.AllowCredentials,
				CorsConfigured = Input.CorsConfigured
			};

			await _settingsService.SetSettingsAsync(settings);
			return RedirectToPage("/Install/NextStep"); // Adjust for next step
		}
	}
}