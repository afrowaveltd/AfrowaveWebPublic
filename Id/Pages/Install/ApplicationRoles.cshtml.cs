using Id.Models.InputModels;
using Id.Models.ResultModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Install
{
	public class ApplicationRolesModel(
		IStringLocalizer<ApplicationRolesModel> _t,
		ILogger<ApplicationRolesModel> logger,
		IInstallationStatusService installationStatus,
		IUsersManager userService,
		IApplicationsManager applicationService,
		IApplicationUsersManager applicationUsersManager,
		ISettingsService settingsService,
		IEncryptionService encryptionService,
		IRolesManager roleService,
		ApplicationDbContext context) : PageModel
	{
		private readonly ILogger<ApplicationRolesModel> _logger = logger;
		private readonly IInstallationStatusService _installationStatus = installationStatus;
		private readonly IUsersManager _userService = userService;
		private readonly IApplicationsManager _applicationService = applicationService;
		private readonly IApplicationUsersManager _applicationUsersManager = applicationUsersManager;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IRolesManager _roleService = roleService;
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

			var roleCreatingResult = await CreateDefaultRolesAsync(application.Id);

			if(!roleCreatingResult.Where(x => x.Success == false).Any())
			{
				CreateRoles = t["Creating default application roles"];

				var RoleAssigningResult = await _roleService.SetAllRolesToOwner(user.Id, application.Id);

				return Page();
			}
			else
			{
				ErrorMessage = string.Join(", ", roleCreatingResult.Where(s => s.Success == false).Select(s => s.Errors.FirstOrDefault()).ToList()) ?? string.Empty;
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
				RegisterApplicationUserInput applicationUser = new()
				{
					ApplicationId = application.Id,
					UserId = user.Id,
					AgreedToTerms = Input.AgreedToTerms,
					AgreedToCookies = Input.AgreedToCookies,
					AgreedSharingUserDetails = Input.AgreedSharingUserDetails,
				};

				var result = await _applicationUsersManager.RegisterApplicationUserAsync(applicationUser);

				if(!result.Success)
				{
					ErrorMessage = result.ErrorMessage ?? string.Empty;

					return Page();
				}
				else
				{
					return RedirectToPage("/Install/SmtpSettings");
				}
			}
			else
			{
				ErrorMessage = t["You must agree to terms, cookies and sharing user details"];
				return Page();
			}
		}

		private async Task<List<CreateRoleResult>> CreateDefaultRolesAsync(string applicationId)
		{
			List<CreateRoleResult> results = [];
			List<CreateRoleInput> inputs =
			[
				new CreateRoleInput { ApplicationId = applicationId, Name = "Owner", AllignToAll = false, CanAdministerRoles = true },
				new CreateRoleInput { ApplicationId = applicationId, Name = "Administrator", AllignToAll = false, CanAdministerRoles = true },
				new CreateRoleInput { ApplicationId = applicationId, Name = "Moderator", AllignToAll = false, CanAdministerRoles = false },
				new CreateRoleInput { ApplicationId = applicationId, Name = "Translator", AllignToAll = false, CanAdministerRoles = false },
				new CreateRoleInput { ApplicationId = applicationId, Name = "User", AllignToAll = true, CanAdministerRoles = false }

			];
			foreach(CreateRoleInput input in inputs)
			{
				results.Add(await _roleService.CreateApplicationRoleAsync(input));
			}
			return results;
		}
	}
}