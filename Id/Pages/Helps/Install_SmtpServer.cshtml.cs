namespace Id.Pages.Helps
{
	public class Install_SmtpServerModel(IStringLocalizer<Install_SmtpServerModel> _t)
	  : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpServerModel> t = _t;
		public string Title { get; set; } = string.Empty;
		public List<string> Lines { get; set; } = new();

		public async Task OnGetAsync()
		{
			Title = t["SMTP Server"];
			Lines.Add(t["To send emails, you need to configure the SMTP server."]);
			Lines.Add(t["The SMTP server is your outgoing mail server."]);
		}
	}
}