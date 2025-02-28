using Id.Models.SettingsModels;
using SharedTools.Services;
using System.Globalization;

namespace Id.Pages.Install
{
	/// <summary>
	/// The installation result page model
	/// </summary>
	/// <param name="_t">Localizer</param>
	/// <param name="context">Entity manager</param>
	/// <param name="applicationService">Applications service</param>
	/// <param name="brandService">Brands service</param>
	/// <param name="settingsService">Settings manager</param>
	/// <param name="installationStatus">Installation status service</param>
	/// <param name="translatorService">Translator manager</param>
	/// <param name="logger">Logger manager</param>
	public class InstallationResultModel(IStringLocalizer<InstallationResultModel> _t,
		ApplicationDbContext context,
		IApplicationsManager applicationService,
		IBrandsManager brandService,
		ISettingsService settingsService,
		IInstallationStatusService installationStatus,
		ITranslatorService translatorService,
		ILogger<InstallationResultModel> logger) : PageModel
	{
		public readonly IStringLocalizer<InstallationResultModel> t = _t;
		private readonly ApplicationDbContext _context = context;
		private readonly IApplicationsManager _applicationService = applicationService;
		private readonly IBrandsManager _brandService = brandService;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IInstallationStatusService _statusService = installationStatus;
		private readonly ILogger<InstallationResultModel> _logger = logger;
		private readonly ITranslatorService _translatorService = translatorService;

		/// <summary>
		/// Model for the input form
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; }

		/// <summary>
		/// Model for the input form
		/// </summary>
		public class InputModel
		{
			/// <summary>
			/// Gets or sets the installation finished state.
			/// </summary>
			public bool InstallationFinished { get; set; }
		}

		/// <summary>
		/// The current culture
		/// </summary>
		public string CurrentCulture { get; private set; } = "en";

		/// <summary>
		/// The brand name
		/// </summary>
		public string BrandName { get; set; } = string.Empty;

		/// <summary>
		/// The brand description
		/// </summary>
		public string BrandDescription { get; set; } = string.Empty;

		/// <summary>
		/// The brand ID
		/// </summary>
		public int BrandId { get; set; } = 0;

		/// <summary>
		/// The brand email
		/// </summary>
		public string BrandEmail { get; set; } = string.Empty;

		/// <summary>
		/// The brand website
		/// </summary>
		public string BrandWebsite { get; set; } = string.Empty;

		/// <summary>
		/// The brand logo link
		/// </summary>
		public string BrandLogoLink { get; set; } = "/img/no-logo.png";

		/// <summary>
		/// The application name
		/// </summary>
		public string ApplicationName { get; set; } = string.Empty;

		/// <summary>
		/// The application description
		/// </summary>
		public string ApplicationDescription { get; set; } = string.Empty;

		/// <summary>
		/// The application email
		/// </summary>
		public string ApplicationEmail { get; set; } = string.Empty;

		/// <summary>
		/// The application website
		/// </summary>
		public string ApplicationWebsite { get; set; } = string.Empty;

		/// <summary>
		/// The application ID
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// The application logo link
		/// </summary>
		public string ApplicationLogoLink { get; set; } = "/img/no-logo.png";

		/// <summary>
		/// The admin name
		/// </summary>
		public string AdminName { get; set; } = string.Empty;

		/// <summary>
		/// The admin roles
		/// </summary>
		public string AdminRoles { get; set; } = string.Empty;

		/// <summary>
		/// The SMTP settings
		/// </summary>
		public ApplicationSmtpSettings SmtpSettings { get; set; } = new ApplicationSmtpSettings();

		/// <summary>
		/// The SMTP needs authentication
		/// </summary>
		public string SmtpNeedsAuthentication { get; set; } = string.Empty;

		/// <summary>
		/// The identificator settings
		/// </summary>
		public IdentificatorSettings IdentificatorSettings { get; set; } = new IdentificatorSettings();

		/// <summary>
		/// On post
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> OnPostAsync()
		{
			if(!await _statusService.ProperInstallState(InstalationSteps.Result))
			{
				return RedirectToPage("/");
			}
			IdentificatorSettings = await _settingsService.GetSettingsAsync();
			IdentificatorSettings.InstallationFinished = true;
			await _settingsService.SetSettingsAsync(IdentificatorSettings);
			return RedirectToPage("/Index");
		}

		/// <summary>
		/// Get the page
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _statusService.ProperInstallState(InstalationSteps.Result))
			{
				return RedirectToPage("/");
			}
			CurrentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
			IdentificatorSettings = await _settingsService.GetSettingsAsync();
			Application application = await _context.Applications.Include(s => s.SmtpSettings).FirstOrDefaultAsync(s => s.Id == IdentificatorSettings.ApplicationId) ?? new();
			ApplicationId = application.Id;
			ApplicationLogoLink = _applicationService.GetIconPath(application.Id);
			User admin = await _context.Users.FirstOrDefaultAsync(s => s.Id == application.OwnerId) ?? new();
			AdminName = admin.DisplayName;
			ApplicationSmtpSettings smtpSettings = application.SmtpSettings ?? new();
			Brand? brand = await _context.Brands.FirstOrDefaultAsync(s => s.Id == application.BrandId);
			BrandName = brand?.Name ?? string.Empty;
			BrandId = brand.Id;
			BrandLogoLink = _brandService.GetIconPath(brand.Id);
			ApiResponse<TranslateResponse> apiResponse = await _translatorService.AutodetectSourceLanguageAndTranslateAsync(brand.Description ?? string.Empty, CurrentCulture);
			if(apiResponse.Successful)
			{
				if(apiResponse.Data?.DetectedLanguage?.Language == CurrentCulture)
				{
					BrandDescription = brand.Description ?? string.Empty;
				}
				else
				{
					BrandDescription = apiResponse.Data?.TranslatedText ?? string.Empty;
				}
			}
			else
			{
				BrandDescription = brand.Description ?? string.Empty;
			}

			BrandEmail = brand.Email ?? string.Empty;
			BrandWebsite = brand.Website ?? string.Empty;
			ApplicationName = application.Name ?? string.Empty;

			ApiResponse<TranslateResponse> appResponse = await _translatorService.AutodetectSourceLanguageAndTranslateAsync(application.Description, CurrentCulture);
			if(appResponse.Successful)
			{
				if(appResponse.Data?.DetectedLanguage?.Language == CurrentCulture)
				{
					ApplicationDescription = application.Description ?? string.Empty;
				}
				else
				{
					ApplicationDescription = appResponse.Data?.TranslatedText ?? string.Empty;
				}
			}
			else
			{
				ApplicationDescription = application.Description ?? string.Empty;
			}

			ApplicationEmail = application.ApplicationEmail ?? string.Empty;
			ApplicationWebsite = application.ApplicationWebsite ?? string.Empty;
			List<string> roles = await _context.UserRoles.Include(s => s.ApplicationRole).Select(s => s.ApplicationRole.NormalizedName).ToListAsync() ?? new();
			AdminRoles = string.Join(", ", roles);
			SmtpSettings = await _context.ApplicationSmtpSettings.FirstOrDefaultAsync(s => s.ApplicationId == application.Id) ?? new();
			SmtpNeedsAuthentication = SmtpSettings.AuthorizationRequired ? t["Yes"] : t["No"];

			return Page();
		}
	}
}