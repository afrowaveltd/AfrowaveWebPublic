using System.Globalization;

namespace Id.Middlewares
{
    public class I18nMiddleware(ICookieService cookieService, ILogger<I18nMiddleware> logger) : IMiddleware
    {
        private readonly ICookieService _cookieService = cookieService;
        private readonly ILogger<I18nMiddleware> _logger = logger;

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Check cookie or header for language
            string cultureKey;
            if(_cookieService.GetCookie("language") != string.Empty)
            {
                cultureKey = _cookieService.GetCookie("language");
                context.Request.Headers.AcceptLanguage = cultureKey;
            }
            else if(_cookieService.GetCookie("Language") != string.Empty)
            {
                cultureKey = _cookieService.GetCookie("Language");
                context.Request.Headers.AcceptLanguage = cultureKey;
            }
            else
            {
                cultureKey = context.Request.Headers.AcceptLanguage.ToString() ?? "en";
                _cookieService.SetCookie("language", cultureKey);
            }

            // try to find if we know this language
            if(!string.IsNullOrEmpty(cultureKey))
            {
                if(CultureExists(cultureKey))
                {
                    CultureInfo culture = new CultureInfo(cultureKey);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                    _cookieService.SetCookie("language", cultureKey);
                }
            }
            else
            {
                CultureInfo culture = new CultureInfo("en");
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            await next(context);
        }

        private static bool CultureExists(string cultureName)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => culture.Name.Equals(cultureName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}