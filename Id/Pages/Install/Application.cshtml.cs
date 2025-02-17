using Id.Models.CommunicationModels;
using SharedTools.Services;
using System.ComponentModel.DataAnnotations;

namespace Id.Pages.Install
{
	public class ApplicationModel(ILogger<ApplicationModel> logger,
								  ApplicationDbContext context,
								  IInstallationStatusService installationStatus,
								  IEncryptionService encryptionService,
								  ISettingsService settingsService,
								  IApplicationService applicationService,
								  IImageService imageService,
								  IStringLocalizer<ApplicationModel> _t) : PageModel
	{
		private readonly ILogger<ApplicationModel> _logger = logger;
		private readonly ApplicationDbContext _context = context;
		private readonly IInstallationStatusService _installationStatus = installationStatus;
		private readonly IEncryptionService _encryptionService = encryptionService;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IApplicationService _applicationService = applicationService;
		private readonly IImageService _imageService = imageService;
		public IStringLocalizer<ApplicationModel> t = _t;

		[BindProperty]
		public InputSettings Input { get; set; } = new();

		public string ErrorMessage { get; set; } = string.Empty;

		public class InputSettings
		{
			[Required]
			public string ApplicationName { get; set; } = string.Empty;

			public string ApplicationDescription { get; set; } = string.Empty;

			public string? ApplicationEmail { get; set; } = string.Empty;
			public string? ApplicationWebsite { get; set; } = string.Empty;

			[Required]
			public string OwnerId { get; set; } = string.Empty;

			[Required]
			public int BrandId { get; set; } = 0;

			public IFormFile? ApplicationIcon { get; set; }
		}

		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.Application))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}
			// get the Id of the first user
			if(await _context.Users.CountAsync() != 1)
			{
				_logger.LogError("There is more than one user in the database");
				return RedirectToPage("/Index");
			}

			User? user = await _context.Users.FirstOrDefaultAsync();
			if(user is null)
			{
				_logger.LogError("User is null");
				return RedirectToPage("/Index");
			}

			// get the Id of the first brand
			if(await _context.Brands.CountAsync() != 1)
			{
				_logger.LogError("There is more than one brand in the database");
				return RedirectToPage("/Index");
			}

			Brand? brand = await _context.Brands.FirstOrDefaultAsync();
			if(brand is null)
			{
				_logger.LogError("Brand is null");
				return RedirectToPage("/Index");
			}

			Input.OwnerId = user.Id;
			Input.BrandId = brand.Id;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.Application))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}

			if(!ModelState.IsValid)
			{
				_logger.LogError("Model state is not valid. It has following error(s):{err}", ModelState.Select(x => x.Value?.Errors)
							  .Where(y => y.Count > 0)
							  .ToList());
				ErrorMessage = ModelState.Select(x => x.Value?.Errors).ToString() ?? "Form data invalid";
				return Page();
			}

			User? user = await _context.Users.FirstOrDefaultAsync();
			if(user is null)
			{
				_logger.LogError("User is null");
				return RedirectToPage("/Index");
			}
			if(user.Id != Input.OwnerId)
			{
				_logger.LogError("User Id is not the same {userId} as the one in the form {ownerId}", user.Id, Input.OwnerId);
				return RedirectToPage("/Index");
			}
			// checks done - rest should be the work of the service

			RegisterApplicationModel newApplication = new()
			{
				Name = Input.ApplicationName,
				Description = Input.ApplicationDescription,
				Email = Input.ApplicationEmail,
				Website = Input.ApplicationWebsite,
				Icon = Input.ApplicationIcon,
				OwnerId = Input.OwnerId,
				BrandId = Input.BrandId,
				RequireTerms = true,
				RequirePrivacyPolicy = true,
				RequireCookiePolicy = true
			};

			RegisterApplicationResult response = await _applicationService.RegisterApplicationAsync(newApplication);

			if(!response.Success)
			{
				ErrorMessage = response.Error.FirstOrDefault() ?? "Unknown error";
				return Page();
			}
			// now we need to work on ApplicationId and Settings

			return RedirectToPage("/Index");
		}
	}
}