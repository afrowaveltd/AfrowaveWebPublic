namespace Id.Pages.Helps
{
	/// <summary>
	/// Represents the model for the SMTP installation page.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_SmtpModel(IStringLocalizer<Install_SmtpModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpModel> t = _t;

		/// <summary>
		/// Gets or sets the title of the page.
		/// </summary>
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the list of lines.
		/// </summary>
		public List<string> Lines { get; set; } = [];

		/// <summary>
		/// Get method for the SMTP installation page.
		/// </summary>
		/// <returns></returns>
		public void OnGet()
		{
			Title = t["Smtp settings"];
			Lines.Add(t["To send emails, you need to configure the SMTP settings."]);
			Lines.Add(t["The SMTP settings are used to send emails from the application."]);
		}
	}
}