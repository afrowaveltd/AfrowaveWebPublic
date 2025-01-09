namespace Id.Pages.Helps
{
	public class Install_PasswordRulesModel(IStringLocalizer<Install_PasswordRulesModel> _t) : PageModel
	{
		public readonly IStringLocalizer<Install_PasswordRulesModel> t = _t;

		public string Title => t["Password settings"];
		public List<string> Lines { get; set; } = [];

		public void OnGet()
		{
			Lines.Add(t["Here you can setup the password rules for the users."]);
			Lines.Add(t["The password rules are used to enforce a certain level of complexity for the passwords."]);
			Lines.Add(t["The password rules are applied when a user creates a new password or changes an existing password."]);
			Lines.Add(t["Default values are fair balance between the complexity and the security"]);
		}
	}
}