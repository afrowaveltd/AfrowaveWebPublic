using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Id.Pages.Install
{
	public class SmtpSettingsModel(ILogger<BrandModel> logger,
		ApplicationDbContext context,
		IInstallationStatusService installationStatus,
		ISettingsService settingsService,
		IStringLocalizer<SmtpSettingsModel> _t,
		ISelectOptionsServices selectOptions)
	  : PageModel
	{
		private readonly ILogger<BrandModel> _logger = logger;
		private readonly ApplicationDbContext _context = context;
		private readonly IInstallationStatusService _installationStatus = installationStatus;
		private readonly ISettingsService _settingsService = settingsService;
		public readonly IStringLocalizer<SmtpSettingsModel> t = _t;
		private readonly ISelectOptionsServices _selectOptions = selectOptions;
		public List<SelectListItem> options = new();

		[BindProperty]
		public InputModel Input { get; set; } = new();

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
			[Required]
			public string ApplicationId { get; set; } = string.Empty;

			[Required]
			public string Host { get; set; } = string.Empty;

			[Required]
			public int Port { get; set; } = 0;

			[Required]
			public string SmtpUsername { get; set; } = string.Empty;

			[Required]
			public string SmtpPassword { get; set; } = string.Empty;

			[Required]
			public string SenderEmail { get; set; } = string.Empty;

			[Required]
			public string SenderName { get; set; } = string.Empty;

			[Required]
			public MailKit.Security.SecureSocketOptions Secure { get; set; } = MailKit.Security.SecureSocketOptions.Auto;

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
		/// <param name="Input">The input model for the smtp settings</param>
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
				return new();
			}

			return await _context.Applications.FirstOrDefaultAsync(a => a.Id == settings.ApplicationId) ?? new();
		}
	}
}