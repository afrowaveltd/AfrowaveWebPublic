namespace Id.Pages.Helps
{
	/// <summary>
	/// Represents the model for the SMTP password page.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_SmtpPasswordModel(IStringLocalizer<Install_SmtpPasswordModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpPasswordModel> t = _t;

		/// <summary>
		/// Gets or sets the title of the page.
		/// </summary>
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the list of lines.
		/// </summary>
		public List<string> Lines { get; set; } = [];

		/// <summary>
		/// Get method for the SMTP password page.
		/// </summary>
		/// <returns></returns>
		public void OnGet()
		{
			Title = t["SMTP Password"];

			Lines.Add(t["Please enter the SMTP server password."]);
			Lines.Add(t["This is the password for the SMTP server that will be used to send emails."]);
			Lines.Add(t["If you do not have this information, please contact your email provider."]);
		}
	}
}