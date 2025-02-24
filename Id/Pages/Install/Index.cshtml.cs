using System.ComponentModel.DataAnnotations;

namespace Id.Pages.Install
{
	public class IndexModel(ILogger<IndexModel> logger,
							  ApplicationDbContext context,
							  IStringLocalizer<IndexModel> t,
							  IInstallationStatusService installationStatus,
							  IEncryptionService encryptionService) : PageModel
	{
		// Dependency injection
		private readonly ILogger<IndexModel> _logger = logger;

		private readonly ApplicationDbContext _context = context;
		private readonly IInstallationStatusService _installationStatus = installationStatus;
		private readonly IEncryptionService _encryptionService = encryptionService;

		// Localization
		public readonly IStringLocalizer<IndexModel> t = t;

		// Model binding
		[BindProperty]
		public InputModel Input { get; set; } = new();

		/// <summary>
		/// Model for the input form
		/// </summary>
		/// <permission cref="Email">Email used for the registration and as the login</permission>
		/// <permission cref="Password">Password used for the registration and as the login</permission>
		/// <permission cref="PasswordConfirm">Password confirmation</permission>
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

		/// <summary>
		/// Get request for the page
		/// </summary>
		/// <returns>Administrator registration page</returns>
		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.Administrator))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}
			return Page();
		}

		/// <summary>
		/// Post request for the page
		/// </summary>
		/// <returns>Redirection to the Brand creation</returns>
		public async Task<IActionResult> OnPostAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.Administrator))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}
			if(ModelState.IsValid)
			{
				User admin = new()
				{
					Email = Input.Email,
					Password = await _encryptionService.HashPasswordAsync(Input.Password),
					DisplayName = "Administrator",
					Firstname = "System",
					Lastname = "Administrator",
					EmailConfirmed = true
				};

				_ = await _context.Users.AddAsync(admin);
				_ = await _context.SaveChangesAsync();
				_logger.LogInformation("Administrator account created with {email} and {id}", admin.Email, admin.Id);
				return RedirectToPage("/Install/Brand");
			}
			return Page();
		}
	}
}