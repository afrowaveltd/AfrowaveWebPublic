namespace Id.Services
{
	/// <summary>
	/// The LanguagesManager class is responsible for managing languages in the application.
	/// </summary>
	/// <param name="context">EF DbContext</param>
	/// <param name="t">Localization service</param>
	/// <param name="logger">Logger service</param>
	/// <param name="translator">translator service</param>
	public class LanguagesManager(ApplicationDbContext context,
		IStringLocalizer<LanguagesManager> t,
		ILogger<LanguagesManager> logger,
		ITranslatorService translator) : ILanguagesManager
	{
		private readonly ApplicationDbContext _context = context;
		private readonly IStringLocalizer<LanguagesManager> _t = t;
		private readonly ILogger<LanguagesManager> _logger = logger;
		private readonly ITranslatorService _translator = translator;

		/// <summary>
		/// Gets a language by its code.
		/// </summary>
		/// <param name="code">Two digits language code</param>
		/// <returns>ApiResponse with LanguageView Data</returns>
		public async Task<ApiResponse<LanguageView>> GetLanguageByCodeAsync(string code)
		{
			ApiResponse<LanguageView> response = new();

			if(string.IsNullOrEmpty(code))
			{
				response.Successful = false;
				response.Message = _t["Language code is required"];
				return response;
			}

			if(code.Length != 2)
			{
				response.Successful = false;
				response.Message = _t["Language code must be two characters"];
				return response;
			}

			Language? language = await _context.Languages
				.AsNoTracking()
				.FirstOrDefaultAsync(l => l.Code == code);

			if(language == null)
			{
				response.Successful = false;
				response.Message = _t["Language not found"];
				return response;
			}

			LanguageView languageView = new()
			{
				Id = language.Id,
				Code = language.Code,
				Name = language.Name,
				Native = language.Native,
				Rtl = language.Rtl,
			};
			string[] translatable = await _translator.GetSupportedLanguagesAsync() ?? [];
			languageView.CanAutotranslate = translatable.Contains(language.Code);
			response.Data = languageView;
			return response;
		}

		/// <summary>
		/// Gets all languages.
		/// </summary>
		/// <returns>ApiResponse with Data as a List of objects LanguageView</returns>
		public async Task<ApiResponse<List<LanguageView>>> GetAllLanguagesAsync()
		{
			string[] translatable = await _translator.GetSupportedLanguagesAsync() ?? [];
			ApiResponse<List<LanguageView>> response = new();
			List<Language> languages = await _context.Languages
				.AsNoTracking()
				.ToListAsync();
			if(languages == null || languages.Count == 0)
			{
				response.Successful = false;
				response.Message = _t["No languages found"];
				return response;
			}
			List<LanguageView> languageViews = [.. languages.Select(l => new LanguageView
			{
				Id = l.Id,
				Code = l.Code,
				Name = l.Name,
				Native = l.Native,
				Rtl = l.Rtl,
			})];
			foreach(LanguageView languageView in languageViews)
			{
				languageView.CanAutotranslate = translatable.Contains(languageView.Code);
			}
			response.Data = languageViews;
			return response;
		}

		/// <summary>
		/// Gets all languages supported by LibreTranslate.
		/// </summary>
		/// <returns>ApiResponse with Data including list of LanguageView</returns>
		public async Task<ApiResponse<List<LanguageView>>> GetAllTranslatableLanguagesAsync()
		{
			string[] translatable = await _translator.GetSupportedLanguagesAsync() ?? [];

			List<Language> languages = await _context.Languages
				 .AsNoTracking()
				 .Where(l => translatable.Contains(l.Code))
				 .ToListAsync();

			ApiResponse<List<LanguageView>> response = new ApiResponse<List<LanguageView>>();

			if(languages == null || languages.Count == 0)
			{
				response.Successful = false;
				response.Message = _t["No translatable languages found"];
				return response;
			}

			response.Data = [.. languages.Select(l => new LanguageView
			{
				Id = l.Id,
				Code = l.Code,
				Name = l.Name,
				Native = l.Native,
				Rtl = l.Rtl,
				CanAutotranslate = true
			})];

			return response;
		}
	}
}