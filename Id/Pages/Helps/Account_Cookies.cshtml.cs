namespace Id.Pages.Helps
{
	public class Account_CookiesModel(IStringLocalizer<Account_CookiesModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_CookiesModel> t = _t;
		public string Title => t["Cookies"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["Cookies are small text files that are stored on your device."]);
			Lines.Add(t["They are used to store information about your preferences."]);
			Lines.Add(t["Cookies are used to personalize the application."]);
			Lines.Add(t["You can delete cookies from your browser."]);
			Lines.Add(t["You can read more about our cookies by"] + " <a href='/Cookies' target='_blank'> " + t["clicking here"] + " </a>");
			Lines.Add(t["You must agree to the use of cookies to use the application."]);
		}
	}
}