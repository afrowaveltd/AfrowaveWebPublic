namespace Id.Pages.Helps
{
	public class Install_SmtpSecure_Model(IStringLocalizer<Install_SmtpSecure_Model> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpSecure_Model> t = _t;

		public string Title { get; set; } = string.Empty;
		public List<string> Lines { get; set; } = new();
		public List<string> Options { get; set; } = new();

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