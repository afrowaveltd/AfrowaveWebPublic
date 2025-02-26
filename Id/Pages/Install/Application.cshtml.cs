using Id.Models.InputModels;
using Id.Models.ResultModels;
using SharedTools.Services;
using System.ComponentModel.DataAnnotations;

namespace Id.Pages.Install
{
	public class ApplicationModel(ILogger<ApplicationModel> logger,
								  ApplicationDbContext context,
								  IInstallationStatusService installationStatus,
								  IEncryptionService encryptionService,
								  ISettingsService settingsService,
								  IApplicationsManager applicationService,
								  IApplicationUsersManager applicationUserService,
								  IImageService imageService,
								  IStringLocalizer<ApplicationModel> _t) : PageModel
	{
		// Dependency Injection
		private readonly ILogger<ApplicationModel> _logger = logger;

		private readonly ApplicationDbContext _context = context;
		private readonly IInstallationStatusService _installationStatus = installationStatus;
		private readonly IEncryptionService _encryptionService = encryptionService;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IApplicationUsersManager _applicationUserService = applicationUserService;
		private readonly IApplicationsManager _applicationService = applicationService;
		private readonly IImageService _imageService = imageService;

		// Localization
		public IStringLocalizer<ApplicationModel> t = _t;

		// Properties
		[BindProperty]
		public InputSettings Input { get; set; } = new();

		public string ErrorMessage { get; set; } = string.Empty;

		/// <summary>
		/// Input settings for the application
		/// </summary>
		/// <permission cref="ApplicationName">Name of the application used for authentication and authorization services</permission>
		/// <permission cref="ApplicationDescription">Description of the application</permission>
		/// <permission cref="ApplicationEmail">Email of the application</permission>
		/// <permission cref="ApplicationWebsite">Website of the application</permission>
		/// <permission cref="OwnerId">Id of the owner of the application</permission>
		/// <permission cref="BrandId">Id of the brand of the application</permission>
		/// <permission cref="ApplicationIcon">Icon of the application</permission>
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

		/// <summary>
		/// Get the page
		/// </summary>
		/// <returns>The default application registration page</returns>
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

		/// <summary>
		/// Post the page
		/// </summary>
		/// <returns>In the case of success, redirects to the ApplicationRoles page</returns>
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

			RegisterApplicationInput newApplication = new()
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

			if(!response.ApplicationCreated)
			{
				ErrorMessage = response.ErrorMessage ?? "Unknown error";
				return Page();
			}

			// we have the application, let register the applicationUser;
			RegisterApplicationUserInput applicationUser = new()
			{
				ApplicationId = response.ApplicationId,
				UserId = user.Id,
				AgreedSharingUserDetails = true,
				AgreedToCookies = true,
				AgreedToTerms = true,
			};
			_ = await _applicationUserService.RegisterApplicationUserAsync(applicationUser);

			// now we need to work on ApplicationId and Settings
			await _settingsService.SetApplicationId(response.ApplicationId);
			return RedirectToPage("/ApplicationRoles");
		}
	}
}