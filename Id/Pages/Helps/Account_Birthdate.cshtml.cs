namespace Id.Pages.Helps
{
	/// <summary>
	/// Represents the model for the birthdate help page.
	/// </summary>
	/// <param name="_t">Localizer</param>
	public class Account_BirthdateModel(IStringLocalizer<Account_BirthdateModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_BirthdateModel> t = _t;

		/// <summary>
		/// Sets the title of the page.
		/// </summary>
		public string Title => t["Birthdate"];

		/// <summary>
		/// Gets the list of lines.
		/// </summary>
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The birthdate is used to verify your age."]);
			Lines.Add(t["It is used to personalize the application."]);
			Lines.Add(t["Make sure to use the correct birthdate."]);
			Lines.Add(t["You must be at least 8 years old"]);
		}
	}
}