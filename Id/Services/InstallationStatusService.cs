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
			var smtpSettings = await _context.ApplicationSmtpSettings.FirstOrDefaultAsync();
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
			if(settings.Cookie == null || string.IsNullOrEmpty(settings.Cookie.Name))
			{
				return InstalationSteps.Cookie;
			}
			if(settings.JwtSettings == null || string.IsNullOrEmpty(settings.JwtSettings.Secret))
			{
				return InstalationSteps.JwtSettings;
			}
			return InstalationSteps.Finish;
		}

		public async Task<bool> ProperInstallState(InstalationSteps actualStep)
		{
			return await GetInstallationStepAsync() == actualStep;
		}
	}
}