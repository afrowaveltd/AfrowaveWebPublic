namespace Id.Pages.Account
{
	public class ResetPasswordModel(IEmailManager emailService,
		IUsersManager userService,
		IStringLocalizer<ResetPasswordModel> t) : PageModel
	{
		private readonly IEmailManager _emailService = emailService;
		private readonly IUsersManager _userService = userService;
		private readonly IStringLocalizer<ResetPasswordModel> _t = t;

		[FromRoute]
		public string email { get; set; }

		public async Task<ActionResult> OnGetAsync()
		{
			return Page();
		}

		public async Task<ActionResult> OnPostAsync(string email)
		{
			return Page();
		}
	}
}