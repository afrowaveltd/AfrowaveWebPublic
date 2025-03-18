using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Id.Tests.Api
{
	/// <summary>
	/// Unit tests for the <see cref="GetLocalized"/> class.
	/// </summary>

	/// <summary>
	/// Unit tests for the <see cref="GetLocalized"/> class.
	/// </summary>
	public class GetLocalizedTests
	{
		private readonly Mock<IStringLocalizer<GetLocalized>> _localizerMock;
		private readonly Mock<ITranslatorService> _translatorMock;
		private readonly Mock<ILogger<GetLocalized>> _loggerMock;
		private readonly GetLocalized _controller;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetLocalizedTests"/> class.
		/// </summary>
		public GetLocalizedTests()
		{
			_localizerMock = new Mock<IStringLocalizer<GetLocalized>>();
			_translatorMock = new Mock<ITranslatorService>();
			_loggerMock = new Mock<ILogger<GetLocalized>>();

			Mock<IHttpContextAccessor> httpContextAccessorMock = new Mock<IHttpContextAccessor>();
			DefaultHttpContext mockHttpContext = new DefaultHttpContext();

			// Simulate a valid language context
			Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es");

			_ = httpContextAccessorMock.Setup(x => x.HttpContext).Returns(mockHttpContext);

			_controller = new GetLocalized(
				 _localizerMock.Object,
				 _translatorMock.Object,
				 _loggerMock.Object,
				 httpContextAccessorMock.Object);
		}

		/// <summary>
		/// Tests whether OnGetAsync returns localized text when localization exists.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task OnGetAsync_ShouldReturnLocalizedText_WhenLocalizationExists()
		{
			// ✅ FIX: Properly mock the localizer to return "Hola"
			_ = _localizerMock.Setup(l => l["Hello"])
				 .Returns(new LocalizedString("Hello", "Hola", false));

			IActionResult result = await _controller.OnGetAsync("Hello", "es");

			_ = result.Should().BeOfType<OkObjectResult>()
					.Which.Value.Should().Be("Hola");
		}

		/// <summary>
		/// Tests whether OnGetAsync translates text when localization is missing.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task OnGetAsync_ShouldTranslateText_WhenLocalizationIsMissing()
		{
			// ✅ FIX: Ensure localizer does not have a translation
			_ = _localizerMock.Setup(l => l["Hello"])
				 .Returns(new LocalizedString("Hello", "Hello", true));

			// ✅ Ensure that the translation service is used
			_ = _translatorMock.Setup(t => t.GetSupportedLanguagesAsync())
				 .ReturnsAsync(new string[] { "es" });

			_ = _translatorMock.Setup(t => t.TranslateAsync("Hello", "en", "es"))
				 .ReturnsAsync(new ApiResponse<string> { Data = "Hola", Successful = true });

			IActionResult result = await _controller.OnGetAsync("Hello", "es");

			_ = result.Should().BeOfType<OkObjectResult>()
					.Which.Value.Should().Be("Hola");

			// ✅ Verify that the translator was called
			_translatorMock.Verify(t => t.TranslateAsync("Hello", "en", "es"), Times.Once);
		}
	}
}