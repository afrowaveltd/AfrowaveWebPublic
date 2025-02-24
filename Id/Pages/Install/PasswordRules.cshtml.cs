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
		// Injected services
		private readonly ILogger<PasswordRulesModel> _logger = logger;

		private readonly ISettingsService _settings = settings;
		private readonly IInstallationStatusService _status = status;
		private readonly ISelectOptionsServices _selectOptions = selectOptions;
		public readonly IStringLocalizer<PasswordRulesModel> t = _t;

		// Set the select list items
		public List<SelectListItem> RequireNonAlphanumeric { get; set; } = [];

		public List<SelectListItem> RequireLowercase { get; set; } = [];
		public List<SelectListItem> RequireUppercase { get; set; } = [];
		public List<SelectListItem> RequireDigit { get; set; } = [];

		[BindProperty]
		public InputModel? Input { get; set; }

		/// <summary>
		/// The input model for the password rules
		/// </summary>
		/// <permission cref="MinimumLength">The minimum length of the password</permission>
		/// <permission cref="MaximumLength">The maximum length of the password</permission>
		/// <permission cref="RequireNonAlphanumeric">If the password requires a non-alphanumeric character</permission>
		/// <permission cref="RequireLowercase">If the password requires a lowercase character</permission>
		/// <permission cref="RequireUppercase">If the password requires an uppercase character</permission>
		/// <permission cref="RequireDigit">If the password requires a digit</permission>
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

		/// <summary>
		/// Handles the GET request
		/// </summary>
		/// <returns>The password rules page</returns>
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

		/// <summary>
		/// Handles the POST request
		/// </summary>
		/// <returns>Redirect to the Cookie rules page in the case of success</returns>

		public async Task<IActionResult> OnPostAsync()
		{
			if(!ModelState.IsValid)
			{
				return Page();
			}
			Models.SettingsModels.IdentificatorSettings settings = await _settings.GetSettingsAsync();
			settings.PasswordRules = new()
			{
				MinimumLength = Input.MinimumLength,
				MaximumLength = Input.MaximumLength,
				RequireDigit = Input.RequireDigit,
				RequireLowercase = Input.RequireLowercase,
				RequireNonAlphanumeric = Input.RequireNonAlphanumeric,
				RequireUppercase = Input.RequireUppercase,
				IsConfigured = true
			};
			await _settings.SetSettingsAsync(settings);
			return RedirectToPage("/Install/CookieSettings");
		}
	}
}