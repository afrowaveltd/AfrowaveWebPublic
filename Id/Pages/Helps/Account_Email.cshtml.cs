namespace Id.Pages.Helps
{
	/// <summary>
	/// Represents the model for the email address help page.
	/// </summary>
	/// <param name="_t"></param>
	public class Account_EmailModel(IStringLocalizer<Account_EmailModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_EmailModel> t = _t;

		/// <summary>
		/// Sets the title of the page.
		/// </summary>
		public string Title => t["Email address"];

		/// <summary>
		/// Gets the list of lines.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Handles the GET request.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The email address is used to login into your account."]);
			Lines.Add(t["It is also used to send notifications and to recover your account."]);
			Lines.Add(t["Make sure to use the valid email address. You might receive an activation email"]);
		}
	}
}