namespace Id.Pages.Helps
{
	public class Account_DisplayedNameModel(IStringLocalizer<Account_DisplayedNameModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_DisplayedNameModel> t = _t;

		public string Title => t["Displayed name"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The displayed name is how people will see you."]);
			Lines.Add(t["It is used to personalize the application."]);
			Lines.Add(t["If you leave this field empty, your first name will be used."]);
			Lines.Add(t["Displayed must be non offensive, or confusing one."]);
		}
	}
}