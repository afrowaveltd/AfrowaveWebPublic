using Id.Models.SettingsModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Install
{
	/// <summary>
	/// The cookie settings page model
	/// </summary>
	/// <param name="logger">The logger</param>
	/// <param name="settings">Settings service</param>
	/// <param name="_t">Localizer</param>
	/// <param name="selectOptionsService">Service generating options for the Select HTML elements</param>
	/// <param name="status">Installation status service</param>
	public class CookieSettingsModel(ILogger<CookieSettingsModel> logger,
		 ISettingsService settings,
		 IStringLocalizer<CookieSettingsModel> _t,
		 ISelectOptionsServices selectOptionsService,
		 IInstallationStatusService status) : PageModel
	{
		// Initialize the class
		private readonly ILogger<CookieSettingsModel> _logger = logger;

		private readonly ISettingsService _settingsService = settings;

		/// <summary>
		/// Localizer
		/// </summary>
		public readonly IStringLocalizer<CookieSettingsModel> t = _t;

		private readonly ISelectOptionsServices _selectOptionsService = selectOptionsService;
		private readonly IInstallationStatusService _statusService = status;

		// Initialize the properties
		/// <summary>
		/// The options for the SameSite mode of the cookie
		/// </summary>
		public List<SelectListItem> CookieSameSiteOptions { get; set; } = [];

		/// <summary>
		/// The options for the secure mode of the cookie
		/// </summary>
		public List<SelectListItem> CookieSecureOptions { get; set; } = [];

		/// <summary>
		/// The options for the http only mode of the cookie
		/// </summary>
		public List<SelectListItem> CookieHttpOnlyOptions { get; set; } = [];

		/// <summary>
		/// The input model for the cookie settings
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; } = new();

		/// <summary>
		/// The input model for the cookie settings
		/// </summary>
		/// <permission cref="CookieName">The name of the cookie</permission>
		/// <permission cref="CookieDomain">The domain of the cookie</permission>
		/// <permission cref="CookiePath">The path of the cookie</permission>
		/// <permission cref="CookieSecure">If the cookie is secure</permission>
		/// <permission cref="CookieSameSite">The same site mode of the cookie</permission>
		/// <permission cref="CookieHttpOnly">If the cookie is http only</permission>
		/// <permission cref="CookieExpiration">The expiration of the cookie</permission>
		public class InputModel
		{
			/// <summary>
			/// Gets or sets the name of the cookie.
			/// </summary>
			public string CookieName { get; set; } = ".AuthCookie";

			/// <summary>
			/// Gets or sets the domain of the cookie.
			/// </summary>
			public string CookieDomain { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the path of the cookie.
			/// </summary>
			public string CookiePath { get; set; } = "/";

			/// <summary>
			/// Gets or sets a value indicating whether the cookie is secure.
			/// </summary>
			public bool CookieSecure { get; set; } = true;

			/// <summary>
			/// Gets or sets the same site mode of the cookie.
			/// </summary>
			public SameSiteMode CookieSameSite { get; set; } = SameSiteMode.Lax;

			/// <summary>
			/// Gets or sets a value indicating whether the cookie is http only.
			/// </summary>
			public bool CookieHttpOnly { get; set; } = true;

			/// <summary>
			/// Gets or sets the expiration of the cookie.
			/// </summary>
			public int CookieExpiration { get; set; } = 60; // in minutes
		}

		/// <summary>
		///  Get the cookie settings page
		/// </summary>
		/// <returns>Cookie settings page</returns>
		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _statusService.ProperInstallState(InstalationSteps.CookieSettings))
			{
				return RedirectToPage("/");
			}

			CookieSecureOptions = await _selectOptionsService.GetBinaryOptionsAsync(true);
			CookieSameSiteOptions = await _selectOptionsService.GetSameSiteModeOptionsAsync(SameSiteMode.Lax);
			CookieHttpOnlyOptions = await _selectOptionsService.GetBinaryOptionsAsync(true);
			return Page();
		}

		/// <summary>
		/// Post the cookie settings
		/// </summary>
		/// <returns>in the case of success redirection to JWT settings</returns>
		public async Task<IActionResult> OnPostAsync()
		{
			if(!await _statusService.ProperInstallState(InstalationSteps.CookieSettings))
			{
				return RedirectToPage("/");
			}

			if(!ModelState.IsValid)
			{
				return Page();
			}

			IdentificatorSettings settings = await _settingsService.GetSettingsAsync();
			settings.CookieSettings = new CookieSettings()
			{
				Name = Input.CookieName,
				Domain = Input.CookieDomain,
				Path = Input.CookiePath,
				Secure = Input.CookieSecure,
				SameSite = Input.CookieSameSite,
				HttpOnly = Input.CookieHttpOnly,
				Expiration = Input.CookieExpiration,
				IsConfigured = true
			};

			await _settingsService.SetSettingsAsync(settings);

			return RedirectToPage("/Install/JwtSettings");
		}
	}
}