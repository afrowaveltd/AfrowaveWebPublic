namespace Id.Pages.Helps
{
	/// <summary>
	/// Jwt settings are used to store user session information.
	/// </summary>
	/// <param name="t">Localizer</param>
	public class Install_JwtModel(IStringLocalizer<Install_JwtModel> t) : PageModel
	{
		private readonly IStringLocalizer<Install_JwtModel> _t = t;

		/// <summary>
		/// The title of the page.
		/// </summary>
		public string Title => _t["Jwt settings"];

		/// <summary>
		/// The description of the page.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// The list of LI elements.
		/// </summary>
		public List<string> LiElements = [];

		/// <summary>
		/// The OnGet method.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(_t["Jwt settings are used to store user session information."]);
			Lines.Add(_t["This is important for the application to work properly."]);

			LiElements.Add("<b class='information'>" + _t["Issuer"] + "</b> " + _t["The name of the token issuer."]);
			LiElements.Add("<b class='information'>" + _t["Audience"] + "</b> " + _t["The domain names valid for the token."]);
			LiElements.Add("<b class='information'>" + _t["Access token expiration"] + "</b> " + _t["The time in minutes the access token is valid."]);
			LiElements.Add("<b class='information'>" + _t["Refresh token expiration"] + "</b> " + _t["The time in days the refresh token is valid."]);
		}
	}
}