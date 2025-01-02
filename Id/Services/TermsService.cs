using SharedTools.Services;

namespace Id.Services
{
   public class TermsService(IWebHostEnvironment environment, ITextToHtmlService textToHtmlService, ILogger<TermsService> logger) : ITermsService
   {
      private readonly IWebHostEnvironment _environment = environment;
      private readonly ITextToHtmlService _textToHtmlService = textToHtmlService;
      private readonly ILogger<TermsService> _logger = logger;

      public async Task<string> GetTermsHtmlAsync(string language)
      {
         string termsPath = Path.Combine(_environment.WebRootPath, "docs", "terms", $"{language}.txt");

         if(!File.Exists(termsPath))
         {
            _logger.LogWarning("Terms file not found for language {language}", language);
            termsPath = Path.Combine(_environment.WebRootPath, "docs", "terms", "en.txt");
         }

         if(!File.Exists(termsPath))
         {
            _logger.LogError("Terms file not found for any language");
            return string.Empty;
         }

         _logger.LogInformation("Reading terms from {termsPath}", termsPath);
         string termsText = await File.ReadAllTextAsync(termsPath);

         return _textToHtmlService.ConvertTextToHtml(termsText);
      }
   }
}