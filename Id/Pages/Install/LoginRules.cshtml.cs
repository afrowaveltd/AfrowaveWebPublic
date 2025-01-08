using Id.Models.SettingsModels;

namespace Id.Pages.Install
{
	public class LoginRulesModel(ILogger<LoginRulesModel> logger,
		ISettingsService settings,
		IStringLocalizer<LoginRulesModel> _t,
		IInstallationStatusService status) : PageModel
	{
		private readonly ILogger<LoginRulesModel> _logger = logger;
		private readonly ISettingsService _settings = settings;
		private readonly IInstallationStatusService _status = status;
		public readonly IStringLocalizer<LoginRulesModel> t = _t;

		[BindProperty]
		public InputModel? Input { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _status.ProperInstallState(InstalationSteps.LoginRules))
			{
				return RedirectToPage("/Install/SmtpSettings");
			}

			IdentificatorSettings settings = await _settings.GetSettingsAsync();
			string applicationId = settings.ApplicationId;
			Input = new InputModel();
			Input.ApplicationId = applicationId;

			return Page();
		}

		public class InputModel
		{
			public int MaxFailedLoginAttempts { get; set; } = 5;
			public int LockoutTime { get; set; } = 15;
			public int PasswordResetTokenExpiration { get; set; } = 15;
			public int EmailConfirmationTokenExpiration { get; set; } = 15;
			public bool RequireConfirmedEmail { get; set; } = true;
			public string ApplicationId { get; set; } = string.Empty;
		}
	}
}