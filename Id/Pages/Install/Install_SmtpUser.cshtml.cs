namespace Id.Pages.Install
{
	public class Install_SmtpUserModel(IStringLocalizer<Install_SmtpUserModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpUserModel> t = _t;

		public string Title { get; set; } = string.Empty;
		public List<string> Lines { get; set; } = new();

		public async Task OnGetAsync()
		{
			Title = t["SMTP Login"];

			Lines.Add(t["Please enter the SMTP server login details."]);
			Lines.Add(t["This is the login information for the SMTP server that will be used to send emails."]);
			Lines.Add(t["If you do not have this information, please contact your email provider."]);
		}
	}
}