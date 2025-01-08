namespace Id.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly ApplicationDbContext _context;
		private readonly IInstallationStatusService _installationStatus;

		public IndexModel(ILogger<IndexModel> logger,
			 ApplicationDbContext context,
			 IInstallationStatusService installationStatus)
		{
			_logger = logger;
			_context = context;
			_installationStatus = installationStatus;
		}

		public async Task<ActionResult> OnGetAsync()
		{
			InstalationSteps step = await _installationStatus.GetInstallationStepAsync();
			switch(step)
			{
				case InstalationSteps.Administrator:
					_logger.LogInformation("Redirecting to Administrator setup page");
					return RedirectToPage("/Install/Index");

				case InstalationSteps.Brand:
					_logger.LogInformation("Redirecting to Brand setup page");
					return RedirectToPage("/Install/Brand");

				case InstalationSteps.Application:
					_logger.LogInformation("Redirecting to Application setup page");
					return RedirectToPage("/Install/Application");

				case InstalationSteps.ApplicationRoles:
					_logger.LogInformation("Redirecting to Application roles setup page");
					return RedirectToPage("/Install/ApplicationRoles");

				case InstalationSteps.SmtpSettings:
					_logger.LogInformation("Redirecting to SMTP setup page");
					return RedirectToPage("/Install/SmtpSettings");

				case InstalationSteps.LoginRules:
					_logger.LogInformation("Redirecting to LoginRules setup page");
					return RedirectToPage("/Install/LoginRules");

				case InstalationSteps.PasswordRules:
					_logger.LogInformation("Redirecting to Password rules setup page");
					return RedirectToPage("/Install/PasswordRules");

				case InstalationSteps.CookieSettings:
					_logger.LogInformation("Redirecting to Cookies setup page");
					return RedirectToPage("/Install/CookieSettings");

				case InstalationSteps.JwtSettings:
					_logger.LogInformation("Redirecting to JWT" +
						 " setup page");
					return RedirectToPage("/Install/JwtSettings");

				case InstalationSteps.CorsSettings:
					_logger.LogInformation("Redirecting to CORS setup page");
					return RedirectToPage("/Install/CorsSettings");

				case InstalationSteps.Finish:
					_logger.LogDebug("Installation not needed");
					break;

				default:
					break;
			}

			return Page();
		}
	}
}