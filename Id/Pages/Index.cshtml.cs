namespace Id.Pages
{
	/// <summary>
	/// The index page model
	/// </summary>
	/// <param name="logger">Logger manager</param>
	/// <param name="context">Entity service</param>
	/// <param name="installationStatus">Installation status manager</param>
	public class IndexModel(ILogger<IndexModel> logger,
			 ApplicationDbContext context,
			 IInstallationStatusService installationStatus) : PageModel
	{
		private readonly ILogger<IndexModel> _logger = logger;
		private readonly ApplicationDbContext _context = context;
		private readonly IInstallationStatusService _installationStatus = installationStatus;

		/// <summary>
		/// The index page model
		/// </summary>
		/// <returns></returns>
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

				case InstalationSteps.Result:
					_logger.LogInformation("Redirecting to Result page");
					return RedirectToPage("/Install/InstallationResult");

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