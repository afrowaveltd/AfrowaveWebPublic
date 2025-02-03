namespace Id.Pages.Helps
{
	public class Account_PasswordModel(IStringLocalizer<Account_PasswordModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_PasswordModel> t = _t;
		public string Title => t["Password"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The password is used to login into your account."]);
			Lines.Add(t["Make sure it is strong enough."]);
			Lines.Add(t["You can use a password manager to generate and store your password."]);
			Lines.Add(t["If you forget your password, you can recover it by using the forgot password feature."]);
			Lines.Add(t["In the window below you can see if your password is strong enough."]);
		}
	}
}