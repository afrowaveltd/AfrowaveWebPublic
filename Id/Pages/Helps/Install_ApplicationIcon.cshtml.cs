namespace Id.Pages.Helps
{
	/// <summary>
	/// This class is used to provide the help information for the 'Install_ApplicationIcon' page.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_ApplicationIconModel(IStringLocalizer<Install_ApplicationIconModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_ApplicationIconModel> t = _t;

		/// <summary>
		/// The title of the page.
		/// </summary>
		public string Title = string.Empty;

		/// <summary>
		/// The list of lines that will be displayed on the page.
		/// </summary>
		public List<string> Lines = new();

		/// <summary>
		/// This method is called when the 'Install_ApplicationIcon' page is requested.
		/// </summary>
		public void OnGet()
		{
			Title = t["Application Icon"];
			Lines.Add(t["The application icon is a small image that represents the application."]);
			Lines.Add(t["This icon will be displayed in the application list."]);
			Lines.Add(t["The icon is not required, but it is recommended to upload it."]);
			Lines.Add(t["The icon should be square and have a size of 150x150 pixels."]);
			Lines.Add(t["The icon should be in PNG or JPG format."]);
		}
	}
}