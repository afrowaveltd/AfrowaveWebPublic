namespace Id.Pages.Helps
{
	/// <summary>
	/// The brand is the name of the company or organization that owns the application
	/// </summary>
	/// <param name="_t"></param>
	public class Install_BrandModel(IStringLocalizer<Install_BrandModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_BrandModel> t = _t;

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
			Title = t["Brand"];
			Lines.Add(t["The brand is the name of the company or organization that owns the application"]);
			Lines.Add(t["On this page you can enter the name of the brand, a logo, a website, a description and an email address"]);
		}
	}
}