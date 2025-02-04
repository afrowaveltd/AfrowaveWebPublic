using Id.Models.CommunicationModels;
using Id.Models.SettingsModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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

		public ApplicationPublicInfo? ApplicationInfo { get; set; } = null;
		public bool DisplayApplicationTerms { get; set; } = false;
		public bool DisplayApplicationPrivacyPolicy { get; set; } = false;
		public bool DisplayApplicationCookiePolicy { get; set; } = false;
		public string ApplicationTerms { get; set; } = string.Empty;
		public string ApplicationPrivacyPolicy { get; set; } = string.Empty;
		public string ApplicationCookiePolicy { get; set; } = string.Empty;
		public string AuthenticatorId { get; set; } = string.Empty;
		public List<SelectListItem> PrivacyOptions { get; set; } = new();
		public List<SelectListItem> TermsOptions { get; set; } = new();
		public List<SelectListItem> CookieOptions { get; set; } = new();
		public RegistrationResult RegistrationResult { get; set; } = RegistrationResult.None;

		[BindProperty]
		public RegistrationStep RegistrationStep { get; set; } = RegistrationStep.User;

		[BindProperty]
		public RegisterApplicationUserModel Input { get; set; } = new();

		[BindProperty]
		public ApplicationPolicyModel Agreement { get; set; } = new();

		[BindProperty]
		public PasswordRules PasswordRules { get; set; } = new();

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
				string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if(!string.IsNullOrEmpty(userId))
				{
					if(ApplicationId == AuthenticatorId)
					{
						RegistrationStep = RegistrationStep.Finish;
						RegistrationResult = RegistrationResult.AllreadyRegistered;
					}
				}
				RegistrationStep = RegistrationStep.Application;
				return Page();
			}
			else
			{
				ApplicationInfo = await _applicationService.GetPublicInfoAsync(ApplicationId);
				if(ApplicationInfo == null)
				{
					_logger.LogError("ApplicationInfo is null");
					return RedirectToPage("/Error/404");
				}
				TermsOptions = await _selectOptionsService.GetBinaryOptionsAsync(false);
				PrivacyOptions = await _selectOptionsService.GetBinaryOptionsAsync(true);
				CookieOptions = await _selectOptionsService.GetBinaryOptionsAsync(false);
				PasswordRules = await _settingsService.GetPasswordRulesAsync();
				return Page();
			}
		}

		public async Task<IActionResult> OnPostRegisterUserAsync()
		{
			if(ModelState.IsValid)
			{
			}
			else
			{
				return Page();
			}
			_logger.LogInformation("Registering user");

			// Inicializace vlastností modelu, jako v OnGetAsync
			AuthenticatorId = await GetDefaultApplicationId();
			if(string.IsNullOrEmpty(ApplicationId))
			{
				ApplicationId = AuthenticatorId;
			}
			else
			{
				ApplicationId = await CheckApplicationId(ApplicationId);
			}

			if(string.IsNullOrEmpty(ApplicationId))
			{
				_logger.LogError("ApplicationId is invalid");
				return RedirectToPage("/Error/404");
			}

			ApplicationInfo = await _applicationService.GetPublicInfoAsync(ApplicationId);
			if(ApplicationInfo == null)
			{
				_logger.LogError("ApplicationInfo is null");
				return RedirectToPage("/Error/404");
			}

			TermsOptions = await _selectOptionsService.GetBinaryOptionsAsync(false);
			PrivacyOptions = await _selectOptionsService.GetBinaryOptionsAsync(true);
			CookieOptions = await _selectOptionsService.GetBinaryOptionsAsync(false);
			PasswordRules = await _settingsService.GetPasswordRulesAsync();

			return Page();
		}

		public async Task<IActionResult> OnPostRegisterApplicationPolicyAsync()
		{
			_logger.LogInformation("Registering user");

			// Inicializace vlastností modelu, jako v OnGetAsync
			AuthenticatorId = await GetDefaultApplicationId();
			if(string.IsNullOrEmpty(ApplicationId))
			{
				ApplicationId = AuthenticatorId;
			}
			else
			{
				ApplicationId = await CheckApplicationId(ApplicationId);
			}

			if(string.IsNullOrEmpty(ApplicationId))
			{
				_logger.LogError("ApplicationId is invalid");
				return RedirectToPage("/Error/404");
			}

			ApplicationInfo = await _applicationService.GetPublicInfoAsync(ApplicationId);
			if(ApplicationInfo == null)
			{
				_logger.LogError("ApplicationInfo is null");
				return RedirectToPage("/Error/404");
			}

			TermsOptions = await _selectOptionsService.GetBinaryOptionsAsync(false);
			PrivacyOptions = await _selectOptionsService.GetBinaryOptionsAsync(true);
			CookieOptions = await _selectOptionsService.GetBinaryOptionsAsync(false);
			PasswordRules = await _settingsService.GetPasswordRulesAsync();

			return Page();
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