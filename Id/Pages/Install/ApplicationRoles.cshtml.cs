using Id.Models.CommunicationModels;
using Id.Models.SettingsModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Install
{
	public class ApplicationRolesModel(
		IStringLocalizer<ApplicationRolesModel> _t,
		ILogger<ApplicationRolesModel> logger,
		IInstallationStatusService installationStatus,
		IUserService userService,
		IApplicationService applicationService,
		ISettingsService settingsService,
		IEncryptionService encryptionService,
		IRoleService roleService,
		ApplicationDbContext context) : PageModel
	{
		private readonly ILogger<ApplicationRolesModel> _logger = logger;
		private readonly IInstallationStatusService _installationStatus = installationStatus;
		private readonly IUserService _userService = userService;
		private readonly IApplicationService _applicationService = applicationService;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IRoleService _roleService = roleService;
		private readonly ApplicationDbContext _context = context;
		private readonly IEncryptionService _encryptionService = encryptionService;
		private readonly string successMessage = "<span class='text-center success'><i class='bi bi-check-lg'></i></span>";
		private readonly string failMessage = "<span class='text-center error'><i class='bi bi-x-lg'></i></span>";

		public readonly IStringLocalizer<ApplicationRolesModel> t = _t;
		public string ErrorMessage { get; set; } = string.Empty;
		public string UserIdData { get; set; } = string.Empty;
		public string ApplicationIdData { get; set; } = string.Empty;
		public string CreateRoles { get; set; } = string.Empty;
		public bool UserIdFound { get; set; } = false;
		public bool ApplicationIdFound { get; set; } = false;
		public ApiResponse<List<ApplicationRole>> RoleCreatingResult { get; set; } = new();
		public List<RoleAssignResult> RoleAssigningResult { get; set; } = new();
		public List<SelectListItem> TermsAgreement = [];
		public List<SelectListItem> CookiesAgreement = [];
		public List<SelectListItem> SharingUserDetailsAgreement = [];

		public class FormInput
		{
			public string ApplicationId { get; set; } = string.Empty;
			public string UserId { get; set; } = string.Empty;
			public bool AgreedToTerms { get; set; } = false;
			public bool AgreedToCookies { get; set; } = false;
			public bool AgreedSharingUserDetails { get; set; } = false;
		}

		[BindProperty]
		public FormInput Input { get; set; } = new();

		public async Task<IActionResult> OnGetAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.ApplicationRoles))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}
			User? user = await _context.Users.FirstOrDefaultAsync();
			Application? application = await _context.Applications.FirstOrDefaultAsync();

			if(user == null)
			{
				_logger.LogError("No user found in the database");
				UserIdFound = false;
				UserIdData = "No user found in the database";
				return Page();
			}
			else
			{
				UserIdFound = true;
				UserIdData = user.Id;
			}
			if(application == null)
			{
				_logger.LogError("No application found in the database");
				ApplicationIdFound = false;
				ApplicationIdData = "No application found in the database";
				return Page();
			}
			else
			{
				ApplicationIdFound = true;
				ApplicationIdData = application.Id;
			}

			TermsAgreement = new()
			{
				new SelectListItem { Text = t["Agree to terms"], Value = "true", Selected = Input.AgreedToTerms },
				new SelectListItem { Text = t["Disagree to terms"], Value = "false", Selected = !Input.AgreedToTerms }
			};

			CookiesAgreement = new()
			{
				new SelectListItem { Text = t["Agree to cookies"], Value = "true", Selected = Input.AgreedToCookies },
				new SelectListItem { Text = t["Disagree to cookies"], Value = "false", Selected = !Input.AgreedToCookies }
			};

			SharingUserDetailsAgreement = new()
			{
				new SelectListItem { Text = t["Agree to share user details"], Value = "true", Selected = Input.AgreedSharingUserDetails },
				new SelectListItem { Text = t["Disagree to share user details"], Value = "false", Selected = !Input.AgreedSharingUserDetails }
			};

			// create default roles for the application

			RoleCreatingResult = await _roleService.CreateDefaultRoles(application.Id);

			if(RoleCreatingResult.Successful)
			{
				CreateRoles = t["Creating default application roles"];

				RoleAssigningResult = await _userService.AssignAllRolesToOwner(user.Id, application.Id);

				return Page();
			}
			else
			{
				ErrorMessage = RoleCreatingResult.Message ?? string.Empty;
				return Page();
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if(!await _installationStatus.ProperInstallState(InstalationSteps.ApplicationRoles))
			{
				_logger.LogWarning("Installation is not in the correct state");
				return RedirectToPage("/Index");
			}
			User? user = await _context.Users.FirstOrDefaultAsync();
			Application? application = await _context.Applications.FirstOrDefaultAsync();
			if(user == null)
			{
				_logger.LogError("No user found in the database");
				return Page();
			}

			if(application == null)
			{
				_logger.LogError("No application found in the database");
				return Page();
			}
			// logic for the user to agree to terms, cookies and sharing user details

			if(Input.AgreedToTerms && Input.AgreedToCookies && Input.AgreedSharingUserDetails)
			{
				CreateApplicationUserModel applicationUser = new()
				{
					ApplicationId = application.Id,
					UserId = user.Id,
					AgreedToTerms = Input.AgreedToTerms,
					AgreedToCookies = Input.AgreedToCookies,
					AgreedSharingUserDetails = Input.AgreedSharingUserDetails,
				};

				ApiResponse<int> result = await _userService.CreateApplicationUserAsync(applicationUser) ?? new();

				if(!result.Successful)
				{
					ErrorMessage = result.Message ?? string.Empty;

					return Page();
				}
				else
				{
					IdentificatorSettings settings = await _settingsService.GetSettingsAsync();

					settings.ApplicationId = application.Id;

					await _settingsService.SetSettingsAsync(settings);

					return RedirectToPage("/Install/Index");
				}
			}
			else
			{
				ErrorMessage = t["You must agree to terms, cookies and sharing user details"];
				return Page();
			}
		}
	}
}