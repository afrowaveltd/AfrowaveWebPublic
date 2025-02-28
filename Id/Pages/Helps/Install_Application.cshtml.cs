namespace Id.Pages.Helps
{
	/// <summary>
	/// This class is used to provide the help information for the 'Install_Application' page.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_ApplicationModel(IStringLocalizer<Install_ApplicationModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_ApplicationModel> t = _t;

		/// <summary>
		/// The title of the page.
		/// </summary>
		public string Title { get; set; } = "";

		/// <summary>
		/// The list of lines that will be displayed on the page.
		/// </summary>
		public List<string> Lines = new List<string>();

		/// <summary>
		/// This method is called when the 'Install_Application' page is requested.
		/// </summary>
		public void OnGet()
		{
			Title = t["Create Authenticator Application"];

			Lines.Add(t["To create an authenticator application, you need to fill the form properly and click on the 'Next' button"]);
			Lines.Add(t["For the security reason, use the dedicated email address for the administrator account"]);
			Lines.Add(t["The administrator account is the first account that will be created"]);
		}
	}
}