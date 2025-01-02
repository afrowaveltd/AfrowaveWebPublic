namespace Id.Pages.Helps
{
	public class Install_SmtpPasswordModel(IStringLocalizer<Install_SmtpPasswordModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpPasswordModel> t = _t;

		public string Title { get; set; } = string.Empty;
		public List<string> Lines { get; set; } = new();

		public async Task OnGetAsync()
		{
			Title = t["SMTP Password"];

			Lines.Add(t["Please enter the SMTP server password."]);
			Lines.Add(t["This is the password for the SMTP server that will be used to send emails."]);
			Lines.Add(t["If you do not have this information, please contact your email provider."]);
		}
	}
}