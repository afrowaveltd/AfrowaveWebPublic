namespace Id.Tests.Api
{
	/// <summary>
	/// Unit tests for the <see cref="GetLocalized"/> class.
	/// </summary>
	public class GetLocalizedTests
	{
		private readonly IStringLocalizer<GetLocalized> _localizerMock;
		private readonly ITranslatorService _translatorMock;
		private readonly ILogger<GetLocalized> _loggerMock;
		private readonly GetLocalized _controller;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetLocalizedTests"/> class.
		/// </summary>
		public GetLocalizedTests()
		{
			// Arrange
			_localizerMock = Substitute.For<IStringLocalizer<GetLocalized>>();
			_translatorMock = Substitute.For<ITranslatorService>();
			_loggerMock = Substitute.For<ILogger<GetLocalized>>();

			IHttpContextAccessor httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
			DefaultHttpContext mockHttpContext = new DefaultHttpContext();

			// Simulate a valid language context
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
			_ = httpContextAccessorMock.HttpContext.Returns(mockHttpContext);

			_controller = new GetLocalized(
				  _localizerMock,
				  _translatorMock,
				  _loggerMock,
				  httpContextAccessorMock);
		}

		/// <summary>
		/// Tests whether OnGetAsync returns localized text when localization exists.
		/// </summary>
		[Fact]
		public async Task OnGetAsync_ShouldReturnLocalizedText_WhenLocalizationExists()
		{
			// Arrange
			_ = _localizerMock["Hello"].Returns(new LocalizedString("Hello", "Hola", false));

			// Act
			IActionResult result = await _controller.OnGetAsync("Hello", "es");

			// Assert
			_ = result.Should().BeOfType<OkObjectResult>()
					.Which.Value.Should().Be("Hola");
		}

		/// <summary>
		/// Tests whether OnGetAsync translates text when localization is missing.
		/// </summary>
		[Fact]
		public async Task OnGetAsync_ShouldTranslateText_WhenLocalizationIsMissing()
		{
			// Arrange
			_ = _localizerMock["Hello"].Returns(new LocalizedString("Hello", "Hello", true));
			_ = _translatorMock.GetSupportedLanguagesAsync().Returns(new string[] { "es" });
			_ = _translatorMock.TranslateAsync("Hello", "en", "es")
				 .Returns(Task.FromResult(new ApiResponse<string> { Data = "Hola", Successful = true }));

			// Act
			IActionResult result = await _controller.OnGetAsync("Hello", "es");

			// Assert
			_ = result.Should().BeOfType<OkObjectResult>()
					.Which.Value.Should().Be("Hola");

			_ = await _translatorMock.Received(1).TranslateAsync("Hello", "en", "es");
		}
	}
}