using SharedTools.Services;

namespace Id.Services
{
	/// <summary>
	/// Service to handle terms.
	/// </summary>
	/// <param name="environment">Web Host Environment</param>
	/// <param name="textToHtmlService">Text to HTML service</param>
	/// <param name="logger">Logger</param>
	public class TermsService(IWebHostEnvironment environment, ITextToHtmlService textToHtmlService, ILogger<TermsService> logger) : ITermsService
	{
		private readonly IWebHostEnvironment _environment = environment;
		private readonly ITextToHtmlService _textToHtmlService = textToHtmlService;
		private readonly ILogger<TermsService> _logger = logger;

		/// <summary>
		/// Get the privacy policy HTML.
		/// </summary>
		/// <param name="language"></param>
		/// <returns>Privacy policy HTML</returns>
		public async Task<string> GetTermsHtmlAsync(string language)
		{
			string termsPath = Path.Combine(_environment.WebRootPath, "docs", "terms", $"{language}.txt");

			if(!File.Exists(termsPath))
			{
				_logger.LogWarning("Terms file not found for language {language}", language);
				termsPath = Path.Combine(_environment.WebRootPath, "docs", "terms", "en.txt");
			}

			if(!File.Exists(termsPath))
			{
				_logger.LogError("Terms file not found for any language");
				return string.Empty;
			}

			_logger.LogInformation("Reading terms from {termsPath}", termsPath);
			string termsText = await File.ReadAllTextAsync(termsPath);

			return _textToHtmlService.ConvertTextToHtml(termsText);
		}

		/// <summary>
		/// Get the cookies HTML.
		/// </summary>
		/// <param name="language">Language code</param>
		/// <returns>Cookies HTML page</returns>
		public async Task<string> GetCookiesHTMLAsync(string language)
		{
			string cookiesPath = Path.Combine(_environment.WebRootPath, "docs", "cookies", $"{language}.txt");
			if(!File.Exists(cookiesPath))
			{
				_logger.LogWarning("Cookies file not found for language {language}", language);
				cookiesPath = Path.Combine(_environment.WebRootPath, "docs", "cookies", "en.txt");
			}
			if(!File.Exists(cookiesPath))
			{
				_logger.LogError("Cookies file not found for any language");
				return string.Empty;
			}
			_logger.LogInformation("Reading cookies from {cookiesPath}", cookiesPath);
			string cookiesText = await File.ReadAllTextAsync(cookiesPath);
			return _textToHtmlService.ConvertTextToHtml(cookiesText);
		}
	}
}