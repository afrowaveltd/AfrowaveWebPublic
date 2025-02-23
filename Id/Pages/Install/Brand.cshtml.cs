using Id.Models.InputModels;
using Id.Models.ResultModels;
using System.ComponentModel.DataAnnotations;

namespace Id.Pages.Install
{
	public class BrandModel(ILogger<BrandModel> logger,
	  ApplicationDbContext context,
	  IInstallationStatusService installationStatus,
	  IStringLocalizer<BrandModel> _t,
	  IBrandsManager brandService)
	 : PageModel
	{
		private readonly ILogger<BrandModel> _logger = logger;
		private readonly ApplicationDbContext _context = context;
		private readonly IInstallationStatusService _installationStatus = installationStatus;
		public readonly IStringLocalizer<BrandModel> t = _t;
		public readonly IBrandsManager _brandService = brandService;

		[BindProperty]
		public InputModel Input { get; set; } = new();

		public string ErrorMessage { get; set; } = string.Empty;

		public class InputModel
		{
			[Required]
			public string Name { get; set; } = string.Empty;

			[Required]
			public string OwnerId { get; set; } = string.Empty;

			public IFormFile? CompanyLogo { get; set; }
			public string? Website { get; set; }
			public string? Description { get; set; }
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
		/// <returns>If successful returns redirection to the next step</returns>
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
				ModelState.AddModelError("Input.Name", response.ErrorMessage);
				return Page();
			}
		}
	}
}