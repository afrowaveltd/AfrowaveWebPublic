using System.ComponentModel.DataAnnotations;

namespace Id.Pages.Install
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly ApplicationDbContext _context;
		private readonly IInstallationStatusService _installationStatus;
		private readonly IEncryptionService _encryptionService;
		public IStringLocalizer<IndexModel> t;

		[BindProperty]
		public InputModel Input { get; set; } = new();

		public class InputModel
		{
			[Required]
			[EmailAddress]
			public string Email { get; set; } = string.Empty;

			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; } = string.Empty;

			[Required]
			[DataType(DataType.Password)]
			public string PasswordConfirm { get; set; } = string.Empty;
		}

		public IndexModel(ILogger<IndexModel> logger,
							  ApplicationDbContext context,
							  IStringLocalizer<IndexModel> t,
							  IInstallationStatusService installationStatus,
							  ISettingsService settingsService,
							  IEncryptionService encryptionService)
		{
			_context = context;
			_logger = logger;
			this.t = t;
			_installationStatus = installationStatus;
			_encryptionService = encryptionService;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.Administrator))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.Administrator))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}
			if(ModelState.IsValid)
			{
				User admin = new();
				admin.Email = Input.Email;
				admin.Password = await _encryptionService.HashPasswordAsync(Input.Password);
				admin.DisplayName = "Administrator";
				admin.Firstname = "System";
				admin.Lastname = "Administrator";
				admin.EmailConfirmed = true;

				_ = await _context.Users.AddAsync(admin);
				_ = await _context.SaveChangesAsync();
				_logger.LogInformation("Administrator account created with {email} and {id}", admin.Email, admin.Id);
				return RedirectToPage("/Install/Brand");
			}
			return Page();
		}
	}
}