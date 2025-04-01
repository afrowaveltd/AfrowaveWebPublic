namespace Id.Pages.Helps
{
	/// <summary>
	/// Email address
	/// </summary>
	/// <param name="_t"></param>
	public class Install_EmailModel(IStringLocalizer<Install_EmailModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_EmailModel> t = _t;

		/// <summary>
		/// Help title
		/// </summary>
		public string Title { get; set; } = "";

		/// <summary>
		/// Lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// OnGet method
		/// </summary>
		/// <returns></returns>
		public IActionResult OnGet()
		{
			Title = t["Email address"];
			Lines.Add(t["The email address is used to send notifications and to recover your password."]);
			Lines.Add(t["It must be a valid email address."]);
			return Page();
		}
	}
}