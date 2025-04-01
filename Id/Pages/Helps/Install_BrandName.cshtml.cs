namespace Id.Pages.Helps
{
	/// <summary>
	/// Help for the brand name.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_BrandNameModel(IStringLocalizer<Install_BrandNameModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_BrandNameModel> t = _t;

		/// <summary>
		/// The title of the help.
		/// </summary>
		public string Title => t["Brand name"];

		/// <summary>
		/// The lines of the help.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Called when the page is requested.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The Brand name is a unique identifier for your company or the mark under which you create your software."]);
			Lines.Add(t["It is used in the title of the application."]);
			Lines.Add(t["It is also used in the title of the emails sent by the application."]);
			Lines.Add(t["Brand name should have between 2 and 50 characters."]);
			Lines.Add(t["Don't use abusive or vulgar brand names."]);
		}
	}
}