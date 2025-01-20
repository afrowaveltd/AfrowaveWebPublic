using Id.Models.SettingsModels;
using SharedTools.Services;

/*
			 * 1. Load UI translator settings from appsettings.json - done;
			 * 2. Load the default translations file from the file system
			 * 3. Check if old translations are available
			 * 4. Compare the default translations with the old translations
			 * 5. Add missing or changed values to the gueue for translation
			 * 6. In each language check if there are redundand translations,
			 * 7. If there are, remove them from the actual language file
			 * 8. Translate the values in the queue
			 * 9. Save the new translations to the file system
			 * 10. Do this with all supported languages, except the one which are in the settings list of exceptions.
			 * 11. End the cycle
			 */

namespace Id.Services
{
	public class UiTranslatorService(ILogger<UiTranslatorService> logger,
		ITranslatorService translator,
		IConfiguration config,
		ApplicationDbContext context) : IUiTranslatorService
	{
		// 1. Load UI translator settings from appsettings.json - done;
		private readonly UITranslator uiTranslator = config.GetSection("UiTranslator").Get<UITranslator>() ?? new UITranslator();

		private readonly ApplicationDbContext _context = context;
		private readonly ILogger<UiTranslatorService> _logger = logger;
		private readonly ITranslatorService _translator = translator;

		private readonly string localesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
									  .Substring(0, AppDomain.CurrentDomain.BaseDirectory
									  .IndexOf("bin")), "Locales");

		public string tempFolderPath = string.Empty;
		public string defaultLanguage = string.Empty;
		public List<string> ignoredLanguages = [];

		public async Task RunTranslationsAsync()
		{
			_logger.LogInformation("UiTranslatorService is starting.");
			UiTranslatorLog log = new();
			try
			{
				tempFolderPath = Path.Combine(Path.GetTempPath(), "afrowave_id");
				if(!Directory.Exists(tempFolderPath))
				{
					_ = Directory.CreateDirectory(tempFolderPath);
				}
			}
			catch(UnauthorizedAccessException)
			{
				tempFolderPath = Path.Combine(AppContext.BaseDirectory, "Temp");
				if(!Directory.Exists(tempFolderPath))
				{
					_ = Directory.CreateDirectory(tempFolderPath);
				}
			}
			log.StartTime = DateTime.UtcNow;
			Console.WriteLine(localesPath);
			Console.WriteLine(tempFolderPath);
			defaultLanguage = uiTranslator.DefaultLanguage;
			ignoredLanguages = uiTranslator.IgnoredLanguages;
			List<string> supportedLanguages = [.. (await _translator.GetSupportedLanguagesAsync())];
			if(supportedLanguages == null || supportedLanguages.Count == 0)
			{
				log.TargetLanguagesCount = 0;
				log.EndTime = DateTime.UtcNow;
				log.TotalTime = log.EndTime - log.StartTime;
				_ = await _context.UiTranslatorLogs.AddAsync(log);
				_ = await _context.SaveChangesAsync();
				_logger.LogWarning("Can't get list of supported languages, ending the task");
				return;
			}

			if(!supportedLanguages.Contains(defaultLanguage))
			{
				log.DefaultLanguageFound = false;
				log.EndTime = DateTime.UtcNow;
				log.TotalTime = log.EndTime - log.StartTime;
				_ = await _context.UiTranslatorLogs.AddAsync(log);
				_ = await _context.SaveChangesAsync();
				_logger.LogWarning("Default language is not supported by translator service, ending the task");
				return;
			}
			List<string> translationTargets = supportedLanguages.ToList();
			foreach(string ignoredLanguage in ignoredLanguages)
			{
				if(translationTargets.Contains(ignoredLanguage))
				{
					_ = translationTargets.Remove(ignoredLanguage);
				}
			}
			if(translationTargets.Count > 0)
			{
			}
			else
			{
				_logger.LogWarning("No target languages for translations found");
				log.EndTime = DateTime.UtcNow;
				log.TotalTime = log.EndTime - log.StartTime;
			}

			_logger.LogInformation("UiTranslatorService is done.");
		}

		private async Task<Dictionary<string, string>> LoadLanguage(string language)
		{
			string targetFile = Path.Join(localesPath, language + ".json");
			if(language == null)
			{
				_logger.LogError("Missing language name");
				return null;
			}
			if(!File.Exists(targetFile))
			{
				_logger.LogWarning($"Language {language} not found");
				return [];
			}
			string json;
			try
			{
				json = await File.ReadAllTextAsync(targetFile);
			}
			catch(Exception ex)
			{
				_logger.LogError("Error loading file {target} due to error {error}", targetFile, ex);
			}
		}

		private async Task SaveLanguage(string language, Dictionary<string, string>)
		{
		}

		private async Task<Dictionary<string, string>> LoadOldTranslation()
		{
		}

		private async Task StoreOldTranslation(Dictionary<string, string>)
		{
		}
	}
}