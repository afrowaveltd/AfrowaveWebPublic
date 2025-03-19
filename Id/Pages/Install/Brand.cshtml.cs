using System.ComponentModel.DataAnnotations;

namespace Id.Pages.Install
{
	/// <summary>
	/// Brand registration page
	/// </summary>
	/// <param name="logger">Logger service</param>
	/// <param name="context">Database context</param>
	/// <param name="installationStatus">Installation status service</param>
	/// <param name="_t">Localization service</param>
	/// <param name="brandService">Brand manager service</param>
	public class BrandModel(ILogger<BrandModel> logger,
	  ApplicationDbContext context,
	  IInstallationStatusService installationStatus,
	  IStringLocalizer<BrandModel> _t,
	  IBrandsManager brandService)
	 : PageModel
	{
		// Dependency injection
		private readonly ILogger<BrandModel> _logger = logger;

		private readonly ApplicationDbContext _context = context;
		private readonly IInstallationStatusService _installationStatus = installationStatus;

		/// <summary>
		/// Holds a reference to a localized string provider for the BrandModel type. It is marked as readonly, indicating it
		/// cannot be modified after initialization.
		/// </summary>
		public readonly IStringLocalizer<BrandModel> t = _t;

		/// <summary>
		/// Holds a reference to an instance of IBrandsManager, providing access to brand-related operations. It is marked as
		/// readonly, ensuring it cannot be modified after initialization.
		/// </summary>
		public readonly IBrandsManager _brandService = brandService;

		// Model binding
		/// <summary>
		/// Model for the input form
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; } = new();

		// Properties
		/// <summary>
		/// Error message
		/// </summary>
		public string ErrorMessage { get; set; } = string.Empty;

		/// <summary>
		/// Model for the input form
		/// </summary>
		/// <permission cref="Name">Name of the brand</permission>
		/// <permission cref="OwnerId">Id of the owner of the brand</permission>
		/// <permission cref="CompanyLogo">Logo of the brand</permission>
		/// <permission cref="Website">Website of the brand</permission>
		/// <permission cref="Description">Description of the brand</permission>
		/// <permission cref="Email">Email of the brand</permission>
		public class InputModel
		{
			/// <summary>
			/// Gets or sets the brand name.
			/// </summary>
			[Required]
			public string Name { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the brand owner ID.
			/// </summary>
			[Required]
			public string OwnerId { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the brand icon.
			/// </summary>
			public IFormFile? CompanyLogo { get; set; }

			/// <summary>
			/// Gets or sets the brand website.
			/// </summary>
			public string? Website { get; set; }

			/// <summary>
			/// Gets or sets the brand description.
			/// </summary>
			public string? Description { get; set; }

			/// <summary>
			/// Gets or sets the brand email.
			/// </summary>
			public string? Email { get; set; }
		}

		/// <summary>
		/// Get the page
		/// </summary>
		/// <returns> The Brand registration page</returns>
		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.Brand))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}
			Input.OwnerId = await _context.Users.Select(u => u.Id).FirstOrDefaultAsync() ?? string.Empty;
			if(Input.OwnerId is null || Input.OwnerId == string.Empty)
			{
				_logger.LogError("No user found to be the owner of the brand");
				Exception ex = new("No user found");
				throw ex;
			}
			return Page();
		}

		/// <summary>
		/// Post the Brand registration
		/// </summary>
		/// <returns>If successful returns redirection to the next step - application registration</returns>
		public async Task<IActionResult> OnPostAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.Brand))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}
			if(!ModelState.IsValid)
			{
				return Page();
			}
			RegisterBrandInput newBrand = new()
			{
				Name = Input.Name,
				Description = Input.Description,
				Icon = Input.CompanyLogo,
				Website = Input.Website,
				Email = Input.Email,
				OwnerId = Input.OwnerId
			};
			RegisterBrandResult response = await _brandService.RegisterBrandAsync(newBrand);

			// Check all the fields
			if(response.BrandCreated)
			{
				return RedirectToPage("/Install/Application");
			}
			else
			{
				ErrorMessage = response.ErrorMessage;
				return Page();
			}
		}
	}
}