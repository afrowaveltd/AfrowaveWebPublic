namespace Id.Pages.Helps
{
	/// <summary>
	/// Configure login rules
	/// </summary>
	/// <param name="t"></param>
	public class Install_LoginModel(IStringLocalizer<Install_LoginModel> t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginModel> _t = t;

		/// <summary>
		/// Title
		/// </summary>
		public string Title => _t["Configure login rules"];

		/// <summary>
		/// Lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Li elements
		/// </summary>
		public List<string> LiElements = [];

		/// <summary>
		/// On get
		/// </summary>
		/// <returns></returns>
		public IActionResult OnGet()
		{
			Lines.Add(_t["The login rules are used to configure the login process. The following rules are available:"]);

			LiElements.Add("<b class='information'>" + _t["Maximal failed login attempts"] + "</b> " + _t["The maximum number of failed login attempts before the account is locked."]);
			LiElements.Add("<b class='information'>" + _t["Lockout time"] + "</b> " + _t["The time in minutes the account is locked after the maximum number of failed login attempts"]);
			LiElements.Add("<b class='information'>" + _t["Password reset token expiration"] + "</b> " + _t["The time in minutes the password reset token is valid."]);
			LiElements.Add("<b class='information'>" + _t["Email confirmation token expiration"] + "</b> " + _t["The time in minutes the email confirmation token is valid."]);
			LiElements.Add("<b class='information'>" + _t["Require confirmed email"] + "</b> " + _t["If true, the user must confirm the email address before logging in."]);

			return Page();
		}
	}
}