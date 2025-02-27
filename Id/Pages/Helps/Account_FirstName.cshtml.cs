namespace Id.Pages.Account
{
	/// <summary>
	/// Represents the model for the first name help page.
	/// </summary>
	/// <param name="_t"></param>
	public class Account_FirstNameModel(IStringLocalizer<Account_FirstNameModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_FirstNameModel> t = _t;

		/// <summary>
		/// Sets the title of the page.
		/// </summary>
		public string Title => t["First name"];

		/// <summary>
		/// Gets the list of lines.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Handles the GET request.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The first name is used to identify you."]);
			Lines.Add(t["It is used to personalize the application."]);
			Lines.Add(t["Make sure to use the correct first name."]);
			Lines.Add(t["First name must be at least 2 characters long"]);
			Lines.Add(t["Names are not public, but if you leave the displayed name empty, your first name will be used"]);
		}
	}
}