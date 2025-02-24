using Id.Models.SettingsModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Install
{
	public class CorsSettingsModel(ILogger<CorsSettingsModel> logger,
		 ISettingsService settings,
		 IStringLocalizer<CorsSettingsModel> _t,
		 IInstallationStatusService status,
		 ISelectOptionsServices selectOptions) : PageModel
	{
		private readonly ILogger<CorsSettingsModel> _logger = logger;
		private readonly ISettingsService _settingsService = settings;
		public readonly IStringLocalizer<CorsSettingsModel> t = _t;
		private readonly IInstallationStatusService _statusService = status;
		private readonly ISelectOptionsServices _selectOptions = selectOptions;
		public List<SelectListItem> HttpMethodsOptions { get; set; } = new();
		public List<SelectListItem> HttpHeadersOptions { get; set; } = new();
		public List<SelectListItem> AllowAnyOriginOptions { get; set; } = new();
		public List<SelectListItem> AllowAnyMethodOptions { get; set; } = new();
		public List<SelectListItem> AllowCredentialsOptions { get; set; } = new();
		public List<SelectListItem> AllowAnyHeaderOptions { get; set; } = new();

		[BindProperty]
		public InputModel Input { get; set; } = new();

		public class InputModel
		{
			public CorsPolicyMode PolicyMode { get; set; } = CorsPolicyMode.AllowAll;
			public List<string> AllowedOrigins { get; set; } = new();
			public bool AllowAnyMethod { get; set; } = false;
			public List<string> AllowedMethods { get; set; } = new();
			public bool AllowAnyHeader { get; set; } = false;
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

			HttpMethodsOptions = await _selectOptions.GetHttpMethodsAsync(Input.AllowedMethods);
			HttpHeadersOptions = await _selectOptions.GetHttpHeadersAsync(Input.AllowedHeaders);
			AllowAnyOriginOptions = await _selectOptions.GetBinaryOptionsAsync(true);
			AllowAnyMethodOptions = await _selectOptions.GetBinaryOptionsAsync(true);
			AllowAnyHeaderOptions = await _selectOptions.GetBinaryOptionsAsync(true);
			AllowCredentialsOptions = await _selectOptions.GetBinaryOptionsAsync(true);
			IdentificatorSettings settings = await _settingsService.GetSettingsAsync();
			CorsSettings cors = settings.CorsSettings ?? new CorsSettings();

			Input = new InputModel
			{
				PolicyMode = cors.PolicyMode,
				AllowAnyMethod = cors.AllowAnyMethod,
				AllowAnyHeader = cors.AllowAnyHeader,
				AllowedOrigins = cors.AllowedOrigins,
				AllowedMethods = cors.AllowedMethods,
				AllowedHeaders = cors.AllowedHeaders,
				AllowCredentials = cors.AllowCredentials,
				CorsConfigured = cors.IsConfigured
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

			IdentificatorSettings settings = await _settingsService.GetSettingsAsync();
			settings.CorsSettings = new CorsSettings
			{
				PolicyMode = Input.PolicyMode,
				AllowedOrigins = Input.AllowedOrigins,
				AllowAnyMethod = Input.AllowAnyMethod,
				AllowedMethods = Input.AllowedMethods,
				AllowAnyHeader = Input.AllowAnyHeader,
				AllowedHeaders = Input.AllowedHeaders,
				AllowCredentials = Input.AllowCredentials,
				IsConfigured = true
			};

			await _settingsService.SetSettingsAsync(settings);
			return RedirectToPage("/Install/InstallationResult"); // Adjust for next step
		}
	}
}