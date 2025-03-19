namespace Id.Pages.Helps
{
	/// <summary>
	/// Represents a model for sharing user account details within a community application. It provides localized strings
	/// for user prompts.
	/// </summary>
	/// <param name="_t">Used for localization, allowing the model to retrieve translated strings for display.</param>
	public class Account_DataShareModel(IStringLocalizer<Account_DataShareModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_DataShareModel> t = _t;

		/// <summary>
		/// Returns the title for agreeing to share user details from a dictionary. The title is retrieved from the key 'Agree
		/// to share user details'.
		/// </summary>
		public string Title => t["Agree to share user details"];

		/// <summary>
		/// A list that stores strings, typically used to hold multiple lines of text. It is initialized as an empty list.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Handles the GET request by adding specific messages to the Lines collection.
		/// These messages inform users about
		/// data sharing policies.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The application needs to share your details with other applications inside the community."]);
			Lines.Add(t["We don't share your details with third-party applications."]);
		}
	}
}