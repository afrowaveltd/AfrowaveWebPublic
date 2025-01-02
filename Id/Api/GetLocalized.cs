using SharedTools.Services;

namespace Id.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetLocalized : ControllerBase
    {
        private readonly IStringLocalizer<GetLocalized> _localizer;
        private readonly ITranslatorService _translator;
        private readonly ILogger<GetLocalized> _logger;
        private IHttpContextAccessor _httpContextAccessor;

        public GetLocalized(IStringLocalizer<GetLocalized> localizer,
                                ITranslatorService translator,
                                ILogger<GetLocalized> logger,
                                IHttpContextAccessor httpContextAccessor)
        {
            _localizer = localizer;
            _translator = translator;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

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
                _logger.LogDebug("No point to translate from english to english");
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

                _ = new ApiResponse<string>();
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