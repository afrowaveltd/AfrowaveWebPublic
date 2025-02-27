namespace Id.Pages.Helps
{
	/// <summary>
	/// This page is used to help the user to understand the registration.
	/// </summary>
	/// <param name="_t"></param>
	public class Account_RegisterUserModel(IStringLocalizer<Account_RegisterUserModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_RegisterUserModel> t = _t;

		/// <summary>
		/// The title of the page.
		/// </summary>
		public string Title => t["Register"];

		/// <summary>
		/// The lines of the page.
		/// </summary>
		public List<string> Lines { get; set; } = [];

		/// <summary>
		/// OnGet method of the page.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["Enter the email address, which you want to register into the application"]);
			Lines.Add($"{t["If you are already registered, you can"]} <a href='/Login'> {t["login-"]} </a>");
			Lines.Add($"{t["If you forgot the password"]} <a href='/ForgottenPassword'> {t["click here"]}</a>");
		}
	}
}