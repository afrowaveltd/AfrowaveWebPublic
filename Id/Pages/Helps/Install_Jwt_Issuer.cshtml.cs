namespace Id.Pages.Helps
{
	public class Install_Jwt_IssuerModel(IStringLocalizer<Install_Jwt_IssuerModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_Jwt_IssuerModel> t = _t;

		public string Title => t["Issuer"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The name of the token issuer."]);
		}
	}
}