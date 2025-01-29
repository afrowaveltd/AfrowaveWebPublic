using Id.Models.CommunicationModels;
using System.Security.Claims;

namespace Id.Pages.Account
{
	public class RegisterUserModel(
		 ILogger<RegisterUserModel> logger,
		 IUserService userService,
		 IApplicationService applicationService,
		 IEncryptionService encryptionService,
		 IRoleService roleService,
		 IStringLocalizer<RegisterUserModel> _t) : PageModel
	{
		private readonly ILogger<RegisterUserModel> _logger = logger;
		private readonly IUserService _userService = userService;
		private readonly IApplicationService _applicationService = applicationService;
		private readonly IEncryptionService _encryptionService = encryptionService;
		private readonly IRoleService _roleService = roleService;
		public IStringLocalizer<RegisterUserModel> t = _t;

		public ApplicationPublicInfo? ApplicationInfo { get; set; } = null;
		public bool DisplayAuthenticatorTerms { get; set; } = true;
		public bool DisplayAuthenticatorPrivacyPolicy { get; set; } = true;
		public bool DisplayAuthenticatorCookiePolicy { get; set; } = true;
		public bool DisplayApplicationTerms { get; set; } = false;
		public bool DisplayApplicationPrivacyPolicy { get; set; } = false;
		public bool DisplayApplicationCookiePolicy { get; set; } = false;
		public string ApplicationTermsUrl { get; set; } = string.Empty;
		public string ApplicationPrivacyPolicyUrl { get; set; } = string.Empty;
		public string ApplicationCookiePolicyUrl { get; set; } = string.Empty;
		public string AuthenticatorId { get; set; } = string.Empty;
		public RegistrationStep RegistrationStep { get; set; } = RegistrationStep.User;
		public RegistrationResult RegistrationResult { get; set; } = RegistrationResult.None;

		[BindProperty]
		public RegisterApplicationUserModel Input { get; set; } = new();

		[FromRoute]
		public string? ApplicationId { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			AuthenticatorId = await GetDefaultApplicationId();
			// Check if ApplicationId from the route is null or empty
			if(string.IsNullOrEmpty(ApplicationId))
			{
				// Set the ApplicationId to the default value
				ApplicationId = AuthenticatorId;
			}
			else
			{
				// Check if the ApplicationId is valid
				ApplicationId = await CheckApplicationId(ApplicationId);
			}
			if(string.IsNullOrEmpty(ApplicationId))
			{
				// Redirect to the error page
				_logger.LogError("ApplicationId is invalid");
				_ = RedirectToPage("/Error/404");
			}
			if(User.Identity?.IsAuthenticated ?? false)
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if(!string.IsNullOrEmpty(userId))
				{
					if(ApplicationId == AuthenticatorId)
					{
						RegistrationStep = RegistrationStep.Finish;
						RegistrationResult = RegistrationResult.AllreadyRegistered;
					}
				}
				return RedirectToPage("./QuickRegistration/" + ApplicationId);
			}
			else
			{
				ApplicationInfo = await _applicationService.GetPublicInfoAsync(ApplicationId);
				if(ApplicationInfo == null)
				{
					_logger.LogError("ApplicationInfo is null");
					return RedirectToPage("/Error/404");
				}

				return Page();
			}
		}

		private async Task<string> GetDefaultApplicationId()
		{
			// Retrieve the default application ID (e.g., from your service or configuration)
			return await _applicationService.GetDefaultApplicationId();
		}

		private async Task<string> CheckApplicationId(string applicationId)
		{
			// Check if the application ID is valid (e.g., from your service)
			return await _applicationService.CheckApplicationId(applicationId);
		}
	}
}