namespace Id.Pages.Account
{
	/// <summary>
	/// Model for the authenticator user registration
	/// </summary>
	/// <remarks>
	/// Initializing new class instance<see cref="ApplicationUserRegistrationModel"/>.
	/// </remarks>
	/// <param name="t">String localizer.</param>
	/// <param name="logger">Logger.</param>
	public class ApplicationUserRegistrationModel(IStringLocalizer<ApplicationUserRegistrationModel> t,
			 ILogger<ApplicationUserRegistrationModel> logger) : PageModel
	{
		private readonly IStringLocalizer<ApplicationUserRegistrationModel> _t = t;
		private readonly ILogger<ApplicationUserRegistrationModel> _logger = logger;

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
		/// Zpracovává požadavek GET.
		/// </summary>
		/// <returns>Výsledek akce.</returns>
		public async Task<IActionResult> OnGetAsync()
		{
			if(string.IsNullOrEmpty(AuthenticatorId) || string.IsNullOrEmpty(UserId))
			{
				_logger.LogError("ApplicationId nebo UserId je neplatné");
				return RedirectToPage("/Error/404");
			}

			return Page();
		}
	}
}