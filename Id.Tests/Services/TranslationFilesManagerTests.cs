using System.IO.Compression;

namespace Id.Tests.Services
{
	/// <summary>
	/// Tests for the <see cref="TranslationFilesManager"/> class.
	/// </summary>
	public class TranslationFilesManagerTests : IDisposable
	{
		private readonly string _testDir;
		private readonly string _enPath;
		private readonly string _csPath;
		private readonly string _oldPath;

		private readonly TranslationFilesManager _manager;

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslationFilesManagerTests"/> class.
		/// </summary>
		public TranslationFilesManagerTests()
		{
			_testDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Split("bin")[0], "Locales");
			_ = Directory.CreateDirectory(_testDir);

			_enPath = Path.Combine(_testDir, "en.json");
			_csPath = Path.Combine(_testDir, "cs.json");
			_oldPath = Path.Combine(_testDir, "old.json");

			File.WriteAllText(_enPath, "{\"Hello\":\"Hello\"}");
			File.WriteAllText(_csPath, "{\"Hello\":\"Ahoj\"}");
			File.WriteAllText(_oldPath, "{\"Hello\":\"Old\"}");

			IApplicationsManager appsManager = Substitute.For<IApplicationsManager>();
			_ = appsManager.GetAuthenticatorIdAsync().Returns("dummy");

			ILogger<TranslationFilesManager> logger = Substitute.For<ILogger<TranslationFilesManager>>();

			IStringLocalizer<TranslationFilesManager> localizer = Substitute.For<IStringLocalizer<TranslationFilesManager>>();
			_ = localizer["ZIP export failed"].Returns(new LocalizedString("ZIP export failed", "ZIP export failed"));
			_ = localizer["Translation folder not found"].Returns(new LocalizedString("Translation folder not found", "Translation folder not found"));

			IWebHostEnvironment env = Substitute.For<IWebHostEnvironment>();
			_ = env.WebRootPath.Returns(AppContext.BaseDirectory);

			_manager = new TranslationFilesManager(appsManager, env, logger, localizer);
		}

		/// <summary>
		/// Tests the ExportTranslationsAsZipAsync method.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task ExportTranslationsAsZipAsync_ShouldReturnZip_WithoutOldJson()
		{
			ApiResponse<byte[]> result = await _manager.ExportTranslationsAsZipAsync(null);

			Assert.True(result.Successful);
			Assert.NotNull(result.Data);

			using MemoryStream ms = new(result.Data);
			using ZipArchive archive = new(ms, ZipArchiveMode.Read);

			Assert.Equal(2, archive.Entries.Count);
			Assert.DoesNotContain(archive.Entries, e => e.FullName == "old.json");
			Assert.Contains(archive.Entries, e => e.FullName == "en.json");
			Assert.Contains(archive.Entries, e => e.FullName == "cs.json");
		}

		/// <summary>
		/// Tests the ExportTranslationsAsZipAsync method with specific languages.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task GetTranslationAsync_ShouldReturnCorrectFile()
		{
			ApiResponse<Dictionary<string, string>> result = await _manager.GetTranslationAsync(null, "cs");

			Assert.True(result.Successful);
			Assert.NotNull(result.Data);
			_ = Assert.Contains("Hello", result.Data);
			Assert.Equal("Ahoj", result.Data["Hello"]);
		}

		/// <summary>
		/// Tests the GetAllTranslationsAsync method.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task GetAllTranslationsAsync_ShouldReturnAll_ExceptOld()
		{
			ApiResponse<Dictionary<string, Dictionary<string, string>>> result = await _manager.GetAllTranslationsAsync(null);

			Assert.True(result.Successful);
			Assert.NotNull(result.Data);

			Assert.True(result.Data.ContainsKey("en"));
			Assert.True(result.Data.ContainsKey("cs"));
			Assert.False(result.Data.ContainsKey("old"));

			Assert.Equal("Hello", result.Data["en"].Keys.First());
			Assert.Equal("Hello", result.Data["cs"].Keys.First());
		}

		/// <summary>
		/// Cleans up the test files and directories.
		/// </summary>
		public void Dispose()
		{
			if(File.Exists(_enPath))
			{
				File.Delete(_enPath);
			}

			if(File.Exists(_csPath))
			{
				File.Delete(_csPath);
			}

			if(File.Exists(_oldPath))
			{
				File.Delete(_oldPath);
			}

			if(Directory.Exists(_testDir))
			{
				Directory.Delete(_testDir, true);
			}
		}

		/// <summary>
		/// Test paths for the translation files.
		/// </summary>
		public static class TestPaths
		{
			/// <summary>
			/// Gets the path to the locales directory.
			/// </summary>
			/// <returns></returns>
			public static string GetLocalesPath() =>
				 Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Split("bin")[0], "Locales");

			/// <summary>
			/// Gets the path to the translations directory.
			/// </summary>
			/// <returns></returns>
			public static string GetTranslationsPath() =>
				 Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Split("bin")[0], "Translations");
		}
	}
}