namespace Id.Pages.Helps
{
	public class Account_EmailModel(IStringLocalizer<Account_EmailModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_EmailModel> t = _t;
		public string Title => t["Email address"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The email address is used to login into your account."]);
			Lines.Add(t["It is also used to send notifications and to recover your account."]);
			Lines.Add(t["Make sure to use the valid email address. You might receive an activation email"]);
		}
	}
}