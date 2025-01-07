namespace Id.Pages.Helps
{
	public class Install_LoginRules_RequireConfirmedEmailModel(IStringLocalizer<Install_LoginRules_RequireConfirmedEmailModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_RequireConfirmedEmailModel> t = _t;

		public string Title => t["Require confirmed email"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["If true, the user must confirm the email address before logging in."]);
			Lines.Add(t["If set to false, the user can log in without a confirmed email"]);
		}
	}
}