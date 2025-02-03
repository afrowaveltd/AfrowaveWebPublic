namespace Id.Pages.Helps
{
	public class Account_TermsModel(IStringLocalizer<Account_TermsModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_TermsModel> t = _t;
		public string Title => t["Terms and conditions"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The terms and conditions are the rules that you must follow."]);
			Lines.Add(t["It is used to protect you and the application."]);
			Lines.Add($"{t["You can read the terms and conditions by"]} <a href='/Privacy'> {t["clicking here"]} </a>");
			Lines.Add(t["You must accept the terms and conditions to use the application."]);
		}
	}
}