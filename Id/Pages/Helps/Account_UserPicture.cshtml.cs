namespace Id.Pages.Helps
{
	/// <summary>
	/// This page is used to help the user to understand the profile picture.
	/// </summary>
	/// <param name="_t"></param>
	public class Account_UserPictureModel(IStringLocalizer<Account_UserPictureModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_UserPictureModel> t = _t;

		/// <summary>
		/// The title of the page.
		/// </summary>
		public string Title => t["Profile picture"];

		/// <summary>
		/// The lines of the page.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// OnGet method of the page.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The profile picture is public."]);
			Lines.Add(t["It is used to personalize the application."]);
			Lines.Add(t["Make sure to use a picture that represents you."]);
			Lines.Add(t["Don't use offensive or obscene pictures"]);
		}
	}
}