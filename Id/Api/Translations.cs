using System.Text.RegularExpressions;

namespace Id.Api
{
	/// <summary>
	/// Provides endpoints for retrieving translation dictionaries for UI components.
	/// </summary>
	[ApiController]
	[Route("api/translations")]
	[Produces("application/json", "application/xml")]
	public class Translations(ITranslationFilesManager translationFilesManager, IStringLocalizer<Translations> t, ILogger<Translations> logger) : ControllerBase
	{
		private readonly ITranslationFilesManager _translationFilesManager = translationFilesManager;
		private readonly IStringLocalizer<Translations> _t = t;
		private readonly ILogger<Translations> _logger = logger;

		/// <summary>
		/// Returns a translation dictionary for the given language (based on Locales folder).
		/// </summary>
		/// <param name="lang">Language code (e.g., "en", "cs").</param>
		/// <returns>Translation dictionary as ApiResponse.</returns>
		[HttpGet("{lang}")]
		public async Task<IActionResult> GetTranslation(string lang)
		{
			if(string.IsNullOrWhiteSpace(lang) || !Regex.IsMatch(lang, "^[a-z]{2}$"))
			{
				return BadRequest(ApiResponse<Dictionary<string, string>>.Fail(_t["Invalid language code."]));
			}

			ApiResponse<Dictionary<string, string>> result = await _translationFilesManager.GetTranslationAsync(null, lang);

			if(!result.Successful || result.Data is null || result.Data.Count == 0)
			{
				return NotFound(ApiResponse<Dictionary<string, string>>.Fail(_t["No translation exists for language"] + " " + lang));
			}

			return Ok(result);
		}

		/// <summary>
		/// Returns a translation dictionary for a specific language and application ID.
		/// </summary>
		/// <param name="lang">Language code (e.g., "en", "cs").</param>
		/// <param name="applicationId">GUID of the application.</param>
		/// <returns>Translation dictionary as ApiResponse.</returns>
		[HttpGet("{lang}/{applicationId}")]
		public async Task<IActionResult> GetTranslationForApp(string lang, string applicationId)
		{
			if(string.IsNullOrWhiteSpace(lang) || !Regex.IsMatch(lang, "^[a-z]{2}$"))
			{
				return BadRequest(ApiResponse<Dictionary<string, string>>.Fail(_t["Invalid language code."]));
			}

			if(string.IsNullOrWhiteSpace(applicationId) || !Guid.TryParse(applicationId, out _))
			{
				return BadRequest(ApiResponse<Dictionary<string, string>>.Fail(_t["Invalid application ID."]));
			}

			ApiResponse<Dictionary<string, string>> result = await _translationFilesManager.GetTranslationAsync(applicationId, lang);

			if(!result.Successful || result.Data is null || result.Data.Count == 0)
			{
				return NotFound(ApiResponse<Dictionary<string, string>>.Fail(_t["No translation exists for language"] + " " + lang + " " + _t["in application"] + " " + applicationId));
			}

			return Ok(result);
		}

		/// <summary>
		/// Returns all translations for a specific application ID or all applications if no ID is provided.
		/// </summary>
		/// <param name="applicationId">ApplicationId</param>
		/// <returns></returns>
		[HttpGet("all")]
		[HttpGet("all/{applicationId}")]
		[Produces("application/json", "application/xml")]
		public async Task<IActionResult> GetAllTranslationsAsync(string? applicationId = null)
		{
			ApiResponse<Dictionary<string, Dictionary<string, string>>> result = await _translationFilesManager.GetAllTranslationsAsync(applicationId);

			if(!result.Successful || result.Data is null || result.Data.Count == 0)
			{
				return Ok(ApiResponse<Dictionary<string, Dictionary<string, string>>>.Fail(result.Message ?? "No translations found."));
			}

			return Ok(result);
		}
	}
}