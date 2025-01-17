using Id.Models.SettingsModels;
using SharedTools.Services;
using System.Globalization;

namespace Id.Pages.Install
{
	public class InstallationResultModel(IStringLocalizer<InstallationResultModel> _t,
		ApplicationDbContext context,
		IApplicationService applicationService,
		IBrandService brandService,
		ISettingsService settingsService,
		IInstallationStatusService installationStatus,
		ITranslatorService translatorService,
		ILogger<InstallationResultModel> logger) : PageModel
	{
		public readonly IStringLocalizer<InstallationResultModel> t = _t;
		private readonly ApplicationDbContext _context = context;
		private readonly IApplicationService _applicationService = applicationService;
		private readonly IBrandService _brandService = brandService;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IInstallationStatusService _statusService = installationStatus;
		private readonly ILogger<InstallationResultModel> _logger = logger;
		private readonly ITranslatorService _translatorService = translatorService;

		public string CurrentCulture { get; private set; }
		public string BrandName { get; set; } = string.Empty;
		public string BrandDescription { get; set; } = string.Empty;
		public int BrandId { get; set; } = 0;
		public string BrandEmail { get; set; } = string.Empty;
		public string BrandWebsite { get; set; } = string.Empty;
		public string BrandLogoLink { get; set; } = "/img/no-logo.png";
		public string ApplicationName { get; set; } = string.Empty;
		public string ApplicationDescription { get; set; } = string.Empty;
		public string ApplicationEmail { get; set; } = string.Empty;
		public string ApplicationWebsite { get; set; } = string.Empty;
		public string ApplicationId { get; set; } = string.Empty;
		public string ApplicationLogoLink { get; set; } = "/img/no-logo.png";
		public string AdminName { get; set; } = string.Empty;
		public string AdminRoles { get; set; } = string.Empty;
		public ApplicationSmtpSettings SmtpSettings { get; set; } = new ApplicationSmtpSettings();
		public string SmtpNeedsAuthentication { get; set; } = string.Empty;
		public IdentificatorSettings IdentificatorSettings { get; set; } = new IdentificatorSettings();

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
			ApplicationLogoLink = _applicationService.GetApplicationIconPath(application.Id);
			User admin = await _context.Users.FirstOrDefaultAsync(s => s.Id == application.OwnerId) ?? new();
			AdminName = admin.DisplayName;
			ApplicationSmtpSettings smtpSettings = application.SmtpSettings ?? new();
			Brand? brand = await _context.Brands.FirstOrDefaultAsync(s => s.Id == application.BrandId);
			BrandName = brand.Name ?? string.Empty;
			BrandId = brand.Id;
			BrandLogoLink = _brandService.GetBrandIconPath(brand.Id);
			ApiResponse<TranslateResponse> apiResponse = await _translatorService.AutodetectSourceLanguageAndTranslateAsync(brand.Description, CurrentCulture);
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