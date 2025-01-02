using SharedTools.Services;

namespace Id.Services
{
   public class ApplicationLoader : IApplicationLoader
   {
      private readonly ApplicationDbContext _context;
      private readonly ILogger<ApplicationLoader> _logger;
      private readonly ITranslatorService _translator;
      private readonly ITextTranslationService _textTranslationService;
      private readonly string jsonsPath;
      private readonly string localesPath;
      private readonly string staticAssetsPath;

      public ApplicationLoader(ApplicationDbContext context,
                                    ILogger<ApplicationLoader> logger,
                                    IWebHostEnvironment environment,
                                    ITranslatorService translator,
                                    ITextTranslationService textTranslationService)
      {
         _context = context;
         _logger = logger;
         _translator = translator;
         _textTranslationService = textTranslationService;
         staticAssetsPath = Path.Combine(environment.WebRootPath, "docs");

         _ = AppDomain.CurrentDomain.BaseDirectory;
         string projectPath = AppDomain.CurrentDomain.BaseDirectory
                             .Substring(0, AppDomain.CurrentDomain.BaseDirectory
                             .IndexOf("bin"));
         jsonsPath = Path.Combine(projectPath, "Jsons");
         localesPath = Path.Combine(projectPath, "Locales");
      }

      private JsonSerializerOptions options = new JsonSerializerOptions
      {
         PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
         DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
         ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
      };

      public async Task ApplyMigrations()
      {
         try
         {
            _logger.LogInformation("Applying migrations");
            await _context.Database.MigrateAsync();
         }
         catch(Exception e)
         {
            _logger.LogError(e, "Error applying migrations");
         }
      }

      public async Task SeedLanguagesAsync()
      {
         try
         {
            if(await _context.Languages.AnyAsync())
            {
               _logger.LogInformation("Languages already seeded");
               return;
            }
            string filePath = Path.Combine(jsonsPath, "languages.json");
            if(!File.Exists(filePath))
            {
               _logger.LogError("languages.json file not found");
               return;
            }
            string json = await File.ReadAllTextAsync(filePath);
            List<Language> languages = JsonSerializer.Deserialize<List<Language>>(json, options) ?? new();
            await _context.Languages.AddRangeAsync(languages);
            _ = await _context.SaveChangesAsync();
            _logger.LogInformation("Seeded {amount} languages.", languages.Count);
         }
         catch(Exception e)
         {
            _logger.LogError(e, "Error seeding languages");
         }
      }

      public string[] GetSupportedCultures()
      {
         try
         {
            string[] supportedLanguages = Directory.GetFiles(localesPath, "*.json")
                .Select(file => Path.GetFileNameWithoutExtension(file))
                .ToArray();
            if(supportedLanguages.Length == 0)
            {
               _logger.LogWarning("No supported languages found");
               return ["en"];
            }
            _logger.LogInformation("Supported languages found: {languages}", supportedLanguages);
            return supportedLanguages;
         }
         catch(Exception e)
         {
            _logger.LogError(e, "Error getting supported languages");
            return ["en"];
         }
      }

      public async Task SeedCountriesAsync()
      {
         if(await _context.Countries.AnyAsync())
         {
            _logger.LogInformation("Countries already seeded");
            return;
         }
         try
         {
            string json = await File.ReadAllTextAsync(Path.Combine(jsonsPath, "countries.json"));
            if(json == null && json == "")
            {
               _logger.LogError("countries.json file not found");
               return;
            }
            List<Country> countries = JsonSerializer.Deserialize<List<Country>>(json, options) ?? new();
            await _context.Countries.AddRangeAsync(countries);
            _ = await _context.SaveChangesAsync();
            _logger.LogInformation("Seeded {amount} countries.", countries.Count);
         }
         catch(Exception e)
         {
            _logger.LogError("Error seeding countries because of {error}", e);
         }
      }

      public async Task TranslateLanguageNamesAsync()
      {
         string[] supportedLanguages = await _translator.GetSupportedLanguagesAsync();
         if(supportedLanguages.Length == 0)
         {
            _logger.LogWarning("No supported languages found");
            return;
         }
         _logger.LogInformation("Supported languages found: {languages}", supportedLanguages);
         DateTime start = DateTime.Now;
         int countTotal = 0;
         int countToTranslate = 0;
         int countSuccess = 0;
         int countFail = 0;
         foreach(string language in supportedLanguages)
         {
            string json = "{}";
            try
            {
               string path = Path.Combine(localesPath, language + ".json");
               json = await File.ReadAllTextAsync(path);
            }
            catch(Exception e)
            {
               _logger.LogError(e, "Error reading {language}.json", language);
            }
            if(json == null && json == "")
            {
               _logger.LogError("Error reading {language} json", language);
            }
            Dictionary<string, string> translations = new();
            try
            {
               translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json, options) ?? new();
            }
            catch(Exception e)
            {
               _logger.LogError(e, "Error deserializing {language}.json", language);
               translations = new();
            }
            List<Country> countries = await _context.Countries.ToListAsync();
            foreach(Country country in countries)
            {
               countTotal++;
               if(translations.ContainsKey(country.Name))
               {
                  continue;
               }
               else
               {
                  countToTranslate++;
                  bool isTranslated = false;
                  while(!isTranslated)
                  {
                     DateTime startTranslation = DateTime.Now;
                     ApiResponse<string> trans = new();
                     if(language == "en")
                     {
                        trans.Data = country.Name;
                     }
                     else
                     {
                        trans = await _translator.TranslateAsync(country.Name, "en", language);
                     }
                     DateTime endTranslation = DateTime.Now;
                     if(trans.Successful)
                     {
                        countSuccess++;
                        translations.Add(country.Name, trans.Data ?? "");
                        _logger.LogInformation("Translated {country} to {language}, as {translation} in [{time} ms]",
                            country.Name,
                            language, trans.Data ?? "",
                            (int)((endTranslation - startTranslation).TotalMilliseconds));
                        isTranslated = true;
                     }
                     else
                     {
                        countFail++;
                        _logger.LogWarning("Error translating {country} to {language}. Retrying in 2 seconds", country.Name, language);
                        Task.Delay(2000).Wait();
                     }
                  }
               }
            }
            translations = translations.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            if(translations.Count == 0)
            {
               _logger.LogWarning("No translations found for {language}", language);
               continue;
            }
            string newJson = JsonSerializer.Serialize(translations);
            try
            {
               await File.WriteAllTextAsync(Path.Combine(localesPath, language + ".json"), newJson);
            }
            catch(Exception e)
            {
               _logger.LogError(e, "Error writing {language}.json", language);
            }
         }
         DateTime end = DateTime.Now;
         if(countToTranslate == 0)
         {
            _logger.LogWarning("No language names to translate");
            return;
         }
         _logger.LogInformation("Found {total} language names, Translated {toTranslate} in {time} s. {success} successful, {fail} failed. AverageTime: [{average} ms]",
             countTotal,
             countToTranslate,
             (int)((end - start).TotalSeconds),
             countSuccess,
             countFail,
         (int)((end - start)).TotalMilliseconds / countToTranslate);
      }

      public async Task<List<ApiResponse<List<string>>>> TranslateStaticAssetsAsync()
      {
         List<ApiResponse<List<string>>> results = new();
         List<string> foldersToTranslate;

         // Get all folders in staticAssetsPath
         try
         {
            foldersToTranslate = Directory.GetDirectories(staticAssetsPath).ToList();

            if(foldersToTranslate.Count == 0)
            {
               _logger.LogWarning("No folders found in {path}", staticAssetsPath);
               return results;
            }

            // Translate each folder using TranslateFolder from the service

            foreach(string folder in foldersToTranslate)
            {
               ApiResponse<List<string>> result = await _textTranslationService.TranslateFolder(folder);
               results.Add(result);
            }
         }
         catch(Exception e)
         {
            _logger.LogError(e, "Error getting folders in {path}", staticAssetsPath);
            results.Add(new ApiResponse<List<string>> { Successful = false, Message = "Error getting folders in " + staticAssetsPath });
         }
         return results;
      }
   }
}