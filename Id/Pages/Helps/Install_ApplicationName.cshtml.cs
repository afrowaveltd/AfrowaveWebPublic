namespace Id.Pages.Helps
{
	/// <summary>
	/// The application name is the name of the application that will be displayed in the browser
	/// </summary>
	/// <param name="_t"></param>
	public class Install_ApplicationNameModel(IStringLocalizer<Install_ApplicationNameModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_ApplicationNameModel> t = _t;

		/// <summary>
		/// The application name is the name of the application that will be displayed in the browser
		/// </summary>
		public string Title { get; set; } = "";

		/// <summary>
		/// The application name is the name of the application that will be displayed in the browser
		/// </summary>
		public List<string> Lines = new List<string>();

		/// <summary>
		/// The application name is the name of the application that will be displayed in the browser
		/// </summary>
		public void OnGet()
		{
			Title = t["Application name"];

			Lines.Add(t["The application name is the name of the application that will be displayed in the browser"]);
			Lines.Add(t["The application name is also used in the email templates"]);
			Lines.Add(t["The application name should be unique and descriptive"]);
			Lines.Add(t["The application name must be at least 3 characters long"]);
			Lines.Add(t["The application name must be shorter than 32 characters"]);
		}
	}
}