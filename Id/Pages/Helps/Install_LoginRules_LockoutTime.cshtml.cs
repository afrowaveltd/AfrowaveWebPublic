namespace Id.Pages.Helps
{
	/// <summary>
	/// Lockout time
	/// </summary>
	/// <param name="_t"></param>
	public class Install_LoginRules_LockoutTimeModel(IStringLocalizer<Install_LoginRules_LockoutTimeModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_LockoutTimeModel> t = _t;

		/// <summary>
		/// Helper title
		/// </summary>
		public string Title => t["Lockout time"];

		/// <summary>
		/// Helper lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// On get method
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The time in minutes the account is locked after the maximum number of failed login attempts"]);
			Lines.Add(t["If you set this value to 0, the account will be locked until administrator unlocks it manually."]);
		}
	}
}