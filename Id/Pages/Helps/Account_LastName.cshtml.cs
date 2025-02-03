namespace Id.Pages.Helps
{
	public class Account_LastNameModel(IStringLocalizer<Account_LastNameModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_LastNameModel> t = _t;
		public string Title => t["Last name"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The last name is used to identify you."]);
			Lines.Add(t["It is used to personalize the application."]);
			Lines.Add(t["Make sure to use the correct last name."]);
			Lines.Add(t["Last name must be at least 2 characters long"]);
			Lines.Add(t["Names are not public"]);
		}
	}
}