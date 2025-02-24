using Id.Models.DataViews;
using Id.Models.InputModels;
using Id.Models.SettingsModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Account
{
	public class RegisterUserModel(
		 ILogger<RegisterUserModel> logger,
		 IUsersManager userService,
		 IApplicationUsersManager applicationUsersManager,
		 IApplicationsManager applicationService,
		 IEncryptionService encryptionService,
		 IRolesManager roleService,
		 ISelectOptionsServices selectOptionsService,
		 ISettingsService settingsService,
		 IStringLocalizer<RegisterUserModel> _t) : PageModel
	{
		private readonly ILogger<RegisterUserModel> _logger = logger;
		private readonly IUsersManager _userService = userService;
		private readonly IApplicationUsersManager _applicationUsersManager = applicationUsersManager;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IApplicationsManager _applicationService = applicationService;
		private readonly IEncryptionService _encryptionService = encryptionService;
		private readonly IRolesManager _roleService = roleService;
		private readonly ISelectOptionsServices _selectOptionsService = selectOptionsService;
		private readonly IStringLocalizer<RegisterUserModel> t = _t;

		public string AuthenticatorId => _applicationService.GetAuthenticatorIdAsync().GetAwaiter().GetResult();
		public ApplicationView ApplicationInfo => _applicationService.GetInfoAsync(AuthenticatorId).GetAwaiter().GetResult() ?? new();

		public List<SelectListItem> PrivacyOptions => _selectOptionsService.GetBinaryOptionsAsync(true).Result;
		public List<SelectListItem> TermsOptions => _selectOptionsService.GetBinaryOptionsAsync(false).Result;
		public List<SelectListItem> CookieOptions => _selectOptionsService.GetBinaryOptionsAsync(false).Result;
		public List<string> RegistrationErrors = [];
		public List<string> RegistrationWarnings = [];
		public PasswordRules PasswordRules => _settingsService.GetPasswordRulesAsync().Result;
		public LoginRules LoginRules => _settingsService.GetLoginRulesAsync().Result;

		[BindProperty]
		public RegisterUserInput Input { get; set; } = new();

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
				if(!await _applicationService.ApplicationExistsAsync(ApplicationId))
				{
					// Redirect to the error page
					_logger.LogError("ApplicationId is invalid");
					return RedirectToPage("/Error/404");
				}
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