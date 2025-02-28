namespace Id.Pages.Helps
{
	/// <summary>
	/// Password reset token expiration
	/// </summary>
	/// <param name="_t"></param>
	public class Install_LoginRules_PasswordResetTokenExpirationModel(IStringLocalizer<Install_LoginRules_PasswordResetTokenExpirationModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_PasswordResetTokenExpirationModel> t = _t;

		/// <summary>
		/// Helper title
		/// </summary>
		public string Title => t["Password reset token expiration"];

		/// <summary>
		/// Helper lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// On get method
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The time in minutes the password reset token is valid."]);
			Lines.Add(t["If you set this value to 0, the token will never expire"]);
		}
	}
}