namespace Id.Pages.Helps
{
	/// <summary>
	/// Represents the model for the cookies help page.
	/// </summary>
	/// <param name="_t"></param>
	public class Account_CookiesModel(IStringLocalizer<Account_CookiesModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_CookiesModel> t = _t;

		/// <summary>
		/// Sets the title of the page.
		/// </summary>
		public string Title => t["Cookies"];

		/// <summary>
		/// Gets the list of lines.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Handles the GET request.
		/// </summary>
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