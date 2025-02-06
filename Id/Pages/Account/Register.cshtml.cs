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
		public RegistrationResult RegistrationResult { get; set; } = RegistrationResult.None;
		public PasswordRules PasswordRules => _settingsService.GetPasswordRulesAsync().Result;

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
				return RedirectToPage("/Account/Index");
			}
			else
			{
				return Page();
			}
		}
	}
}