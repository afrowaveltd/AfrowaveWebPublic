namespace Id.Pages.Helps
{
	/// <summary>
	/// Represents the model for the SMTP authorization required page.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_SmtpAuthorizationRequiredModel(IStringLocalizer<Install_SmtpAuthorizationRequiredModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpAuthorizationRequiredModel> t = _t;

		/// <summary>
		/// Gets or sets the title of the page.
		/// </summary>
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the list of lines.
		/// </summary>
		public List<string> Lines { get; set; } = [];

		/// <summary>
		/// Get method for the SMTP authorization required page.
		/// </summary>
		public void OnGet()
		{
			Title = t["SMTP Authorization Required"];
			Lines.Add(t["If the SMTP server requires authorization, you must provide a username and password."]);
		}
	}
}