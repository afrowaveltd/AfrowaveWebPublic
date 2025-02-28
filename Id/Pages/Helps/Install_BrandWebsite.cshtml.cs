namespace Id.Pages.Helps
{
	/// <summary>
	/// Help for the brand website.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_BrandWebsiteModel(IStringLocalizer<Install_BrandWebsiteModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_BrandWebsiteModel> t = _t;

		/// <summary>
		/// The title of the help.
		/// </summary>
		public string Title { get; set; } = "";

		/// <summary>
		/// The lines of the help.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Called when the page is requested.
		/// </summary>
		public void OnGet()
		{
			Title = t["Brand website"];
			Lines.Add(t["The brand website is the URL of the website of the company or organization that owns the application"]);
			Lines.Add(t["On this page you can enter the URL of the brand website"]);
		}
	}
}