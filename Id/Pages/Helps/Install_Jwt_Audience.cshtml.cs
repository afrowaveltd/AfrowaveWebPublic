namespace Id.Pages.Helps
{
	/// <summary>
	/// Jwt settings are used to store user session information.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_Jwt_AudienceModel(IStringLocalizer<Install_Jwt_AudienceModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_Jwt_AudienceModel> t = _t;

		/// <summary>
		/// The title of the page.
		/// </summary>
		public string Title => t["Audience"];

		/// <summary>
		/// The description of the page.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// The OnGet method.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The domain names valid for the token."]);
		}
	}
}