namespace Id.Api
{
	/// <summary>
	/// Controller for managing languages.
	/// </summary>
	/// <param name="languagesManager">The language manager service</param>
	[ApiController]
	[Route("api/[controller]")]
	public class LanguagesController(ILanguagesManager languagesManager) : ControllerBase
	{
		private readonly ILanguagesManager _languagesManager = languagesManager;

		/// <summary>
		/// Gets all languages.
		/// </summary>
		/// <returns>The ApiResponse with the List of LanguageViews</returns>
		// GET: /api/languages
		[HttpGet]
		public async Task<ActionResult<ApiResponse<List<LanguageView>>>> GetAllLanguagesAsync()
		{
			ApiResponse<List<LanguageView>> result = await _languagesManager.GetAllLanguagesAsync();
			return StatusCode(result.Successful ? 200 : 404, result);
		}

		/// <summary>
		/// Gets all languages that can be translated.
		/// </summary>
		/// <returns>The ApiResponse with the List of LanguageViews</returns>
		// GET: /api/languages/translatable
		[HttpGet("translatable")]
		public async Task<ActionResult<ApiResponse<List<LanguageView>>>> GetAllTranslatableAsync()
		{
			ApiResponse<List<LanguageView>> result = await _languagesManager.GetAllTranslatableLanguagesAsync();
			return StatusCode(result.Successful ? 200 : 404, result);
		}

		/// <summary>
		/// Gets a language by its code.
		/// </summary>
		/// <param name="code">LanguageView or 404 if the code is unknown</param>
		/// <returns></returns>
		// GET: /api/languages/{code}
		[HttpGet("{code}")]
		public async Task<ActionResult<ApiResponse<LanguageView>>> GetByCodeAsync(string code)
		{
			ApiResponse<LanguageView> result = await _languagesManager.GetLanguageByCodeAsync(code);
			return StatusCode(result.Successful ? 200 : 404, result);
		}
	}
}