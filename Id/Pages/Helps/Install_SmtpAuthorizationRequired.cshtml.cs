namespace Id.Pages.Helps
{
	public class Install_SmtpAuthorizationRequiredModel(IStringLocalizer<Install_SmtpAuthorizationRequiredModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpAuthorizationRequiredModel> t = _t;

		public string Title { get; set; } = string.Empty;
		public List<string> Lines { get; set; } = new();

		public void OnGet()
		{
			Title = t["SMTP Authorization Required"];
			Lines.Add(t["If the SMTP server requires authorization, you must provide a username and password."]);
		}
	}
}