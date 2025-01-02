namespace Id.Pages.Helps
{
	public class Install_ApplicationRolesModel(IStringLocalizer<Install_ApplicationRolesModel> _t) : PageModel
	{
		public readonly IStringLocalizer<Install_ApplicationRolesModel> t = _t;

		public string Title { get; set; } = "";
		public List<string> Lines = new();

		public void OnGet()
		{
			Title = t["Terms and Conditions"];

			Lines.Add(t["Now we created all necessary roles for the application and we must finish basic application creation by a bit of legal stuff"]);
			Lines.Add(t["Please read the terms and conditions and privacy policy and confirm that you agree with them."]);
			Lines.Add(t["You can find them by clicking on the links below."]);

		}
	}
}