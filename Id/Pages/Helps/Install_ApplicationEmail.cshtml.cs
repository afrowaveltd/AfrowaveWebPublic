namespace Id.Pages.Helps
{
	/// <summary>
	/// This class is used to provide the help information for the 'Install_ApplicationEmail' page.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_ApplicationEmailModel(IStringLocalizer<Install_ApplicationEmailModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_ApplicationEmailModel> t = _t;

		/// <summary>
		/// The title of the page.
		/// </summary>
		public string Title { get; set; } = "";

		/// <summary>
		/// The list of lines that will be displayed on the page.
		/// </summary>
		public List<string> Lines = new List<string>();

		/// <summary>
		/// This method is called when the 'Install_ApplicationEmail' page is requested.
		/// </summary>
		public void OnGet()
		{
			Title = t["Application Email"];
			Lines.Add(t["The application email is the contact address for contacting administrators or the support of the application"]);
		}
	}
}