namespace Id.Pages.Helps
{
	public class Install_SmtpSenderModel(IStringLocalizer<Install_SmtpSenderModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpSenderModel> t = _t;

		public string Title { get; set; } = string.Empty;
		public List<string> Lines { get; set; } = new();

		public async Task OnGetAsync()
		{
			Title = t["SMTP Sender"];
			Lines.Add(t["The SMTP sender is the name that will be displayed in sent emails."]);
		}
	}
}