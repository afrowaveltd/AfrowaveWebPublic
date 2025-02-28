namespace Id.Pages.Helps
{
	/// <summary>
	/// The time in days the refresh token is valid.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_LoginRules_RefreshTokenExpirationModel(IStringLocalizer<Install_LoginRules_RefreshTokenExpirationModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_RefreshTokenExpirationModel> t = _t;

		/// <summary>
		/// The helper title.
		/// </summary>
		public string Title => t["Refresh token expiration"];

		/// <summary>
		/// The helper lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// The helper on get method
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The time in days the refresh token is valid."]);
			Lines.Add(t["If you set this value to 0, the token will never expire"]);
		}
	}
}