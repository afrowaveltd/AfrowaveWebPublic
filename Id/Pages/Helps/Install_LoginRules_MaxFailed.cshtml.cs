namespace Id.Pages.Helps
{
	/// <summary>
	/// Maximal failed login attempts
	/// </summary>
	/// <param name="_t"></param>
	public class Install_LoginRules_MaxFailedModel(IStringLocalizer<Install_LoginRules_MaxFailedModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_MaxFailedModel> t = _t;

		/// <summary>
		/// Helper title
		/// </summary>
		public string Title => t["Maximal failed login attempts"];

		/// <summary>
		/// Helper lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// On get method
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The maximum number of failed login attempts before the account is locked."]);
			Lines.Add(t["If you set this value to 0, the account will never be locked."]);
		}
	}
}