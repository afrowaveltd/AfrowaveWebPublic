namespace Id.Pages.Helps
{
	/// <summary>
	/// Set the password
	/// </summary>
	/// <param name="_t"></param>
	public class Install_PasswordModel(IStringLocalizer<Install_PasswordModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_PasswordModel> t = _t;

		/// <summary>
		/// The helper title.
		/// </summary>
		public string Title { get; set; } = "";

		/// <summary>
		/// The helper lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// The helper on get method
		/// </summary>
		/// <returns></returns>
		public IActionResult OnGet()
		{
			Title = t["Set the password"];

			Lines.Add(t["This will be a password for the administrator's account."]);
			Lines.Add(t["Make sure it is strong enough."]);

			return Page();
		}
	}
}