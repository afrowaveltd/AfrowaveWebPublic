using Id.Models.InputModels;
using Id.Models.ResultModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Install
{
	/// <summary>
	/// Application roles model
	/// </summary>
	/// <param name="_t">Localization service</param>
	/// <param name="applicationService">Applications manager service</param>
	/// <param name="applicationUsersManager">Application users manager service</param>
	/// <param name="context">Entity service</param>
	/// <param name="encryptionService">Encryption management service</param>
	/// <param name="settingsService">Settings service</param>
	/// <param name="installationStatus">Installation status service</param>
	/// <param name="logger">Logging service</param>
	/// <param name="roleService">Roles manager service</param>
	/// <param name="userService">Users management service</param>
	public class ApplicationRolesModel(
		IStringLocalizer<ApplicationRolesModel> _t,
		IApplicationsManager applicationService,
		IApplicationUsersManager applicationUsersManager,
		IEncryptionService encryptionService,
		IInstallationStatusService installationStatus,
		ILogger<ApplicationRolesModel> logger,
		ISettingsService settingsService,
		IRolesManager roleService,
		IUsersManager userService,
		ApplicationDbContext context) : PageModel
	{
		// Dependency Injection
		private readonly ILogger<ApplicationRolesModel> _logger = logger;

		private readonly IApplicationsManager _applicationService = applicationService;
		private readonly IApplicationUsersManager _applicationUsersManager = applicationUsersManager;
		private readonly ApplicationDbContext _context = context;
		private readonly IEncryptionService _encryptionService = encryptionService;
		private readonly IInstallationStatusService _installationStatus = installationStatus;
		private readonly IRolesManager _roleService = roleService;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IUsersManager _userService = userService;

		// private properties
		private readonly string successMessage = "<span class='text-center success'><i class='bi bi-check-lg'></i></span>";

		private readonly string failMessage = "<span class='text-center error'><i class='bi bi-x-lg'></i></span>";

		// public properties
		/// <summary>
		/// Localization service
		/// </summary>
		public readonly IStringLocalizer<ApplicationRolesModel> t = _t;

		/// <summary>
		/// Error message
		/// </summary>
		public string ErrorMessage { get; set; } = string.Empty;

		/// <summary>
		/// User ID data
		/// </summary>
		public string UserIdData { get; set; } = string.Empty;

		/// <summary>
		/// Application ID data
		/// </summary>
		public string ApplicationIdData { get; set; } = string.Empty;

		/// <summary>
		/// Create roles
		/// </summary>
		public string CreateRoles { get; set; } = string.Empty;

		/// <summary>
		/// User ID found
		/// </summary>
		public bool UserIdFound { get; set; } = false;

		/// <summary>
		/// Application ID found
		/// </summary>
		public bool ApplicationIdFound { get; set; } = false;

		// public properties for the form
		/// <summary>
		/// Terms agreement options for the form
		/// </summary>
		public List<SelectListItem> TermsAgreement = [];

		/// <summary>
		/// Cookies agreement options for the form
		/// </summary>
		public List<SelectListItem> CookiesAgreement = [];

		/// <summary>
		/// Sharing user details agreement options for the form
		/// </summary>
		public List<SelectListItem> SharingUserDetailsAgreement = [];

		/// <summary>
		/// Role creating result
		/// </summary>
		public List<CreateRoleResult> RoleCreatingResult = [];

		/// <summary>
		/// Role assigning result
		/// </summary>
		public List<RoleAssignResult> RoleAssigningResult = [];

		/// <summary>
		/// Form input model
		/// </summary>
		[BindProperty]
		public FormInput Input { get; set; } = new();

		/// <summary>
		/// Form input model
		/// </summary>
		/// <permission cref="ApplicationId">ApplicationId for the authenticator application</permission>
		/// <permission cref="UserId">UserId for the authenticator administrator</permission>"
		/// <permission cref="AgreedToTerms">Agreed to terms</permission>
		/// <permission cref="AgreedToCookies">Agreed to cookies</permission>
		/// <permission cref="AgreedSharingUserDetails">Agreed to share user details</permission>
		public class FormInput
		{
			/// <summary>
			/// ApplicationId for the authenticator application
			/// </summary>
			public string ApplicationId { get; set; } = string.Empty;

			/// <summary>
			/// UserId for the authenticator administrator
			/// </summary>
			public string UserId { get; set; } = string.Empty;

			/// <summary>
			/// Agreed to terms
			/// </summary>
			public bool AgreedToTerms { get; set; } = false;

			/// <summary>
			/// Agreed to cookies
			/// </summary>
			public bool AgreedToCookies { get; set; } = false;

			/// <summary>
			/// Agreed to share user details
			/// </summary>
			public bool AgreedSharingUserDetails { get; set; } = false;
		}

		/// <summary>
		/// OnGetAsync method
		/// </summary>
		/// <returns>Application roles Page</returns>
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
				UserIdData = t["No user found in the database"];
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
				ApplicationIdData = t["No application found in the database"];
				return Page();
			}
			else
			{
				ApplicationIdFound = true;
				ApplicationIdData = application.Id;
			}

			// Here we know that we have both the user and the application
			// let initialize the form input model

			TermsAgreement =
			[
				new SelectListItem { Text = t["Agree to terms"], Value = "true", Selected = Input.AgreedToTerms },
				new SelectListItem { Text = t["Disagree to terms"], Value = "false", Selected = !Input.AgreedToTerms }
			];

			CookiesAgreement =
			[
				new SelectListItem { Text = t["Agree to cookies"], Value = "true", Selected = Input.AgreedToCookies },
				new SelectListItem { Text = t["Disagree to cookies"], Value = "false", Selected = !Input.AgreedToCookies }
			];

			SharingUserDetailsAgreement =
			[
				new SelectListItem { Text = t["Agree to share user details"], Value = "true", Selected = Input.AgreedSharingUserDetails },
				new SelectListItem { Text = t["Disagree to share user details"], Value = "false", Selected = !Input.AgreedSharingUserDetails }
			];

			// create default roles for the application

			ApiResponse<List<CreateRoleResult>> RoleCreationResponse = await CreateDefaultRolesAsync(application.Id);
			if(!RoleCreationResponse.Successful)
			{
				CreateRoles = failMessage;
				RoleCreatingResult = RoleCreationResponse.Data ?? [];
				return Page();
			}
			else
			{
				CreateRoles = successMessage;
				RoleCreatingResult = RoleCreationResponse.Data ?? [];
			}
			// create application user
			RegisterApplicationUserInput applicationUser = new()
			{
				ApplicationId = application.Id,
				UserId = user.Id,
				AgreedToTerms = true,
				AgreedToCookies = true,
				AgreedSharingUserDetails = true,
			};

			RegisterApplicationUserResult result = await _applicationUsersManager.RegisterApplicationUserAsync(applicationUser);
			if(!result.Success)
			{
				ErrorMessage = result.ErrorMessage ?? string.Empty;
				return Page();
			}
			// now we have the applicationUser created and can assign it to the roles
			int applicationUserId = result.ApplicationUserId;
			if(applicationUserId == 0)
			{
				ErrorMessage = t["Application user creation failed"];
				return Page();
			}
			// assign the user to the owner role and assign the owner all the roles
			RoleAssigningResult = await _roleService.SetAllRolesToOwner(application.Id, user.Id);

			return Page();
		}

		/// <summary>
		/// OnPostAsync method
		/// </summary>
		/// <returns>Redirection to the SmtpSettings page in the case of success</returns>
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
				return RedirectToPage("/Install/SmtpSettings");
			}
			else
			{
				ErrorMessage = t["You must agree to terms, cookies and sharing user details"];
				return Page();
			}
		}

		private async Task<ApiResponse<List<CreateRoleResult>>> CreateDefaultRolesAsync(string applicationId)
		{
			ApiResponse<List<CreateRoleResult>> response = new();
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
				CreateRoleResult roleResult = await _roleService.CreateApplicationRoleAsync(input);
				if(!roleResult.Success)
				{
					response.Successful = false;
					response.Message += $"Error creating role {input.Name}, ";
				}
				results.Add(roleResult);
			}
			return response;
		}
	}
}