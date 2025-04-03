namespace Id.Pages.Account
{
	/// <summary>
	/// Model for the authenticator user registration
	/// </summary>
	/// <remarks>
	/// Initializing new class instance<see cref="AuthenticatorUserRegistrationModel"/>.
	/// </remarks>
	/// <param name="t">String localizer.</param>
	/// <param name="context">Application database context.</param>
	/// <param name="applicationUsersManager">Application users manager.</param>
	/// <param name="logger">Logger.</param>
	public class AuthenticatorUserRegistrationModel(IStringLocalizer<AuthenticatorUserRegistrationModel> t,
		ApplicationDbContext context,
		IApplicationUsersManager applicationUsersManager,
		ILogger<AuthenticatorUserRegistrationModel> logger) : PageModel
	{
		private readonly IStringLocalizer<AuthenticatorUserRegistrationModel> _t = t;
		private readonly ILogger<AuthenticatorUserRegistrationModel> _logger = logger;
		private readonly ApplicationDbContext _context = context;
		private readonly IApplicationUsersManager _applicationUsersManager = applicationUsersManager;

		/// <summary>
		/// Gets or sets the authenticator ID.
		/// </summary>
		[BindProperty(SupportsGet = true)]
		public string AuthenticatorId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the application ID.
		/// </summary>
		[BindProperty(SupportsGet = true)]
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user ID.
		/// </summary>
		[BindProperty(SupportsGet = true)]
		public string UserId { get; set; } = string.Empty;

		/// <summary>
		/// Get information if profile picture was uploaded.
		/// </summary>

		[BindProperty(SupportsGet = true)]
		public bool ProfilePictureUploaded { get; set; }

		/// <summary>
		/// Gets or sets information if the user can register.
		/// Depends on supplied authenticator ID and user ID.
		/// </summary>
		public bool CanRegister { get; set; } = true;

		/// <summary>
		/// Gets or sets the error message.
		/// Presented only if the user cannot register.
		/// </summary>
		public string Error { get; set; } = string.Empty;

		/// <summary>
		/// Holds input data for registering an application user, including consent flags for sharing details, cookies, and
		/// terms. Initializes with default values.
		/// </summary>
		public RegisterApplicationUserInput Input { get; set; } = new RegisterApplicationUserInput()
		{
			AgreedSharingUserDetails = true,
			AgreedToCookies = true,
			AgreedToTerms = true,
			UserDescription = string.Empty,
		};

		/// <summary>
		/// Deals with the GET method.
		/// </summary>
		/// <returns>The action result.</returns>
		public async Task<IActionResult> OnGetAsync()
		{
			// Check if the authenticator ID is valid
			if(string.IsNullOrEmpty(AuthenticatorId) || string.IsNullOrEmpty(UserId))
			{
				_logger.LogError("Invalid AuthenticatorId or UserId ");
				Error = _t["Invalid Authenticator or User data"];
				CanRegister = false;
				return Page();
			}

			User? user = await _context.Users.FindAsync(UserId);
			Application? authenticator = await _context.Applications.FindAsync(AuthenticatorId);

			if(user == null)
			{
				// user was not found
				_logger.LogError("User not found: {UserId}", UserId);
				Error = _t["User not found"];
				CanRegister = false;
				return Page();
			}

			if(authenticator == null)
			{
				// authenticator was not found
				_logger.LogError("Authenticator not found: {AuthenticatorId}", AuthenticatorId);
				Error = _t["Authenticating application not found"];
				CanRegister = false;
				return Page();
			}

			if(await _context.ApplicationUsers.Where(s => s.ApplicationId == AuthenticatorId).AnyAsync(s => s.User == user))
			{
				// user already registered
				_logger.LogError("User already registered: {UserId}", UserId);
				Error = _t["User already exists"];
				CanRegister = false;
				return Page();
			}

			Input.ApplicationId = ApplicationId;
			Input.UserId = UserId;

			return Page();
		}
	}
}