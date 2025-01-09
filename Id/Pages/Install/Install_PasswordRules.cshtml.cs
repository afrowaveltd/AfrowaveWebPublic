namespace Id.Pages.Install
{
	public class Install_PasswordRulesModel(IStringLocalizer<Install_PasswordRulesModel> _t) : PageModel
	{
		public readonly IStringLocalizer<Install_PasswordRulesModel> t = _t;

		public string Title { get; set; } = "Password settings";
		public List<string> Lines { get; set; } = [];

		public void OnGet()
		{
		}
	}
}