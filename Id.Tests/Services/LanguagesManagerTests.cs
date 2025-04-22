public class LanguagesManagerTests
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IStringLocalizer<LanguagesManager> _localizer;
	private readonly ILogger<LanguagesManager> _logger;
	private readonly ITranslatorService _translator;
	private readonly ILanguagesManager _languagesManager;

	public LanguagesManagerTests()
	{
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			 .Options;

		_dbContext = new ApplicationDbContext(options);
		_localizer = Substitute.For<IStringLocalizer<LanguagesManager>>();
		_logger = Substitute.For<ILogger<LanguagesManager>>();
		_translator = Substitute.For<ITranslatorService>();

		_languagesManager = new LanguagesManager(_dbContext, _localizer, _logger, _translator);
	}

	[Fact]
	public async Task GetAllLanguagesAsync_ReturnsLanguages()
	{
		_dbContext.Languages.Add(new Language { Id = 1, Code = "en", Name = "English", Native = "English", Rtl = 0 });
		_dbContext.Languages.Add(new Language { Id = 2, Code = "cs", Name = "Czech", Native = "Čeština", Rtl = 0 });
		await _dbContext.SaveChangesAsync();

		_translator.GetSupportedLanguagesAsync().Returns(new[] { "en", "cs" });

		var result = await _languagesManager.GetAllLanguagesAsync();

		Assert.True(result.Successful);
		Assert.Equal(2, result.Data.Count);
		Assert.All(result.Data, lang => Assert.True(lang.CanAutotranslate));
	}

	[Fact]
	public async Task GetAllLanguagesAsync_ReturnsEmpty()
	{
		var result = await _languagesManager.GetAllLanguagesAsync();

		Assert.False(result.Successful);
		Assert.Null(result.Data);
	}

	[Fact]
	public async Task GetLanguageByCodeAsync_Found()
	{
		_dbContext.Languages.Add(new Language { Id = 1, Code = "en", Name = "English", Native = "English", Rtl = 0 });
		await _dbContext.SaveChangesAsync();

		_translator.GetSupportedLanguagesAsync().Returns(new[] { "en" });

		var result = await _languagesManager.GetLanguageByCodeAsync("en");

		Assert.True(result.Successful);
		Assert.Equal("en", result.Data.Code);
		Assert.True(result.Data.CanAutotranslate);
	}

	[Fact]
	public async Task GetLanguageByCodeAsync_NotFound()
	{
		var result = await _languagesManager.GetLanguageByCodeAsync("xx");

		Assert.False(result.Successful);
		Assert.Null(result.Data);
	}

	[Fact]
	public async Task GetAllTranslatableLanguagesAsync_ReturnsFilteredList()
	{
		_dbContext.Languages.Add(new Language { Id = 1, Code = "en", Name = "English", Native = "English", Rtl = 0 });
		_dbContext.Languages.Add(new Language { Id = 2, Code = "cs", Name = "Czech", Native = "Čeština", Rtl = 0 });
		_dbContext.Languages.Add(new Language { Id = 3, Code = "fr", Name = "French", Native = "Français", Rtl = 0 });
		await _dbContext.SaveChangesAsync();

		_translator.GetSupportedLanguagesAsync().Returns(new[] { "en", "fr" });

		var result = await _languagesManager.GetAllTranslatableLanguagesAsync();

		Assert.True(result.Successful);
		Assert.Equal(2, result.Data.Count);
		Assert.Contains(result.Data, l => l.Code == "en");
		Assert.Contains(result.Data, l => l.Code == "fr");
		Assert.DoesNotContain(result.Data, l => l.Code == "cs");
	}
}