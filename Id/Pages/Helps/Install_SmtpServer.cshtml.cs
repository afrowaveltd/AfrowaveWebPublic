namespace Id.Pages.Helps
{
	/// <summary>
	/// SMTP Server
	/// </summary>
	/// <param name="_t"></param>
	public class Install_SmtpServerModel(IStringLocalizer<Install_SmtpServerModel> _t)
	  : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpServerModel> t = _t;

		/// <summary>
		/// Help title
		/// </summary>
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// Help lines
		/// </summary>
		public List<string> Lines { get; set; } = [];

		/// <summary>
		/// OnGet method
		/// </summary>
		/// <returns></returns>

		public void OnGet()
		{
			Title = t["SMTP Server"];
			Lines.Add(t["To send emails, you need to configure the SMTP server."]);
			Lines.Add(t["The SMTP server is your outgoing mail server."]);
		}
	}
}