namespace Id.Pages.Helps
{
	/// <summary>
	/// Access token expiration
	/// </summary>
	/// <param name="_t"></param>
	public class Install_LoginRules_AccessTokenExpirationModel(IStringLocalizer<Install_LoginRules_AccessTokenExpirationModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_AccessTokenExpirationModel> t = _t;

		/// <summary>
		/// Helper title
		/// </summary>
		public string Title => t["Access token expiration"];

		/// <summary>
		/// Helper lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// On get method
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The time in minutes the access token is valid."]);
			Lines.Add(t["If you set this value to 0, the token will never expire"]);
		}
	}
}