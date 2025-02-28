namespace Id.Pages.Helps
{
	/// <summary>
	/// Jwt settings are used to store user session information.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_Jwt_IssuerModel(IStringLocalizer<Install_Jwt_IssuerModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_Jwt_IssuerModel> t = _t;

		/// <summary>
		/// The title of the page.
		/// </summary>
		public string Title => t["Issuer"];

		/// <summary>
		/// The description of the page.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// The OnGet method.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The name of the token issuer."]);
		}
	}
}