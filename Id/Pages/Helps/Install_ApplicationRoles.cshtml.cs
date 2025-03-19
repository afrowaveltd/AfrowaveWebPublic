namespace Id.Pages.Helps
{
	/// <summary>
	/// The application roles are the roles that are used in the application
	/// </summary>
	/// <param name="_t"></param>
	public class Install_ApplicationRolesModel(IStringLocalizer<Install_ApplicationRolesModel> _t) : PageModel
	{
		/// <summary>
		/// Holds a reference to an IStringLocalizer for localizing strings in the Install_ApplicationRolesModel. It is marked
		/// as readonly.
		/// </summary>
		public readonly IStringLocalizer<Install_ApplicationRolesModel> t = _t;

		/// <summary>
		/// The application roles are the roles that are used in the application
		/// </summary>
		public string Title { get; set; } = "";

		/// <summary>
		/// The application roles are the roles that are used in the application
		/// </summary>
		public List<string> Lines = new();

		/// <summary>
		/// The application roles are the roles that are used in the application
		/// </summary>
		public void OnGet()
		{
			Title = t["Terms and Conditions"];

			Lines.Add(t["Now we created all necessary roles for the application and we must finish basic application creation by a bit of legal stuff"]);
			Lines.Add(t["Please read the terms and conditions and privacy policy and confirm that you agree with them."]);
			Lines.Add(t["You can find them by clicking on the links below."]);
		}
	}
}