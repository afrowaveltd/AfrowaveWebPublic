namespace Id.Pages.Helps
{
	/// <summary>
	/// The application website is the URL of the website where the application is hosted
	/// </summary>
	/// <param name="_t"></param>
	public class Install_ApplicationWebsiteModel(IStringLocalizer<Install_ApplicationWebsiteModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_ApplicationWebsiteModel> t = _t;

		/// <summary>
		/// Title of the page
		/// </summary>
		public string Title { get; set; } = "";

		/// <summary>
		/// Lines of the page
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// The OnGet method
		/// </summary>
		public void OnGet()
		{
			Title = t["Application Website"];
			Lines.Add(t["The application website is the URL of the website where the application is hosted"]);
		}
	}
}