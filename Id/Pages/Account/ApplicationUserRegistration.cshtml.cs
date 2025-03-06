namespace Id.Pages.Account
{
	public class ApplicationUserRegistrationModel(IStringLocalizer<ApplicationUserRegistrationModel> t,
		ILogger<ApplicationUserRegistrationModel> logger) : PageModel
	{
		private readonly IStringLocalizer<ApplicationUserRegistrationModel> _t = t;
		private readonly ILogger<ApplicationUserRegistrationModel> _logger = logger;

		[BindProperty(SupportsGet = true)]
		public string ApplicationId { get; set; }

		[BindProperty(SupportsGet = true)]
		public string UserId { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			_ = RouteData.Values["ApplicationId"]?.ToString();

			_ = RouteData.Values["UserId"]?.ToString();
			if(string.IsNullOrEmpty(ApplicationId) || string.IsNullOrEmpty(UserId))
			{
				_logger.LogError("ApplicationId or UserId is invalid");
				return RedirectToPage("/Error/404");
			}
			return Page();
		}
	}
}