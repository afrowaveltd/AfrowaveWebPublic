namespace Id.Pages.Helps
{
	/// <summary>
	/// Represents the model for the SMTP sender email page.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_SmtpEmailModel(IStringLocalizer<Install_SmtpEmailModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpEmailModel> t = _t;

		/// <summary>
		/// Gets or sets the title of the page.
		/// </summary>
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the list of lines.
		/// </summary>
		public List<string> Lines { get; set; } = [];

		/// <summary>
		/// Get method for the SMTP sender email page.
		/// </summary>
		/// <returns></returns>
		public void OnGet()
		{
			Title = t["SMTP Sender Email"];
			Lines.Add(t["Please enter the email address that will be used as the sender email address for the SMTP server."]);
		}
	}
}