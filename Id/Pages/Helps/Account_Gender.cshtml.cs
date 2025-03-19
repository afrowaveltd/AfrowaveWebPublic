namespace Id.Pages.Helps
{
	/// <summary>
	/// Gets the help for Gender
	/// </summary>
	/// <param name="t">Localizer</param>
	public class Account_GenderModel(IStringLocalizer<Account_GenderModel> t) : PageModel
	{
		private readonly IStringLocalizer _t = t;

		/// <summary>
		/// Retrieves the value associated with the key 'Gender' from the dictionary _t.
		/// </summary>
		public string Title => _t["Gender"];

		/// <summary>
		/// A list that stores strings, typically used to hold multiple lines of text. It is initialized as an empty list.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// Get method for the page
		/// </summary>
		public void OnGet()
		{
			Lines.Add(_t["Please select your gender"]);
			Lines.Add(_t["If you don't want to publish your gender, select 'Other'"]);
		}
	}
}