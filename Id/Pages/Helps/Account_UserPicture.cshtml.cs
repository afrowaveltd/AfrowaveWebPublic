namespace Id.Pages.Helps
{
	public class Account_UserPictureModel(IStringLocalizer<Account_UserPictureModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Account_UserPictureModel> t = _t;
		public string Title => t["Profile picture"];
		public List<string> Lines = [];

		public void OnGet()
		{
			Lines.Add(t["The profile picture is public."]);
			Lines.Add(t["It is used to personalize the application."]);
			Lines.Add(t["Make sure to use a picture that represents you."]);
			Lines.Add(t["Don't use offensive or obscene pictures"]);
		}
	}
}