namespace Id.Pages.Helps
{
	/// <summary>
	/// Create administrator's account
	/// </summary>
	/// <param name="_t"></param>
	public class Install_CreateAdminModel(IStringLocalizer<Install_CreateAdminModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_CreateAdminModel> t = _t;

		/// <summary>
		/// Title for help
		/// </summary>
		public string Title2 { get; set; } = "";

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
			Title2 = t["Create administrator's account"];

			Lines.Add(t["To create an administrator's account, fill the form properly and click on the 'Next' button"]);

			return Page();
		}
	}
}