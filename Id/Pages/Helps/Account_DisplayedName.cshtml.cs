namespace Id.Pages.Helps
{
	/// <summary>
	/// Represents the model for the displayed name help page.
	/// </summary>
	/// <param name="_t"></param>
	public class Account_DisplayedNameModel(IStringLocalizer<Account_DisplayedNameModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_DisplayedNameModel> t = _t;

		/// <summary>
		/// Sets the title of the page.
		/// </summary>
		public string Title => t["Displayed name"];

		/// <summary>
		/// Gets the list of lines.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Handles the GET request.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The displayed name is how people will see you."]);
			Lines.Add(t["It is used to personalize the application."]);
			Lines.Add(t["If you leave this field empty, your first name will be used."]);
			Lines.Add(t["Displayed must be non offensive, or confusing one."]);
		}
	}
}