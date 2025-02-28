namespace Id.Pages.Helps
{
	/// <summary>
	/// SMTP Username
	/// </summary>
	/// <param name="_t"></param>
	public class Install_SmtpUserModel(IStringLocalizer<Install_SmtpUserModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpUserModel> t = _t;

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
		public void OnGet()
		{
			Title = t["SMTP Username"];
			Lines.Add(t["Please enter the SMTP server username."]);
			Lines.Add(t["This is the login information for the SMTP server that will be used to send emails."]);
			Lines.Add(t["If you do not have this information, please contact your email provider."]);
		}
	}
}