namespace Id.Pages.Helps
{
	public class Install_LoginRules_AccessTokenExpirationModel(IStringLocalizer<Install_LoginRules_AccessTokenExpirationModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_AccessTokenExpirationModel> t = _t;

		public string Title => t["Access token expiration"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The time in minutes the access token is valid."]);
			Lines.Add(t["If you set this value to 0, the token will never expire"]);
		}
	}
}