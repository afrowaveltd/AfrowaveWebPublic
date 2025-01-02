using System.Globalization;

namespace Id.Pages
{
   public class TermsModel(ITermsService termsService, ILogger<TermsModel> logger) : PageModel
   {
      private readonly ITermsService _termsService = termsService;
      private readonly ILogger<TermsModel> _logger = logger;

      public string TermsHtml { get; private set; } = string.Empty;

      public async Task OnGetAsync()
      {
         string userLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

         TermsHtml = await _termsService.GetTermsHtmlAsync(userLanguage);
      }
   }
}