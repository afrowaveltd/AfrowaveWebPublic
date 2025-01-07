namespace Id.Pages.Helps
{
	public class Install_LoginRules_EmailConfirmationTokenExpirationModel(IStringLocalizer<Install_LoginRules_EmailConfirmationTokenExpirationModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_EmailConfirmationTokenExpirationModel> t = _t;

		public string Title => t["Email confirmation token expiration"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The time in minutes the email confirmation token is valid."]);
			Lines.Add(t["If you set this value to 0, the token will never expire"]);
		}
	}
}