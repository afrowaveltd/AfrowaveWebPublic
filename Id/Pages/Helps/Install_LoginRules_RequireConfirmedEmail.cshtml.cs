namespace Id.Pages.Helps
{
	/// <summary>
	/// Require confirmed email
	/// </summary>
	/// <param name="_t"></param>
	public class Install_LoginRules_RequireConfirmedEmailModel(IStringLocalizer<Install_LoginRules_RequireConfirmedEmailModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_LoginRules_RequireConfirmedEmailModel> t = _t;

		/// <summary>
		/// The helper title.
		/// </summary>
		public string Title => t["Require confirmed email"];

		/// <summary>
		/// The helper lines
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// The helper on get method
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["If true, the user must confirm the email address before logging in."]);
			Lines.Add(t["If set to false, the user can log in without a confirmed email"]);
		}
	}
}