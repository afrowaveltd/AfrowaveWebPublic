namespace Id.Pages.Account
{
	public class Account_FirstNameModel(IStringLocalizer<Account_FirstNameModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_FirstNameModel> t = _t;
		public string Title => t["First name"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The first name is used to identify you."]);
			Lines.Add(t["It is used to personalize the application."]);
			Lines.Add(t["Make sure to use the correct first name."]);
			Lines.Add(t["First name must be at least 2 characters long"]);
			Lines.Add(t["Names are not public, but if you leave the displayed name empty, your first name will be used"]);
		}
	}
}