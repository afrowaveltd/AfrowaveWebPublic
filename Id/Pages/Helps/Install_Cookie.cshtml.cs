namespace Id.Pages.Helps
{
	/// <summary>
	/// Cookie settings
	/// </summary>
	/// <param name="_t"></param>
	public class Install_CookieModel(IStringLocalizer<Install_CookieModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_CookieModel> t = _t;

		/// <summary>
		/// Title
		/// </summary>
		public string Title => t["Cookie settings"];

		/// <summary>
		/// Lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Li elements
		/// </summary>
		public List<string> LiElements = [];

		/// <summary>
		/// OnGet method
		/// </summary>
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