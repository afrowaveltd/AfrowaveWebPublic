using SharedTools.Services;

namespace Id.Api
{
	/// <summary>
	/// API controller for retrieving localized text.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class GetLocalized(IStringLocalizer<GetLocalized> localizer,
										ITranslatorService translator,
										ILogger<GetLocalized> logger,
										IHttpContextAccessor httpContextAccessor) : ControllerBase
	{
		private readonly IStringLocalizer<GetLocalized> _localizer = localizer;
		private readonly ITranslatorService _translator = translator;
		private readonly ILogger<GetLocalized> _logger = logger;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		/// <summary>
		/// Translates a given text to the target language.
		/// </summary>
		/// <param name="toTranslate">The text to translate.</param>
		/// <param name="target">The target language code (optional).</param>
		/// <returns>Translated text or an error response.</returns>
		[Route("{toTranslate}/{target?}")]
		[HttpGet]
		public async Task<IActionResult> OnGetAsync(string toTranslate, string? target)
		{
			string translateTo = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
			string translated = toTranslate;

			if(string.IsNullOrEmpty(toTranslate) || string.IsNullOrEmpty(translateTo))
			{
				return BadRequest();
			}

			if(translateTo == "en")
			{
				_logger.LogDebug("No point to translate from English to English");
				return Ok(toTranslate);
			}

			LocalizedString result = _localizer[toTranslate];
			if(result.ResourceNotFound)
			{
				bool failure = true;
				int failureCount = 0;

				bool isSupported = (await _translator.GetSupportedLanguagesAsync()).Contains(translateTo);
				if(!isSupported)
				{
					translated = toTranslate;
				}

				while(failure)
				{
					ApiResponse<string> libreResult = await _translator.TranslateAsync(toTranslate, "en", translateTo);
					if(libreResult.Successful)
					{
						_logger.LogInformation("Translated {toTranslate} to {translateTo} as {result}", toTranslate, translateTo, libreResult.Data);
						translated = libreResult.Data ?? toTranslate;
						failure = false;
					}
					else
					{
						failureCount++;
						_logger.LogWarning("Failed to translate {toTranslate} to {translateTo}", toTranslate, translateTo);
						if(failureCount > 10)
						{
							failure = false;
							translated = toTranslate;
							_logger.LogError("Failed to translate {toTranslate} to {translateTo} after 10 attempts", toTranslate, translateTo);
						}
						await Task.Delay(1000);
					}
				}
			}
			else
			{
				translated = result.Value;
			}
			return Ok(translated);
		}
	}
}