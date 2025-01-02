namespace Id.Services
{
   public interface IApplicationLoader
   {
      Task ApplyMigrations();

      string[] GetSupportedCultures();

      Task SeedCountriesAsync();

      Task SeedLanguagesAsync();

      Task TranslateLanguageNamesAsync();

      Task<List<ApiResponse<List<string>>>> TranslateStaticAssetsAsync();
   }
}