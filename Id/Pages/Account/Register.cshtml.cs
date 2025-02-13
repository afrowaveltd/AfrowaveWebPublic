using Id.Models.CommunicationModels;
using Id.Models.SettingsModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Account
{
	public class RegisterUserModel(
		 ILogger<RegisterUserModel> logger,
		 IUserService userService,
		 IApplicationService applicationService,
		 IEncryptionService encryptionService,
		 IRoleService roleService,
		 ISelectOptionsServices selectOptionsService,
		 ISettingsService settingsService,
		 IStringLocalizer<RegisterUserModel> _t) : PageModel
	{
		private readonly ILogger<RegisterUserModel> _logger = logger;
		private readonly IUserService _userService = userService;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IApplicationService _applicationService = applicationService;
		private readonly IEncryptionService _encryptionService = encryptionService;
		private readonly IRoleService _roleService = roleService;
		private readonly ISelectOptionsServices _selectOptionsService = selectOptionsService;
		private readonly IStringLocalizer<RegisterUserModel> t = _t;

		public string AuthenticatorId => _applicationService.GetDefaultApplicationId().Result;
		public ApplicationPublicInfo? ApplicationInfo => _applicationService.GetPublicInfoAsync(AuthenticatorId).Result;

		public List<SelectListItem> PrivacyOptions => _selectOptionsService.GetBinaryOptionsAsync(true).Result;
		public List<SelectListItem> TermsOptions => _selectOptionsService.GetBinaryOptionsAsync(false).Result;
		public List<SelectListItem> CookieOptions => _selectOptionsService.GetBinaryOptionsAsync(false).Result;
		public List<string> RegistrationErrors = [];
		public List<string> RegistrationWarnings = [];
		public PasswordRules PasswordRules => _settingsService.GetPasswordRulesAsync().Result;
		public LoginRules LoginRules => _settingsService.GetLoginRulesAsync().Result;

		[BindProperty]
		public RegisterApplicationUserModel Input { get; set; } = new();

		[FromRoute]
		public string? ApplicationId { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			// Check if ApplicationId from the route is null or empty
			if(string.IsNullOrEmpty(ApplicationId))
			{
				// Set the ApplicationId to the default value
				ApplicationId = AuthenticatorId;
			}
			else
			{
				// Check if the ApplicationId is valid
				ApplicationId = await _applicationService.CheckApplicationId(ApplicationId);
			}
			if(string.IsNullOrEmpty(ApplicationId))
			{
				// Redirect to the error page
				_logger.LogError("ApplicationId is invalid");
				_ = RedirectToPage("/Error/404");
			}

			if(User.Identity?.IsAuthenticated ?? false)
			{
				return RedirectToPage("/Account/Index");
			}
			else
			{
				if(ApplicationInfo == null)
				{
					_logger.LogError("ApplicationInfo is null");
					return RedirectToPage("/Error/404");
				}

				return Page();
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			_logger.LogInformation("Registering user");
			if(ModelState.IsValid)
			{
				UserRegistrationResult userRegistrationResult = await _userService.CreateUserAsync(Input);
				if(!userRegistrationResult.Success)
				{
					RegistrationErrors = userRegistrationResult.Errors;
					_logger.LogError("User registration failed");
					return Page();
				}
				else
				{
					// user seems to be registered to Authenticator - now create the application user
					CreateApplicationUserModel applicationUser = new()
					{
						ApplicationId = ApplicationId,
						UserId = userRegistrationResult.UserId,
						AgreedToTerms = Input.AcceptTerms,
						AgreedToCookies = Input.AcceptCookiePolicy,
						AgreedSharingUserDetails = Input.AcceptPrivacyPolicy
					};
					ApiResponse<int> applicationUserResult = await _userService.CreateApplicationUserAsync(applicationUser);
					if(!applicationUserResult.Successful)
					{
						_logger.LogError("Application user creation failed");
						RegistrationErrors.Add("Application user creation failed");
						return Page();
					}
					int applicationUserId = applicationUserResult.Data;
					_logger.LogInformation("Application user created with Id {id}", applicationUserId);
					// assign role to the application user
					if(await _roleService.AssignDefaultRolesToNewUserAsync(userRegistrationResult.UserId, ApplicationId))
					{
						_logger.LogInformation("All roles assigned to user");
					}
					else
					{
						_logger.LogError("All roles not assigned to user");
						RegistrationErrors.Add("All roles not assigned to user");
						return Page();
					}
				}
				if(userRegistrationResult.Errors.Count > 0)
				{
					RegistrationWarnings = userRegistrationResult.Errors;

					_logger.LogError("User successful with warnings");
					return Page();
				}
				if(LoginRules.RequireConfirmedEmail)
				{
					_logger.LogInformation("Email confirmation required");
					return RedirectToPage("/Account/OtpLogin/" + Input.Email);
				}
				if(Input.ApplicationId != AuthenticatorId)
				{
				}
				return RedirectToPage("/Account/Login");
				/// ToDo - if email confirmation is not required, log the user in
			}
			else
			{
				return Page();
			}
		}
	}
}