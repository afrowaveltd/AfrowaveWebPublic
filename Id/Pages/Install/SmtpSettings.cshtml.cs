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
		public List<SelectListItem> options;

		[BindProperty]
		public InputModel? Input { get; set; } = new();

		public string ErrorMessage { get; set; } = string.Empty;

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
			Input.SenderEmail = app.ApplicationEmail;
			Input.SenderName = app.Name;

			return Page();
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