namespace Id.Pages.Helps
{
	public class Install_SmtpEmailModel(IStringLocalizer<Install_SmtpEmailModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpEmailModel> t = _t;

		public string Title { get; set; } = string.Empty;
		public List<string> Lines { get; set; } = new();

		public async Task OnGetAsync()
		{
			Title = t["SMTP Sender Email"];
			Lines.Add(t["Please enter the email address that will be used as the sender email address for the SMTP server."]);
		}
	}
}