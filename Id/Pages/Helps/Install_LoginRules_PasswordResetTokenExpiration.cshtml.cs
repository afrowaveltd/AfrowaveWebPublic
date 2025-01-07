namespace Id.Pages.Helps
{
	public class Install_LoginRules_PasswordResetTokenExpirationModel(IStringLocalizer<Install_LoginRules_PasswordResetTokenExpirationModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_PasswordResetTokenExpirationModel> t = _t;

		public string Title => t["Password reset token expiration"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The time in minutes the password reset token is valid."]);
			Lines.Add(t["If you set this value to 0, the token will never expire"]);
		}
	}
}