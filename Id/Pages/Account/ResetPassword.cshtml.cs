namespace Id.Pages.Account
{
	/// <summary>
	/// Represents the model for the reset password page.
	/// </summary>
	/// <param name="emailService">Email manager</param>
	/// <param name="userService">Users manager</param>
	/// <param name="t">localization service</param>
	public class ResetPasswordModel(IEmailManager emailService,
		IUsersManager userService,
		IStringLocalizer<ResetPasswordModel> t) : PageModel
	{
		private readonly IEmailManager _emailService = emailService;
		private readonly IUsersManager _userService = userService;
		private readonly IStringLocalizer<ResetPasswordModel> _t = t;

		/// <summary>
		/// Gets or sets the email address.
		/// </summary>
		[FromRoute]
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// Handles the GET request.
		/// </summary>
		/// <returns>The password reset page</returns>
		public ActionResult OnGet()
		{
			return Page();
		}

		/// <summary>
		/// Handles the POST request.
		/// </summary>
		/// <param name="email">Email where to send password reset link</param>
		/// <returns>Redirect to the reset result page</returns>
		public ActionResult OnPost(string email)
		{
			return Page();
		}
	}
}