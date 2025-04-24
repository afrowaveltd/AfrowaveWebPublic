using Id.ConsoleToolkit.Models.SettingsModels;

namespace Id.Tests.ConsoleToolkit.Tests.Services
{
	/// <summary>
	/// Tests for the SettingsService class.
	/// </summary>
	public class SettingsServiceTests
	{
		private string _tempFilePath = null!;

		private void SetupTestEnvironment()
		{
			_tempFilePath = Path.Combine(AppContext.BaseDirectory, "settings.json");

			if(File.Exists(_tempFilePath))
			{
				File.Delete(_tempFilePath);
			}
		}

		/// <summary>
		/// Tests, if the settings file is created with default values when it does not exist.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task LoadAsync_CreatesDefaultSettings_WhenFileDoesNotExist()
		{
			SetupTestEnvironment();
			Id.ConsoleToolkit.Services.SettingsService service = new Id.ConsoleToolkit.Services.SettingsService();

			ApplicationSettings result = await service.LoadAsync();

			Assert.NotNull(result);
			Assert.False(string.IsNullOrWhiteSpace(result.DefaultLanguage));
			Assert.Equal(result, Id.ConsoleToolkit.Services.SettingsService.Current);
		}

		/// <summary>
		/// Tests, if the settings file is created with default values when it does not exist.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task SaveAsync_WritesToFile_AndCanBeReloaded()
		{
			SetupTestEnvironment();
			Id.ConsoleToolkit.Services.SettingsService service = new Id.ConsoleToolkit.Services.SettingsService();

			ApplicationSettings newSettings = new ApplicationSettings
			{
				DefaultLanguage = "cs",
				ApplicationId = "TestApp",
				SecretKey = "1234",
				ApplicationConfigured = true,
				Translator = new Id.ConsoleToolkit.Models.SettingsModels.Translator { Host = "http://localhost", Port = 8080 }
			};

			await service.SaveAsync(newSettings);
			Assert.True(File.Exists(_tempFilePath));

			// force clear memory (simulate app restart)
			typeof(Id.ConsoleToolkit.Services.SettingsService).GetProperty("Current")?.SetValue(null, new ApplicationSettings());

			ApplicationSettings reloaded = await service.LoadAsync();

			Assert.Equal("cs", reloaded.DefaultLanguage);
			Assert.Equal("TestApp", reloaded.ApplicationId);
			Assert.Equal("1234", reloaded.SecretKey);
			Assert.True(reloaded.ApplicationConfigured);
			Assert.Equal("http://localhost", reloaded.Translator.Host);
			Assert.Equal(8080, reloaded.Translator.Port);
		}

		/// <summary>
		/// Tests, if the settings file is created with default values when it does not exist.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Get_ReturnsSameInstanceAsCurrent()
		{
			SetupTestEnvironment();
			Id.ConsoleToolkit.Services.SettingsService service = new Id.ConsoleToolkit.Services.SettingsService();

			_ = await service.LoadAsync();
			ApplicationSettings settings = service.Get();

			Assert.Equal(Id.ConsoleToolkit.Services.SettingsService.Current, settings);
		}
	}
}