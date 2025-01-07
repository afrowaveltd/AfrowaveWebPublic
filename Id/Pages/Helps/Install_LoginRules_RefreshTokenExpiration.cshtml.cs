namespace Id.Pages.Helps
{
	public class Install_LoginRules_RefreshTokenExpirationModel(IStringLocalizer<Install_LoginRules_RefreshTokenExpirationModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_RefreshTokenExpirationModel> t = _t;
		public string Title => t["Refresh token expiration"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The time in days the refresh token is valid."]);
			Lines.Add(t["If you set this value to 0, the token will never expire"]);
		}
	}
}