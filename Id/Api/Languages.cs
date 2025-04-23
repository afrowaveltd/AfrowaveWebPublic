namespace Id.Api
{
	[ApiController]
	[Route("api/[controller]")]
	public class LanguagesController(ILanguagesManager languagesManager) : ControllerBase
	{
		private readonly ILanguagesManager _languagesManager = languagesManager;

		// GET: /api/languages
		[HttpGet]
		public async Task<ActionResult<ApiResponse<List<LanguageView>>>> GetAllLanguages()
		{
			ApiResponse<List<LanguageView>> result = await _languagesManager.GetAllLanguagesAsync();
			return StatusCode(result.Successful ? 200 : 404, result);
		}

		// GET: /api/languages/translatable
		[HttpGet("translatable")]
		public async Task<ActionResult<ApiResponse<List<LanguageView>>>> GetAllTranslatable()
		{
			ApiResponse<List<LanguageView>> result = await _languagesManager.GetAllTranslatableLanguagesAsync();
			return StatusCode(result.Successful ? 200 : 404, result);
		}

		// GET: /api/languages/{code}
		[HttpGet("{code}")]
		public async Task<ActionResult<ApiResponse<LanguageView>>> GetByCode(string code)
		{
			ApiResponse<LanguageView> result = await _languagesManager.GetLanguageByCodeAsync(code);
			return StatusCode(result.Successful ? 200 : 404, result);
		}
	}
}