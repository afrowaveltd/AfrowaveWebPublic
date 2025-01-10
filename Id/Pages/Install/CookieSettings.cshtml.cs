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
		private readonly ILogger<CookieSettingsModel> _logger = logger;
		private readonly ISettingsService _settingsService = settings;
		public readonly IStringLocalizer<CookieSettingsModel> t = _t;
		private readonly ISelectOptionsServices _selectOptionsService = selectOptionsService;
		private readonly IInstallationStatusService _statusService = status;

		public List<SelectListItem> CookieSameSiteOptions { get; set; } = [];
		public List<SelectListItem> CookieSecureOptions { get; set; } = [];
		public List<SelectListItem> CookieHttpOnlyOptions { get; set; } = [];

		[BindProperty]
		public InputModel Input { get; set; } = new();

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

			var settings = await _settingsService.GetSettingsAsync();
			settings.CookieSettings = new CookieSettings()
			{
				Name = Input.CookieName,
				Domain = Input.CookieDomain,
				Path = Input.CookiePath,
				Secure = Input.CookieSecure,
				SameSite = Input.CookieSameSite,
				HttpOnly = Input.CookieHttpOnly,
				Expiration = Input.CookieExpiration
			};

			await _settingsService.SetSettingsAsync(settings);

			return RedirectToPage("/Install/JwtSettings");
		}
	}
}