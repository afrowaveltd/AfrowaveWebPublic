namespace Id.Pages.Helps
{
	public class Install_CookieModel(IStringLocalizer<Install_CookieModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_CookieModel> t = _t;

		public string Title => t["Cookie settings"];
		public List<string> Lines = [];
		public List<string> LiElements = [];

		public void OnGet()
		{
			Lines.Add(t["Cookie settings are used to store user session information."]);
			Lines.Add(t["This is important for the application to work properly."]);

			LiElements.Add("<b class='information'>" + t["Cookie name"] + "</b> " + t["The name of the cookie."]);
			LiElements.Add("<b class='information'>" + t["Cookie domain"] + "</b> " + t["The domain of the cookie."]);
			LiElements.Add("<b class='information'>" + t["Cookie path"] + "</b> " + t["The path of the cookie."]);
			LiElements.Add("<b class='information'>" + t["Secure cookie"] + "</b> " + t["If set to yes, the cookie is only sent over HTTPS."]);
			LiElements.Add("<b class='information'>" + t["Same Site cookie"] + "</b> " + t["The SameSite attribute of the cookie."]);
			LiElements.Add("<b class='information'>" + t["HTTP only cookie"] + "</b> " + t["If set to yes, the cookie is not accessible via JavaScript."]);
			LiElements.Add("<b class='information'>" + t["Cookie expiration (min)"] + "</b> " + t["The time in minutes the cookie is valid."]);
		}
	}
}