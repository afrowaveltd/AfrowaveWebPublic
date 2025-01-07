namespace Id.Pages.Helps
{
	public class Install_LoginRules_MaxFailedModel(IStringLocalizer<Install_LoginRules_MaxFailedModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_MaxFailedModel> t = _t;
		public string Title => t["Maximal failed login attempts"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The maximum number of failed login attempts before the account is locked."]);
			Lines.Add(t["If you set this value to 0, the account will never be locked."]);
		}
	}
}