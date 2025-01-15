namespace Id.Pages.Install
{
	public class InstallationResultModel(IStringLocalizer<InstallationResultModel> _t,
		ApplicationDbContext context,
		ISettingsService settingsService,
		IInstallationStatusService installationStatus,
		ILogger<InstallationResultModel> logger) : PageModel
	{
		public readonly IStringLocalizer<InstallationResultModel> t = _t;
		public readonly ApplicationDbContext _context = context;
		public readonly ISettingsService _settingsService = settingsService;
		public readonly IInstallationStatusService _statusService = installationStatus;
		public readonly ILogger<InstallationResultModel> _logger = logger;

		public async Task<IActionResult> OnGetAsync()
		{
			return Page();
		}
	}
}