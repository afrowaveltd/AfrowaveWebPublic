namespace Id.Pages.Helps
{
	/// <summary>
	/// Help for the confirm password.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_ConfirmPasswordModel(IStringLocalizer<Install_ConfirmPasswordModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_ConfirmPasswordModel> t = _t;

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
			Title = t["Confirm password"];
			Lines.Add(t["Please confirm the password you have set."]);
		}
	}
}