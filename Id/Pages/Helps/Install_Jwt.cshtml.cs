namespace Id.Pages.Helps
{
	public class Install_JwtModel(IStringLocalizer<Install_JwtModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_JwtModel> t = _t;

		public string Title => t["Jwt settings"];
		public List<string> Lines = [];
		public List<string> LiElements = [];

		public void OnGet()
		{
			Lines.Add(t["Jwt settings are used to store user session information."]);
			Lines.Add(t["This is important for the application to work properly."]);

			LiElements.Add("<b class='information'>" + t["Issuer"] + "</b> " + t["The name of the token issuer."]);
			LiElements.Add("<b class='information'>" + t["Audience"] + "</b> " + t["The domain names valid for the token."]);
			LiElements.Add("<b class='information'>" + _t["Access token expiration"] + "</b> " + _t["The time in minutes the access token is valid."]);
			LiElements.Add("<b class='information'>" + _t["Refresh token expiration"] + "</b> " + _t["The time in days the refresh token is valid."]);
		}
	}
}