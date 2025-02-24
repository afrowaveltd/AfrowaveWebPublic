using Id.Models.SettingsModels;

namespace Id.Pages.Install
{
	public class LoginRulesModel(ILogger<LoginRulesModel> logger,
		ISettingsService settings,
		IStringLocalizer<LoginRulesModel> _t,
		IInstallationStatusService status) : PageModel
	{
		// Injected services
		private readonly ILogger<LoginRulesModel> _logger = logger;

		private readonly ISettingsService _settings = settings;
		private readonly IInstallationStatusService _status = status;
		public readonly IStringLocalizer<LoginRulesModel> t = _t;

		// Input model
		[BindProperty]
		public InputModel? Input { get; set; }

		/// <summary>
		/// The input model for the login rules
		/// </summary>
		/// <permission cref="MaxFailedLoginAttempts">The maximum number of failed login attempts</permission>
		/// <permission cref="LockoutTime">The time in minutes to lockout the user</permission>
		/// <permission cref="PasswordResetTokenExpiration">The time in minutes for the password reset token to expire</permission>
		/// <permission cref="OTPTokenExpiration">The time in minutes for the OTP token to expire</permission>
		/// <permission cref="RequireConfirmedEmail">If the email must be confirmed</permission>
		/// <permission cref="ApplicationId">The application id</permission>
		/// <permission cref="IsConfigured">If the login rules are configured</permission>
		public class InputModel
		{
			public int MaxFailedLoginAttempts { get; set; } = 5;
			public int LockoutTime { get; set; } = 15;
			public int PasswordResetTokenExpiration { get; set; } = 15;
			public int OTPTokenExpiration { get; set; } = 15;
			public bool RequireConfirmedEmail { get; set; } = true;
			public string ApplicationId { get; set; } = string.Empty;
		}

		/// <summary>
		/// Handles the GET request
		/// </summary>
		/// <returns>Shows Login rules settings form</returns>
		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _status.ProperInstallState(InstalationSteps.LoginRules))
			{
				return RedirectToPage("/Install/SmtpSettings");
			}

			IdentificatorSettings settings = await _settings.GetSettingsAsync();
			string applicationId = settings.ApplicationId;
			Input = new();
			Input.ApplicationId = applicationId;

			return Page();
		}

		/// <summary>
		/// Handles the POST request
		/// </summary>
		/// <param name="Input">The input model</param>
		/// <returns>In case of success redirects to the PasswordRules page</returns>
		public async Task<IActionResult> OnPostAsync()
		{
			if(!ModelState.IsValid)
			{
				return Page();
			}
			IdentificatorSettings settings = await _settings.GetSettingsAsync();
			settings.LoginRules.MaxFailedLoginAttempts = Input.MaxFailedLoginAttempts;
			settings.LoginRules.LockoutTime = Input.LockoutTime;
			settings.LoginRules.PasswordResetTokenExpiration = Input.PasswordResetTokenExpiration;
			settings.LoginRules.OTPTokenExpiration = Input.OTPTokenExpiration;
			settings.LoginRules.RequireConfirmedEmail = Input.RequireConfirmedEmail;
			settings.LoginRules.IsConfigured = true;
			await _settings.SetSettingsAsync(settings);
			return RedirectToPage("/Install/PasswordRules");
		}
	}
}