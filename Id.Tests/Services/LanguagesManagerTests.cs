/// <summary>
/// LanguagesManagerTests class.
/// </summary>
public class LanguagesManagerTests
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IStringLocalizer<LanguagesManager> _localizer;
	private readonly ILogger<LanguagesManager> _logger;
	private readonly ITranslatorService _translator;
	private readonly ILanguagesManager _languagesManager;

	/// <summary>
	/// Initializes a new instance of the <see cref="LanguagesManagerTests"/> class.
	/// </summary>
	public LanguagesManagerTests()
	{
		DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
			 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			 .Options;

		_dbContext = new ApplicationDbContext(options);
		_localizer = Substitute.For<IStringLocalizer<LanguagesManager>>();
		_logger = Substitute.For<ILogger<LanguagesManager>>();
		_translator = Substitute.For<ITranslatorService>();

		_languagesManager = new LanguagesManager(_dbContext, _localizer, _logger, _translator);
	}

	/// <summary>
	/// Test for GetAllLanguagesAsync method.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task GetAllLanguagesAsync_ReturnsLanguages()
	{
		_ = _dbContext.Languages.Add(new Language { Id = 1, Code = "en", Name = "English", Native = "English", Rtl = 0 });
		_ = _dbContext.Languages.Add(new Language { Id = 2, Code = "cs", Name = "Czech", Native = "Čeština", Rtl = 0 });
		_ = await _dbContext.SaveChangesAsync();

		_ = _translator.GetSupportedLanguagesAsync().Returns(new[] { "en", "cs" });

		ApiResponse<List<Id.Models.DataViews.LanguageView>> result = await _languagesManager.GetAllLanguagesAsync();

		Assert.True(result.Successful);
		Assert.Equal(2, result.Data.Count);
		Assert.All(result.Data, lang => Assert.True(lang.CanAutotranslate));
	}

	/// <summary>
	/// Test for GetAllLanguagesAsync method when no languages are found.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task GetAllLanguagesAsync_ReturnsEmpty()
	{
		ApiResponse<List<Id.Models.DataViews.LanguageView>> result = await _languagesManager.GetAllLanguagesAsync();

		Assert.False(result.Successful);
		Assert.Null(result.Data);
	}

	/// <summary>
	/// Test for GetLanguageByCodeAsync method.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task GetLanguageByCodeAsync_Found()
	{
		_ = _dbContext.Languages.Add(new Language { Id = 1, Code = "en", Name = "English", Native = "English", Rtl = 0 });
		_ = await _dbContext.SaveChangesAsync();

		_ = _translator.GetSupportedLanguagesAsync().Returns(new[] { "en" });

		ApiResponse<Id.Models.DataViews.LanguageView> result = await _languagesManager.GetLanguageByCodeAsync("en");

		Assert.True(result.Successful);
		Assert.Equal("en", result.Data.Code);
		Assert.True(result.Data.CanAutotranslate);
	}

	/// <summary>
	/// Test for GetLanguageByCodeAsync method when language is not found.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task GetLanguageByCodeAsync_NotFound()
	{
		ApiResponse<Id.Models.DataViews.LanguageView> result = await _languagesManager.GetLanguageByCodeAsync("xx");

		Assert.False(result.Successful);
		Assert.Null(result.Data);
	}

	/// <summary>
	/// Test for GetLanguageByCodeAsync method when language is not translatable.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task GetAllTranslatableLanguagesAsync_ReturnsFilteredList()
	{
		_ = _dbContext.Languages.Add(new Language { Id = 1, Code = "en", Name = "English", Native = "English", Rtl = 0 });
		_ = _dbContext.Languages.Add(new Language { Id = 2, Code = "cs", Name = "Czech", Native = "Čeština", Rtl = 0 });
		_ = _dbContext.Languages.Add(new Language { Id = 3, Code = "fr", Name = "French", Native = "Français", Rtl = 0 });
		_ = await _dbContext.SaveChangesAsync();

		_ = _translator.GetSupportedLanguagesAsync().Returns(new[] { "en", "fr" });

		ApiResponse<List<Id.Models.DataViews.LanguageView>> result = await _languagesManager.GetAllTranslatableLanguagesAsync();

		Assert.True(result.Successful);
		Assert.Equal(2, result.Data?.Count);
		Assert.Contains(result.Data, l => l.Code == "en");
		Assert.Contains(result.Data, l => l.Code == "fr");
		Assert.DoesNotContain(result.Data, l => l.Code == "cs");
	}
}