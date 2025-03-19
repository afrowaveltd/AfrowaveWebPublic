using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Install
{
	/// <summary>
	/// The CORS settings page model
	/// </summary>
	/// <param name="logger">Logger</param>
	/// <param name="settings">Settings manager</param>
	/// <param name="_t">Localization service</param>
	/// <param name="status">Installation status</param>
	/// <param name="selectOptions">Options generating service for Select HTML element</param>
	public class CorsSettingsModel(ILogger<CorsSettingsModel> logger,
		 ISettingsService settings,
		 IStringLocalizer<CorsSettingsModel> _t,
		 IInstallationStatusService status,
		 ISelectOptionsServices selectOptions) : PageModel
	{
		private readonly ILogger<CorsSettingsModel> _logger = logger;
		private readonly ISettingsService _settingsService = settings;
		private readonly ISelectOptionsServices _selectOptions = selectOptions;
		private readonly IInstallationStatusService _statusService = status;

		/// <summary>
		/// Holds a localizer for string resources specific to the CorsSettingsModel. It is marked as readonly, indicating it
		/// cannot be modified after initialization.
		/// </summary>
		public readonly IStringLocalizer<CorsSettingsModel> t = _t;

		/// <summary>
		/// HTTP methods options
		/// </summary>
		public List<SelectListItem> HttpMethodsOptions { get; set; } = [];

		/// <summary>
		/// HTTP headers options
		/// </summary>
		public List<SelectListItem> HttpHeadersOptions { get; set; } = [];

		/// <summary>
		/// Allow any origin options
		/// </summary>
		public List<SelectListItem> AllowAnyOriginOptions { get; set; } = [];

		/// <summary>
		/// Allow any method options
		/// </summary>
		public List<SelectListItem> AllowAnyMethodOptions { get; set; } = [];

		/// <summary>
		/// Allow credentials options
		/// </summary>
		public List<SelectListItem> AllowCredentialsOptions { get; set; } = [];

		/// <summary>
		/// Allow any header options
		/// </summary>
		public List<SelectListItem> AllowAnyHeaderOptions { get; set; } = [];

		/// <summary>
		/// The input model for the CORS settings
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; } = new();

		/// <summary>
		/// The input model for the CORS settings
		/// </summary>
		public class InputModel
		{
			/// <summary>
			/// Gets or sets the policy mode for CORS.
			/// </summary>
			public CorsPolicyMode PolicyMode { get; set; } = CorsPolicyMode.AllowAll;

			/// <summary>
			/// Gets or sets the allowed origins for CORS.
			/// </summary>
			public List<string> AllowedOrigins { get; set; } = [];

			/// <summary>
			/// Gets or sets a value indicating whether any method is allowed for CORS.
			/// </summary>
			public bool AllowAnyMethod { get; set; } = false;

			/// <summary>
			/// Gets or sets the allowed methods for CORS.
			/// </summary>
			public List<string> AllowedMethods { get; set; } = [];

			/// <summary>
			/// Gets or sets a value indicating whether any header is allowed for CORS.
			/// </summary>
			public bool AllowAnyHeader { get; set; } = false;

			/// <summary>
			/// Gets or sets the allowed headers for CORS.
			/// </summary>
			public List<string> AllowedHeaders { get; set; } = [];

			/// <summary>
			/// Gets or sets a value indicating whether credentials are allowed for CORS.
			/// </summary>
			public bool AllowCredentials { get; set; } = false;

			/// <summary>
			/// Gets or sets a value indicating whether CORS is configured.
			/// </summary>
			public bool CorsConfigured { get; set; } = false;
		}

		/// <summary>
		/// Get the page
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Post the page
		/// </summary>
		/// <returns></returns>
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