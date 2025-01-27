using Id.Models.CommunicationModels;

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
		private readonly IStringLocalizer<RegisterUserModel> t = _t;

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
				ApplicationId = await GetDefaultApplicationId();
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
				return RedirectToPage("./QuickRegistration/" + ApplicationId);
			}
			else
			{
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