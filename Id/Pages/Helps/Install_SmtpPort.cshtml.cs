namespace Id.Pages.Helps
{
	/// <summary>
	/// SMTP port
	/// </summary>
	/// <param name="_t"></param>
	public class Install_SmtpPortModel(IStringLocalizer<Install_SmtpPortModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpPortModel> t = _t;

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
			Title = t["SMTP port"];
			Lines.Add(t["The port used to connect to the SMTP server."]);
			Lines.Add(t["The default port is 25."]);
			Lines.Add(t["If you are using a secure TLS connection, the default port is 587."]);
			Lines.Add(t["If you are using a secure SSL connection, the default port is 465."]);
		}
	}
}