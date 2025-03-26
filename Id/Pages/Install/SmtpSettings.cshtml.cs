using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Id.Pages.Install
{
	/// <summary>
	/// The SMTP settings page model
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="context"></param>
	/// <param name="installationStatus"></param>
	/// <param name="settingsService"></param>
	/// <param name="_t"></param>
	/// <param name="selectOptions"></param>
	public class SmtpSettingsModel(ILogger<SmtpSettingsModel> logger,
		ApplicationDbContext context,
		IInstallationStatusService installationStatus,
		ISettingsService settingsService,
		IStringLocalizer<SmtpSettingsModel> _t,
		ISelectOptionsServices selectOptions)
	  : PageModel
	{
		private readonly ILogger<SmtpSettingsModel> _logger = logger;
		private readonly ApplicationDbContext _context = context;
		private readonly IInstallationStatusService _installationStatus = installationStatus;
		private readonly ISettingsService _settingsService = settingsService;

		/// <summary>
		/// The localization service
		/// </summary>
		public readonly IStringLocalizer<SmtpSettingsModel> t = _t;

		private readonly ISelectOptionsServices _selectOptions = selectOptions;

		/// <summary>
		/// The options for the secure socket options
		/// </summary>
		public List<SelectListItem> options = [];

		/// <summary>
		/// The input model for the smtp settings
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; } = new();

		/// <summary>
		/// The error message
		/// </summary>
		public string ErrorMessage { get; set; } = string.Empty;

		/// <summary>
		/// The input model for the smtp settings
		/// </summary>
		/// <permission cref="ApplicationId">The application id</permission>
		/// <permission cref="Host">The host of the smtp server</permission>
		/// <permission cref="Port">The port of the smtp server</permission>
		/// <permission cref="SmtpUsername">The username for the smtp server</permission>
		/// <permission cref="SmtpPassword">The password for the smtp server</permission>
		/// <permission cref="SenderEmail">The email of the sender</permission>
		/// <permission cref="SenderName">The name of the sender</permission>
		/// <permission cref="Secure">The secure socket options</permission>
		/// <permission cref="AuthorizationRequired">If authorization is required</permission>
		/// <remarks> The input model for the smtp settings</remarks>
		public class InputModel
		{
			/// <summary>
			/// Gets or sets the application id.
			/// </summary>
			[Required]
			public string ApplicationId { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the host of the smtp server.
			/// </summary>
			[Required]
			public string Host { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the port of the smtp server.
			/// </summary>
			[Required]
			public int Port { get; set; } = 0;

			/// <summary>
			/// Gets or sets the username for the smtp server.
			/// </summary>
			[Required]
			public string SmtpUsername { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the password for the smtp server.
			/// </summary>
			[Required]
			public string SmtpPassword { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the email of the sender.
			/// </summary>
			[Required]
			public string SenderEmail { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the name of the sender.
			/// </summary>
			[Required]
			public string SenderName { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the secure socket options.
			/// </summary>
			[Required]
			public MailKit.Security.SecureSocketOptions Secure { get; set; } = MailKit.Security.SecureSocketOptions.Auto;

			/// <summary>
			/// Gets or sets a value indicating whether authorization is required.
			/// </summary>
			[Required]
			public bool AuthorizationRequired { get; set; } = true;
		}

		/// <summary>
		/// Get the page
		/// </summary>
		/// <returns>The Application SMTP Settings creation page</returns>
		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.SmtpSettings))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}

			options = await _selectOptions.GetSecureSocketOptionsAsync();
			Application app = await GetApplicationAsync();
			if(app == null)
			{
				_logger.LogError("No application found to be the owner of the smtp settings");
				return RedirectToPage("/Error");
			}
			Input.ApplicationId = app.Id;
			if(Input.ApplicationId == null)
			{
				_logger.LogError("No application found to be the owner of the smtp settings");
				return RedirectToPage("/Error");
			}
			Input.SenderEmail = app.ApplicationEmail ?? string.Empty;
			Input.SenderName = app.Name;

			return Page();
		}

		/// <summary>
		/// Post the Administrator creation
		/// </summary>
		/// <remarks>Post the SMTP Settings creation</remarks>
		/// <permission cref="Input">The input model for the smtp settings</permission>
		/// <response>Redirection to the next page (Login rules)</response>
		/// <returns></returns>
		public async Task<IActionResult> OnPostAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.SmtpSettings))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}

			if(!ModelState.IsValid)
			{
				return Page();
			}
			Application? app = await _context.Applications.FirstOrDefaultAsync(a => a.Id == Input.ApplicationId);

			if(app == null)
			{
				_logger.LogError("No application found to be the owner of the smtp settings");
				return RedirectToPage("/Error");
			}

			ApplicationSmtpSettings settings = new()
			{
				ApplicationId = Input.ApplicationId,
				Host = Input.Host,
				Port = Input.Port,
				Username = Input.SmtpUsername,
				Password = Input.SmtpPassword,
				SenderEmail = Input.SenderEmail,
				SenderName = Input.SenderName,
				Secure = Input.Secure,
				AuthorizationRequired = Input.AuthorizationRequired,
				Application = app
			};

			try
			{
				_ = await _context.ApplicationSmtpSettings.AddAsync(settings);
				_ = await _context.SaveChangesAsync();
				return RedirectToPage("/Install/LoginRules");
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error saving smtp settings");
				ErrorMessage = t["Error saving smtp settings"];
				return Page();
			}
		}

		private async Task<Application> GetApplicationAsync()
		{
			Models.SettingsModels.IdentificatorSettings settings = await _settingsService.GetSettingsAsync();
			if(settings.ApplicationId == null)
			{
				_logger.LogError("No application found to be the owner of the smtp settings");
				return null;
			}

			return await _context.Applications.FirstOrDefaultAsync(a => a.Id == settings.ApplicationId) ?? null;
		}
	}
}