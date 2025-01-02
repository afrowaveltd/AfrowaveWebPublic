namespace Id.Pages.Helps
{
	public class Install_SmtpPortModel(IStringLocalizer<Install_SmtpPortModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpPortModel> t = _t;

		public string Title { get; set; } = string.Empty;
		public List<string> Lines { get; set; } = new();

		public async Task OnGetAsync()
		{
			Title = t["SMTP port"];
			Lines.Add(t["The port used to connect to the SMTP server."]);
			Lines.Add(t["The default port is 25."]);
			Lines.Add(t["If you are using a secure TLS connection, the default port is 587."]);
			Lines.Add(t["If you are using a secure SSL connection, the default port is 465."]);
		}
	}
}