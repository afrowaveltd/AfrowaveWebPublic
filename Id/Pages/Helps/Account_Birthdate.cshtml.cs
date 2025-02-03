namespace Id.Pages.Helps
{
	public class Account_BirthdateModel(IStringLocalizer<Account_BirthdateModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_BirthdateModel> t = _t;
		public string Title => t["Birthdate"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The birthdate is used to verify your age."]);
			Lines.Add(t["It is used to personalize the application."]);
			Lines.Add(t["Make sure to use the correct birthdate."]);
			Lines.Add(t["You must be at least 8 years old"]);
		}
	}
}