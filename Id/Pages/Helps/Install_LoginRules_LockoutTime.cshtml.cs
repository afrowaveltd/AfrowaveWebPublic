namespace Id.Pages.Helps
{
	public class Install_LoginRules_LockoutTimeModel(IStringLocalizer<Install_LoginRules_LockoutTimeModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_LockoutTimeModel> t = _t;
		public string Title => t["Lockout time"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The time in minutes the account is locked after the maximum number of failed login attempts"]);
			Lines.Add(t["If you set this value to 0, the account will be locked until administrator unlocks it manually."]);
		}
	}
}