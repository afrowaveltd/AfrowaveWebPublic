namespace Id.Pages.Helps
{
	/// <summary>
	/// This class is used to provide the help information for the 'Install_ApplicationDescription' page.
	/// </summary>
	/// <param name="_t">The localization</param>
	public class Install_ApplicationDescriptionModel(IStringLocalizer<Install_ApplicationDescriptionModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_ApplicationDescriptionModel> t = _t;

		/// <summary>
		/// The title of the page.
		/// </summary>
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// The list of lines that will be displayed on the page.
		/// </summary>
		public List<string> Lines { get; set; } = new();

		/// <summary>
		/// This method is called when the 'Install_ApplicationDescription' page is requested.
		/// </summary>
		public void OnGet()
		{
			Title = t["Application Description"];
			Lines.Add(t["The application description is a short text that describes the purpose of application."]);
			Lines.Add(t["This description will be displayed in the application list."]);
			Lines.Add(t["The description is not required, but it is recommended to fill it in."]);
		}
	}
}