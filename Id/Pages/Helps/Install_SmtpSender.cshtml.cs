namespace Id.Pages.Helps
{
	/// <summary>
	/// SMTP Sender
	/// </summary>
	/// <param name="_t"></param>
	public class Install_SmtpSenderModel(IStringLocalizer<Install_SmtpSenderModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_SmtpSenderModel> t = _t;

		/// <summary>
		/// Help title
		/// </summary>
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// Help lines
		/// </summary>
		public List<string> Lines { get; set; } = [];

		/// <summary>
		/// OnGet method
		/// </summary>
		/// <returns></returns>
		public void OnGet()
		{
			Title = t["SMTP Sender"];
			Lines.Add(t["The SMTP sender is the name that will be displayed in sent emails."]);
		}
	}
}