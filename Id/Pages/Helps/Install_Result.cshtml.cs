namespace Id.Pages.Helps
{
	public class Install_ResultModel(ISettingsService settings,
		 IStringLocalizer<Install_ResultModel> _t) : PageModel
	{
		private readonly ISettingsService _settings = settings;
		public readonly IStringLocalizer<Install_ResultModel> t = _t;

		public async Task<IActionResult> OnGetAsync()
		{
			_ = await _settings.GetSettingsAsync() ?? new();

			return Page();
		}
	}
}