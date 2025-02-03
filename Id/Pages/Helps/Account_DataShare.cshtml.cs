namespace Id.Pages.Helps
{
	public class Account_DataShareModel(IStringLocalizer<Account_DataShareModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_DataShareModel> t = _t;
		public string Title => t["Agree to share user details"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The application needs to share your details with other applications inside the community."]);
			Lines.Add(t["We don't share your details with third-party applications."]);
		}
	}
}