using System.Globalization;

namespace Id.Pages
{
	public class CookiesModel(ITermsService termsService, ILogger<CookiesModel> logger) : PageModel
	{
		private readonly ITermsService _termsService = termsService;
		private readonly ILogger<CookiesModel> _logger = logger;

		public string CookiesHtml { get; private set; } = string.Empty;

		public async Task OnGetAsync()
		{
			string userLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			CookiesHtml = await _termsService.GetCookiesHTMLAsync(userLanguage);
		}
	}
}