namespace Id.Pages.Helps
{
	/// <summary>
	/// Help for the brand email.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_BrandEmailModel(IStringLocalizer<Install_BrandEmailModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_BrandEmailModel> t = _t;

		/// <summary>
		/// The title of the help.
		/// </summary>
		public string Title => t["Brand email"];

		/// <summary>
		/// The lines of the help.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Called when the page is requested.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["This should be a valid email address."]);
			Lines.Add(t["This email address will be used to send emails to users."]);
			Lines.Add(t["It will be public for users to see."]);
		}
	}
}