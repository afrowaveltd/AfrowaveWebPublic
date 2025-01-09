using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Id.Pages.Install
{
	public class PasswordRulesModel(ILogger<PasswordRulesModel> logger,
		  ISettingsService settings,
		  IStringLocalizer<PasswordRulesModel> _t,
		  IInstallationStatusService status,
		  ISelectOptionsServices selectOptions) : PageModel
	{
		private readonly ILogger<PasswordRulesModel> _logger = logger;
		private readonly ISettingsService _settings = settings;
		private readonly IInstallationStatusService _status = status;
		private readonly ISelectOptionsServices _selectOptions = selectOptions;
		public readonly IStringLocalizer<PasswordRulesModel> t = _t;
		public List<SelectListItem> RequireNonAlphanumeric { get; set; } = [];
		public List<SelectListItem> RequireLowercase { get; set; } = [];
		public List<SelectListItem> RequireUppercase { get; set; } = [];
		public List<SelectListItem> RequireDigit { get; set; } = [];

		[BindProperty]
		public InputModel? Input { get; set; }

		public class InputModel
		{
			[Range(1, 100)]
			public int MinimumLength { get; set; } = 6;

			[Range(1, 100)]
			public int MaximumLength { get; set; } = 100;

			public bool RequireNonAlphanumeric { get; set; } = false;
			public bool RequireLowercase { get; set; } = true;
			public bool RequireUppercase { get; set; } = true;
			public bool RequireDigit { get; set; } = true;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			RequireNonAlphanumeric = await _selectOptions.GetBinaryOptionsAsync(false);
			RequireLowercase = await _selectOptions.GetBinaryOptionsAsync(true);
			RequireUppercase = await _selectOptions.GetBinaryOptionsAsync(true);
			RequireDigit = await _selectOptions.GetBinaryOptionsAsync(true);
			if(!await _status.ProperInstallState(InstalationSteps.PasswordRules))
			{
				return RedirectToPage("/Install/LoginRules");
			}

			_ = await _settings.GetSettingsAsync();
			Input = new InputModel();

			return Page();
		}
	}
}