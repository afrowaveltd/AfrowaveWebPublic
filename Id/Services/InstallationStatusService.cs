namespace Id.Services
{
	/// <summary>
	/// Provides methods to check and manage the installation status of the application.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the <see cref="InstallationStatusService"/> class.
	/// </remarks>
	/// <param name="settingsService">The settings service to manage application settings.</param>
	/// <param name="logger">The logger instance for logging information.</param>
	/// <param name="context">The database context.</param>
	public class InstallationStatusService(
			 ISettingsService settingsService,
			 ILogger<InstallationStatusService> logger,
			 ApplicationDbContext context) : IInstallationStatusService
	{
		private readonly ISettingsService _settingsService = settingsService;
		private readonly ApplicationDbContext _context = context;
		private readonly ILogger<InstallationStatusService> _logger = logger;

		/// <summary>
		/// Determines the next installation step based on the application's current state.
		/// </summary>
		/// <returns>The next required installation step.</returns>
		public async Task<InstalationSteps> GetInstallationStepAsync()
		{
			if(!await _context.Users.AnyAsync())
			{
				return InstalationSteps.Administrator;
			}

			if(!await _context.Brands.AnyAsync())
			{
				return InstalationSteps.Brand;
			}

			if(!await _context.Applications.AnyAsync())
			{
				return InstalationSteps.Application;
			}

			IdentificatorSettings settings = await _settingsService.GetSettingsAsync();
			if(!await _context.ApplicationRoles.AnyAsync() || !await _context.UserRoles.AnyAsync())
			{
				return InstalationSteps.ApplicationRoles;
			}

			ApplicationSmtpSettings? smtpSettings = await _context.ApplicationSmtpSettings.FirstOrDefaultAsync();
			if(smtpSettings == null)
			{
				return InstalationSteps.SmtpSettings;
			}

			if(settings.LoginRules == null || !settings.LoginRules.IsConfigured)
			{
				return InstalationSteps.LoginRules;
			}
			if(settings.PasswordRules == null || !settings.PasswordRules.IsConfigured)
			{
				return InstalationSteps.PasswordRules;
			}

			if(settings.CookieSettings == null || !settings.CookieSettings.IsConfigured)
			{
				return InstalationSteps.CookieSettings;
			}

			if(settings.JwtSettings == null || !settings.JwtSettings.IsConfigured)
			{
				return InstalationSteps.JwtSettings;
			}

			if(settings.CorsSettings == null || !settings.CorsSettings.IsConfigured)
			{
				return InstalationSteps.CorsSettings;
			}

			if(!settings.InstallationFinished)
			{
				return InstalationSteps.Result;
			}

			return InstalationSteps.Finish;
		}

		/// <summary>
		/// Validates whether the current installation state matches the expected step.
		/// </summary>
		/// <param name="actualStep">The expected installation step.</param>
		/// <returns>True if the installation state is correct; otherwise, false.</returns>
		public async Task<bool> ProperInstallState(InstalationSteps actualStep)
		{
			if(actualStep != InstalationSteps.Administrator &&
				 actualStep != InstalationSteps.Brand &&
				 actualStep != InstalationSteps.Application)
			{
				await CheckAndFixApplicationId();
			}

			return await GetInstallationStepAsync() == actualStep;
		}

		/// <summary>
		/// Ensures the application ID is correctly set in the database and settings.
		/// </summary>
		private async Task CheckAndFixApplicationId()
		{
			IdentificatorSettings settings = await _settingsService.GetSettingsAsync();
			if(settings == null || string.IsNullOrEmpty(settings.ApplicationId))
			{
				return;
			}

			List<Application> applications = await _context.Applications.ToListAsync();
			if(applications.Count == 0)
			{
				return;
			}
			// Correct record in the file
			string? applicationId = applications.Where(x => x.IsAuthenticator).Select(x => x.Id).FirstOrDefault()
				?? throw new InvalidOperationException("No application marked as the Authenticator exists, but other applications found");

			if(applicationId != settings.ApplicationId)
			{
				settings.ApplicationId = applicationId;
				await _settingsService.SetSettingsAsync(settings);
			}
		}
	}
}