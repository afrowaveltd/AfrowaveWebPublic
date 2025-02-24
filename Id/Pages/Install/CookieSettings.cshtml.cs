using Id.Models.SettingsModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Install
{
	public class CookieSettingsModel(ILogger<CookieSettingsModel> logger,
		 ISettingsService settings,
		 IStringLocalizer<CookieSettingsModel> _t,
		 ISelectOptionsServices selectOptionsService,
		 IInstallationStatusService status) : PageModel
	{
		// Initialize the class
		private readonly ILogger<CookieSettingsModel> _logger = logger;

		private readonly ISettingsService _settingsService = settings;
		public readonly IStringLocalizer<CookieSettingsModel> t = _t;
		private readonly ISelectOptionsServices _selectOptionsService = selectOptionsService;
		private readonly IInstallationStatusService _statusService = status;

		// Initialize the properties
		public List<SelectListItem> CookieSameSiteOptions { get; set; } = [];

		public List<SelectListItem> CookieSecureOptions { get; set; } = [];
		public List<SelectListItem> CookieHttpOnlyOptions { get; set; } = [];

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
			public string CookieName { get; set; } = ".AuthCookie";
			public string CookieDomain { get; set; } = string.Empty;
			public string CookiePath { get; set; } = "/";
			public bool CookieSecure { get; set; } = true;
			public SameSiteMode CookieSameSite { get; set; } = SameSiteMode.Lax;
			public bool CookieHttpOnly { get; set; } = true;
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
		/// <param name="Input">The input model</param>
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