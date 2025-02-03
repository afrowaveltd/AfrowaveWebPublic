namespace Id.Pages.Helps
{
	public class Account_RegisterUserModel(IStringLocalizer<Account_RegisterUserModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_RegisterUserModel> t = _t;
		public string Title => t["Register"];
		public List<string> Lines { get; set; } = [];

		public void OnGet()
		{
			Lines.Add(t["Enter the email address, which you want to register into the application"]);
			Lines.Add($"{t["If you are already registered, you can"]} <a href='/Login'> {t["login-"]} </a>");
			Lines.Add($"{t["If you forgot the password"]} <a href='/ForgottenPassword'> {t["click here"]}</a>");
		}
	}
}