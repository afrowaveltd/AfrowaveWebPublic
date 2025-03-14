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
	/// <param name="logger">Logger.</param>
	public class AuthenticatorUserRegistrationModel(IStringLocalizer<AuthenticatorUserRegistrationModel> t,
		ApplicationDbContext context,
		ILogger<AuthenticatorUserRegistrationModel> logger) : PageModel
	{
		private readonly IStringLocalizer<AuthenticatorUserRegistrationModel> _t = t;
		private readonly ILogger<AuthenticatorUserRegistrationModel> _logger = logger;
		private readonly ApplicationDbContext _context = context;

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
		/// Deals with the GET method.
		/// </summary>
		/// <returns>The action result.</returns>
		public async Task<IActionResult> OnGetAsync()
		{
			// Check if the authenticator ID is valid
			if (string.IsNullOrEmpty(AuthenticatorId) || string.IsNullOrEmpty(UserId))
			{
				_logger.LogError("ApplicationId nebo UserId je neplatné");
				return RedirectToPage("/Error/404");
			}
			User? user = await _context.Users.FindAsync(UserId);

			return Page();
		}
	}
}