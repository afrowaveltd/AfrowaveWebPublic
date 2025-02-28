using System.Globalization;

namespace Id.Pages
{
	/// <summary>
	/// The terms page model
	/// </summary>
	/// <param name="termsService">Terms service</param>
	/// <param name="logger"></param>
	public class TermsModel(ITermsService termsService, ILogger<TermsModel> logger) : PageModel
	{
		private readonly ITermsService _termsService = termsService;
		private readonly ILogger<TermsModel> _logger = logger;

		/// <summary>
		/// The terms HTML
		/// </summary>
		public string TermsHtml { get; private set; } = string.Empty;

		/// <summary>
		/// The terms page model
		/// </summary>
		/// <returns></returns>
		public async Task OnGetAsync()
		{
			string userLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

			TermsHtml = await _termsService.GetTermsHtmlAsync(userLanguage);
		}
	}
}