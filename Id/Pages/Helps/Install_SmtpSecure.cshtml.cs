namespace Id.Pages.Helps
{
	/// <summary>
	/// SMTP Secure socket options
	/// </summary>
	/// <param name="_t"></param>
	public class Install_SmtpSecure_Model(IStringLocalizer<Install_SmtpSecure_Model> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpSecure_Model> t = _t;

		/// <summary>
		/// Help title
		/// </summary>
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// Help lines
		/// </summary>
		public List<string> Lines { get; set; } = [];

		/// <summary>
		/// Secure socket options
		/// </summary>
		public List<string> Options { get; set; } = [];

		/// <summary>
		/// OnGet method
		/// </summary>
		public void OnGet()
		{
			Title = t["SMTP Secure socket options"];
			Lines.Add(t["The secure socket options are used to determine how the connection to the SMTP server should be secured."]);
			Lines.Add(t["The options are:"]);
			Options.Add($"None: {t["No security is used."]}");
			Options.Add($"Auto: {t["The client will use the best security option available."]}");
			Options.Add($"SslOnConnect: {t["The client will use SSL/TLS immediately after connecting."]}");
			Options.Add($"StartTls: {t["The client will use the STARTTLS command to switch to a secure connection."]}");
		}
	}
}