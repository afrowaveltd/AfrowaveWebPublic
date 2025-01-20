using Id.Models.SettingsModels;
using SharedTools.Services;

/*
			 * 1. Load UI translator settings from appsettings.json - done;
			 * 2. Load the default translations file from the file system
			 * 3. Check if old translations are available
			 * 4. Compare the default translations with the old translations
			  * 5. In each language check if there are redundand translations,
			  * 6. If there are, remove them from the actual language file
			  * 7. Translate the values in the queue
			  * 8. Save the new translations to the file system
			 * 9. Do this with all supported languages, except the one which are in the settings list of exceptions.
			  * 10. End the cycle
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
				// 2. Load the default translations file from the file system
				Dictionary<string, string> defaultTranslations = await LoadLanguage(defaultLanguage);
				if(defaultTranslations == null)
				{
					log.DefaultLanguageFound = false;
					log.EndTime = DateTime.UtcNow;
					log.TotalTime = log.EndTime - log.StartTime;
					_ = await _context.UiTranslatorLogs.AddAsync(log);
					_ = await _context.SaveChangesAsync();
					_logger.LogWarning("Default language file not found, ending the task");
					return;
				}
				log.DefaultLanguageFound = true;
				log.PhrazesCount = defaultTranslations.Count;
				Dictionary<string, string> oldTranslations = await LoadOldTranslation();
				Dictionary<string, string> toUpdate = [];
				// 3. Check if old translations are available
				if(oldTranslations.Count > 0)
				{
					log.OldTranslationsFound = true;
					// 4. Compare the default translations with the old translations
					// we need to loop through all record in defaultTranslations. If the key is in oldTranslations and the value is different, we need to add it to the toUpdate dictionary
					foreach(KeyValuePair<string, string> translation in defaultTranslations)
					{
						if(oldTranslations.ContainsKey(translation.Key) && oldTranslations[translation.Key] != translation.Value)
						{
							toUpdate.Add(translation.Key, translation.Value);
						}
					}
				}
				// 5. In each language check if there are redundant translations
				int translatedCount = 0;
				int removedCount = 0;
				int translationErrors = 0;
				foreach(string targetLanguage in translationTargets)
				{
					Dictionary<string, string> targetTranslations = await LoadLanguage(targetLanguage);
					if(targetTranslations == null)
					{
						_logger.LogWarning("Language {language} file not found, skipping", targetLanguage);
						continue;
					}
					// 6. If there are, remove them from the actual language file
					foreach(KeyValuePair<string, string> translation in targetTranslations)
					{
						if(!defaultTranslations.ContainsKey(translation.Key))
						{
							_logger.LogWarning("Redundant translation found for {key} in {language}", translation.Key, targetLanguage);
							_ = targetTranslations.Remove(translation.Key);
							removedCount++;
						}
					}
					// 7. Translate the values in the queue
					foreach(KeyValuePair<string, string> translation in toUpdate)
					{
						DateTime transStart = DateTime.UtcNow;
						ApiResponse<string> response = await _translator.TranslateAsync(translation.Value, defaultLanguage, targetLanguage);
						DateTime transEnd = DateTime.UtcNow;
						if(response == null || response.Data == null)
						{
							_logger.LogWarning("Translation from {source} to {target} for {key} failed", defaultLanguage, targetLanguage, translation.Value);
							translationErrors++;
							continue;
						}
						translatedCount++;
						_logger.LogInformation("Translation from {source} to {target} for {key} took {time} ms : {translation}", defaultLanguage, targetLanguage, translation, (transEnd - transStart).TotalMilliseconds, response.Data);
						targetTranslations[translation.Key] = response.Data;

						// now we check if all the translations are in the targetTranslations. If not, we need to add them
					}
					foreach(KeyValuePair<string, string> defaultTranslation in defaultTranslations)
					{
						if(!targetTranslations.ContainsKey(defaultTranslation.Key))
						{
							DateTime st = DateTime.Now;
							ApiResponse<string> addresponse = await _translator.TranslateAsync(defaultTranslation.Value, defaultLanguage, targetLanguage);
							DateTime sp = DateTime.Now;
							if(addresponse == null || addresponse.Data == null)
							{
								_logger.LogWarning("Translation from {source} to {target} failed", defaultLanguage, targetLanguage);
								continue;
							}
							_logger.LogInformation("Translation from {source} to {target} for {key} took {time} ms : {translation}", defaultLanguage, targetLanguage, defaultTranslation.Key, (st - sp).TotalMilliseconds, addresponse.Data);
							translatedCount++;

							targetTranslations[defaultTranslation.Key] = addresponse.Data;
						}
					}
					// 8. Save the new translations to the file system
					if(!await SaveLanguage(targetLanguage, targetTranslations))
					{
						_logger.LogWarning("Error saving translations for {language}", targetLanguage);
					}
					_logger.LogInformation("Translations for {language} saved", targetLanguage);
				}
				log.EndTime = DateTime.UtcNow;
				log.TotalTime = log.EndTime - log.StartTime;
				log.TranslatedPhrazesCount = translatedCount;
				log.PhrazesToRemoveCount = removedCount;
				log.TranslationErrorCount = translationErrors;
				await StoreOldTranslation(defaultTranslations);
				await _context.UiTranslatorLogs.AddAsync(log);
				await _context.SaveChangesAsync();
			}
			else
			{
				_logger.LogWarning("No target languages for translations found");
				log.EndTime = DateTime.UtcNow;
				log.TotalTime = log.EndTime - log.StartTime;
			}

			_logger.LogInformation("UiTranslatorService is done.");
		}

		public bool OldLangaugeExists()
		{
			string targetFile = Path.Join(tempFolderPath, "old.json");
			return File.Exists(targetFile);
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
				try
				{
					File.Move(targetFile, Path.Join(tempFolderPath, language + DateTime.UtcNow.ToString() + ".json"));
					return [];
				}
				catch(Exception e)
				{
					_logger.LogError("Error moving file {target} to {temp} due to error {error}", targetFile, Path.Join(tempFolderPath, language + DateTime.UtcNow.ToString() + ".json"), e);
					return null;
				}
			}
			try
			{
				return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
			}
			catch(Exception ex)
			{
				_logger.LogError("Error deserializing file {target} due to error {error}", targetFile, ex);
				try
				{
					File.Move(targetFile, Path.Join(tempFolderPath, language + DateTime.UtcNow.ToString() + ".json"));
					return [];
				}
				catch(Exception e)
				{
					_logger.LogError("Error moving file {target} to {temp} due to error {error}", targetFile, Path.Join(tempFolderPath, language + DateTime.UtcNow.ToString() + ".json"), e);
					return null;
				}
			}
		}

		private async Task<bool> SaveLanguage(string language, Dictionary<string, string> translations)
		{
			if(language == null)
			{
				_logger.LogError("Missing language name");
				return false;
			}
			if(translations == null)
			{
				_logger.LogError("Missing translations");
				return false;
			}
			string targetFile = Path.Join(localesPath, language + ".json");
			try
			{
				await File.WriteAllTextAsync(targetFile, JsonSerializer.Serialize(translations));
				return true;
			}
			catch(Exception ex)
			{
				_logger.LogError("Error saving file {target} due to error {error}", targetFile, ex);
				return false;
			}
		}

		private async Task<Dictionary<string, string>> LoadOldTranslation()
		{
			string targetFile = Path.Join(tempFolderPath, "old.json");
			if(!File.Exists(targetFile))
			{
				_logger.LogWarning("Old translation file not found");
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
				return [];
			}
			try
			{
				return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
			}
			catch(Exception ex)
			{
				_logger.LogError("Error deserializing file {target} due to error {error}", targetFile, ex);
				return [];
			}
		}

		private async Task StoreOldTranslation(Dictionary<string, string> dictionary)
		{
			string targetFile = Path.Join(tempFolderPath, "old.json");
			try
			{
				await File.WriteAllTextAsync(targetFile, JsonSerializer.Serialize(dictionary));
			}
			catch(Exception ex)
			{
				_logger.LogError("Error saving file {target} due to error {error}", targetFile, ex);
			}
		}
	}
}