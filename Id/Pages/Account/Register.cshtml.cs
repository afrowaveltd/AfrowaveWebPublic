using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Account
{
	/// <summary>
	/// Represents the model for the user registration page.
	/// </summary>
	/// <param name="logger">Logger</param>
	/// <param name="userService">UserService</param>
	/// <param name="applicationUsersManager">ApplicationUsersManager</param>
	/// <param name="applicationService">ApplicationService</param>
	/// <param name="encryptionService">EncryptionService</param>
	/// <param name="roleService">RoleService</param>
	/// <param name="selectOptionsService">SelectOptionsService</param>
	/// <param name="settingsService">SettingsService</param>
	/// <param name="_t">Localization</param>
	/// <completionlist cref="RegisterUserModel" />
	/// <completionlist cref="ILogger" />

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

		/// <summary>
		/// Gets the authenticator id.
		/// </summary>
		public string AuthenticatorId => _applicationService.GetAuthenticatorIdAsync().GetAwaiter().GetResult();

		/// <summary>
		/// Gets the application information.
		/// </summary>
		public ApplicationView ApplicationInfo => _applicationService.GetInfoAsync(AuthenticatorId).GetAwaiter().GetResult() ?? new();

		/// <summary>
		/// Gets the list of privacy options.
		/// </summary>
		public List<SelectListItem> PrivacyOptions => _selectOptionsService.GetBinaryOptionsAsync(true).Result;

		/// <summary>
		/// Gets the list of terms options.
		/// </summary>
		public List<SelectListItem> TermsOptions => _selectOptionsService.GetBinaryOptionsAsync(false).Result;

		/// <summary>
		/// Gets the list of gender options
		/// </summary>
		public List<SelectListItem> GenderOptions => _selectOptionsService.GetGendersAsync("Other").Result;

		/// <summary>
		/// Gets the list of cookie options.
		/// </summary>
		public List<SelectListItem> CookieOptions => _selectOptionsService.GetBinaryOptionsAsync(false).Result;

		/// <summary>
		/// The list of registration errors.
		/// </summary>
		public List<string> RegistrationErrors = [];

		/// <summary>
		/// The list of registration warnings.
		/// </summary>
		public List<string> RegistrationWarnings = [];

		/// <summary>
		/// Gets the password rules.
		/// </summary>
		/// <completionlist cref="PasswordRules" />
		public PasswordRules PasswordRules => _settingsService.GetPasswordRulesAsync().Result;

		/// <summary>
		/// Gets the login rules.
		/// </summary>
		/// <completionlist cref="LoginRules" />
		public LoginRules LoginRules => _settingsService.GetLoginRulesAsync().Result;

		/// <summary>
		/// Gets or sets the user input.
		/// </summary>
		/// <completionlist cref="RegisterUserInput" />
		[BindProperty]
		public RegisterUserInput Input { get; set; } = new();

		/// <summary>
		/// Gets or sets the application id.
		/// </summary>
		/// <completionlist cref="ApplicationId" />
		[FromRoute]
		public string? ApplicationId { get; set; }

		/// <summary>
		/// Handles the GET request.
		/// </summary>
		/// <returns>The user registration page</returns>
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

		/// <summary>
		/// Handles the POST request.
		/// </summary>
		/// <returns>Registration result</returns>
		public async Task<IActionResult> OnPostAsync()
		{
			_logger.LogInformation("Registering user");
			if(ModelState.IsValid)
			{
				// To do: Implement the registration logic
				RegisterUserResult registrationResult = await _userService.RegisterUserAsync(Input);
				if(!registrationResult.UserCreated)
				{
					_logger.LogError("User registration failed: {Errors}", string.Join(", ", registrationResult.Errors ?? new List<string> { "Unknown error" }));
					RegistrationErrors = registrationResult.Errors ?? new List<string>();
					foreach(var error in RegistrationErrors)
					{
						ModelState.AddModelError(string.Empty, error ?? "Unknown registration error");
					}
					return Page();
				}

				return RedirectToPage(
					"/Account/AuthenticatorUserRegistration",
					new
					{
						authenticatorId = AuthenticatorId,
						userId = registrationResult.UserId,
						profilePictureUploaded = registrationResult.ProfilePictureUploaded,
						applicationId = ApplicationId
					});
			}
			else
			{
				RegistrationErrors = [.. ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)];
				_logger.LogError("User registration failed: {Errors}", string.Join(", ", RegistrationErrors));
				ModelState.AddModelError("RegistrationError", t["User registration failed"]);
				return Page();
			}
		}
	}
}