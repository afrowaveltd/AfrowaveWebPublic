using System.Text;

namespace Id.Tests.Api
{
	/// <summary>
	/// Unit tests for the Translations API controller.
	/// </summary>
	public class TranslationsTests
	{
		private readonly ITranslationFilesManager _manager = Substitute.For<ITranslationFilesManager>();
		private readonly IStringLocalizer<Translations> _localizer = Substitute.For<IStringLocalizer<Translations>>();
		private readonly ILogger<Translations> _logger = Substitute.For<ILogger<Translations>>();
		private readonly Translations _controller;

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslationsTests"/> class.
		/// </summary>
		public TranslationsTests()
		{
			_controller = new Translations(_manager, _localizer, _logger);
		}

		/// <summary>
		/// Tests the GetTranslation method of the Translations API controller.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task GetTranslation_ShouldReturnOk_WhenTranslationExists()
		{
			// Arrange
			Dictionary<string, string> translation = new()
			{
				{ "Hello", "Ahoj" },
				{ "Cancel", "Zrušit" }
			};

			ApiResponse<Dictionary<string, string>> response = ApiResponse<Dictionary<string, string>>.Success(translation);
			_ = _manager.GetTranslationAsync(null, "cs").Returns(response);

			// Act
			IActionResult result = await _controller.GetTranslation("cs");

			// Assert
			OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
			ApiResponse<Dictionary<string, string>> data = Assert.IsType<ApiResponse<Dictionary<string, string>>>(ok.Value);
			Assert.True(data.Successful);
			Assert.Equal(2, data.Data?.Count);
		}

		/// <summary>
		/// Tests the GetTranslation method of the Translations API controller when the translation does not exist.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task GetTranslation_ShouldReturnBadRequest_WhenLanguageInvalid()
		{
			// Act
			IActionResult result = await _controller.GetTranslation("czx");

			// Assert
			_ = Assert.IsType<BadRequestObjectResult>(result);
		}

		/// <summary>
		/// Tests the GetTranslationForApp method of the Translations API controller.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task GetTranslationForApp_ShouldReturnOk_WhenTranslationExists()
		{
			// Arrange
			string appId = "123e4567-e89b-12d3-a456-426614174000";

			Dictionary<string, string> translation = new()
			{
				{ "Insert", "Vložit" }
			};

			ApiResponse<Dictionary<string, string>> response = ApiResponse<Dictionary<string, string>>.Success(translation);
			_ = _manager.GetTranslationAsync(appId, "cs").Returns(response);

			// Act
			IActionResult result = await _controller.GetTranslationForApp("cs", appId);

			// Assert
			OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
			ApiResponse<Dictionary<string, string>> data = Assert.IsType<ApiResponse<Dictionary<string, string>>>(ok.Value);
			Assert.True(data.Successful);
			_ = Assert.Single(data.Data);
		}

		/// <summary>
		/// Tests the GetTranslationForApp method of the Translations API controller when the translation does not exist.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task GetTranslationForApp_ShouldReturnBadRequest_WhenAppIdInvalid()
		{
			// Act
			IActionResult result = await _controller.GetTranslationForApp("cs", "not-a-guid");

			// Assert
			_ = Assert.IsType<BadRequestObjectResult>(result);
		}

		/// <summary>
		/// Tests the GetAllTranslationsForApp method of the Translations API controller.
		/// </summary>
		[Fact]
		public async Task GetAllTranslationsForApp_ShouldReturnOk_WhenTranslationsExist()
		{
			// Arrange
			string appId = "123e4567-e89b-12d3-a456-426614174000";

			Dictionary<string, Dictionary<string, string>> allTranslations = new()
			{
				["en"] = new() { { "Insert link", "Insert link" } },
				["cs"] = new() { { "Insert link", "Vložit odkaz" } }
			};

			ApiResponse<Dictionary<string, Dictionary<string, string>>> response =
				ApiResponse<Dictionary<string, Dictionary<string, string>>>.Success(allTranslations);

			_ = _manager.GetAllTranslationsAsync(appId).Returns(response);

			// Act
			IActionResult result = await _controller.GetAllTranslationsAsync(appId);

			// Assert
			OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
			ApiResponse<Dictionary<string, Dictionary<string, string>>> data =
				Assert.IsType<ApiResponse<Dictionary<string, Dictionary<string, string>>>>(ok.Value);

			Assert.True(data.Successful);
			Assert.Equal(2, data.Data?.Count);
		}

		[Fact]
		public async Task ExportTranslationsAsZip_ShouldReturnOk_WhenZipIsCreated()
		{
			// Arrange
			byte[] zipBytes = Encoding.UTF8.GetBytes("fake zip content");
			ApiResponse<byte[]> response = ApiResponse<byte[]>.Success(zipBytes);

			_manager.ExportTranslationsAsZipAsync(null, null).Returns(response);

			// Act
			IActionResult result = await _controller.GetTranslationsAsZip(null, null);

			// Assert
			FileContentResult ok = Assert.IsType<FileContentResult>(result);

			Assert.Equal("application/zip", ok.ContentType);
			Assert.True(ok.FileDownloadName?.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) ?? false);
			Assert.Equal(zipBytes.Length, ok.FileContents.Length);
			Assert.Equal(zipBytes, ok.FileContents);
		}
	}
}