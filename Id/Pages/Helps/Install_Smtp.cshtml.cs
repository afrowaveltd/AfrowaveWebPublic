namespace Id.Pages.Helps
{
	public class Install_SmtpModel(IStringLocalizer<Install_SmtpModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpModel> t = _t;
		public string Title { get; set; } = string.Empty;
		public List<string> Lines { get; set; } = new();

		public async Task OnGetAsync()
		{
			Title = t["Smtp settings"];
			Lines.Add(t["To send emails, you need to configure the SMTP settings."]);
			Lines.Add(t["The SMTP settings are used to send emails from the application."]);
		}
	}
}