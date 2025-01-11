using Id.Models.SettingsModels;

namespace Id.Services
{
	public class InstallationStatusService : IInstallationStatusService
	{
		private readonly ISettingsService _settingsService;
		private readonly ApplicationDbContext _context;
		private readonly ILogger<InstallationStatusService> _logger;

		public InstallationStatusService(ISettingsService settingsService,
			 ILogger<InstallationStatusService> logger,
			 ApplicationDbContext context)
		{
			_logger = logger;
			_settingsService = settingsService;
			_context = context;
		}

		public async Task<InstalationSteps> GetInstallationStepAsync()
		{
			if(await _context.Users.AnyAsync() == false)
			{
				return InstalationSteps.Administrator;
			}
			if(await _context.Brands.AnyAsync() == false)
			{
				return InstalationSteps.Brand;
			}
			if(await _context.Applications.AnyAsync() == false)
			{
				return InstalationSteps.Application;
			}
			IdentificatorSettings settings = await _settingsService.GetSettingsAsync();
			if(settings is null || string.IsNullOrEmpty(settings.ApplicationId))
			{
				return InstalationSteps.ApplicationRoles;
			}
			ApplicationSmtpSettings? smtpSettings = await _context.ApplicationSmtpSettings.FirstOrDefaultAsync();
			if(smtpSettings == null || string.IsNullOrEmpty(smtpSettings.Host))
			{
				return InstalationSteps.SmtpSettings;
			}
			if(settings.LoginRules == null || settings.LoginRules.MaxFailedLoginAttempts == 0)
			{
				return InstalationSteps.LoginRules;
			}
			if(settings.PasswordRules == null || settings.PasswordRules.MinimumLength == 0)
			{
				return InstalationSteps.PasswordRules;
			}
			if(settings.CookieSettings == null || string.IsNullOrEmpty(settings.CookieSettings.Domain))
			{
				return InstalationSteps.CookieSettings;
			}
			if(settings.JwtSettings == null || string.IsNullOrEmpty(settings.JwtSettings.Issuer))
			{
				return InstalationSteps.JwtSettings;
			}
			if(settings.CorsSettings == null || !settings.CorsSettings.CorsConfigured)
			{
				return InstalationSteps.CorsSettings;
			}
			return InstalationSteps.Finish;
		}

		public async Task<bool> ProperInstallState(InstalationSteps actualStep)
		{
			if(actualStep != InstalationSteps.Administrator && actualStep != InstalationSteps.Brand && actualStep != InstalationSteps.Application)
			{
				await CheckAndFixApplicationId();
			}
			return await GetInstallationStepAsync() == actualStep;
		}

		private async Task CheckAndFixApplicationId()
		{
			string settingsApplicationId = string.Empty;
			IdentificatorSettings settings = await _settingsService.GetSettingsAsync();
			if(settings is null)
			{
				settings = new IdentificatorSettings();
				settings.ApplicationId = string.Empty;
				await _settingsService.SetSettingsAsync(settings);
			}
			else
			{
				settingsApplicationId = settings.ApplicationId;
			}
			// check if there is only one application - if not, throw exception;
			List<Application> applications = await _context.Applications.ToListAsync();
			string dbApplicationId;
			if(applications.Count != 1)
			{
				throw new InvalidOperationException("There should be only one application in the database.");
			}
			else
			{
				dbApplicationId = applications.First().Id;
			}
			if(dbApplicationId != settingsApplicationId)
			{
				settings.ApplicationId = dbApplicationId;
				await _settingsService.SetSettingsAsync(settings);
			}
		}
	}
}