namespace Id.Pages.Helps
{
	public class Install_Jwt_AudienceModel(IStringLocalizer<Install_Jwt_AudienceModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_Jwt_AudienceModel> t = _t;

		public string Title => t["Audience"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The domain names valid for the token."]);
		}
	}
}