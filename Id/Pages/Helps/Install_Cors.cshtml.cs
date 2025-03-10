namespace Id.Pages.Helps
{
	/// <summary>
	/// CORS Settings
	/// </summary>
	/// <param name="_t"></param>
	public class Install_CorsModel(IStringLocalizer<Install_CorsModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_CorsModel> t = _t;

		/// <summary>
		/// Title
		/// </summary>
		public string Title => t["CORS Settings"];

		/// <summary>
		/// Lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// OnGet method
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["CORS is a security feature that restricts what resources a web page can access from another domain."]);
			Lines.Add(t["If you need to allow a client-side application to access this server, you need to allow one or more domains."]);
			Lines.Add(t["When you unselect the domain in the list, it will be removed from the list."]);
			Lines.Add(t["If you are selecting individual methods, you need to allow the following methods:"] + "<b> GET, POST, PUT, DELETE, OPTIONS.</b>");
			Lines.Add(t["If you are selecting individual headers, you need to allow the following headers:"] + "<b> Origin, Content-Type, Accept, Authorization.</b>");
		}
	}
}