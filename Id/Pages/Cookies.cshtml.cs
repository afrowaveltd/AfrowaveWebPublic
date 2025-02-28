using System.Globalization;

namespace Id.Pages
{
	/// <summary>
	/// The cookies page model
	/// </summary>
	/// <param name="termsService"></param>
	/// <param name="logger"></param>
	public class CookiesModel(ITermsService termsService, ILogger<CookiesModel> logger) : PageModel
	{
		private readonly ITermsService _termsService = termsService;
		private readonly ILogger<CookiesModel> _logger = logger;

		/// <summary>
		/// The cookies HTML
		/// </summary>
		public string CookiesHtml { get; private set; } = string.Empty;

		/// <summary>
		/// The cookies page model
		/// </summary>
		/// <returns></returns>
		public async Task OnGetAsync()
		{
			string userLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			CookiesHtml = await _termsService.GetCookiesHTMLAsync(userLanguage);
		}
	}
}