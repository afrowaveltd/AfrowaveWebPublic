using System.Globalization;

namespace Id.Middlewares
{
	/// <summary>
	/// Middleware for handling internationalization (i18n) by setting culture based on cookies or request headers.
	/// </summary>
	public class I18nMiddleware(ICookieService cookieService, ILogger<I18nMiddleware> logger) : IMiddleware
	{
		private readonly ICookieService _cookieService = cookieService;
		private readonly ILogger<I18nMiddleware> _logger = logger;

		/// <summary>
		/// Middleware execution logic to set the culture for the request.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		/// <param name="next">The next middleware delegate.</param>
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			// Check cookie or header for language preference
			string cultureKey;
			if(!string.IsNullOrEmpty(_cookieService.GetCookie("language")))
			{
				cultureKey = _cookieService.GetCookie("language");
				context.Request.Headers.AcceptLanguage = cultureKey;
			}
			else if(!string.IsNullOrEmpty(_cookieService.GetCookie("Language")))
			{
				cultureKey = _cookieService.GetCookie("Language");
				context.Request.Headers.AcceptLanguage = cultureKey;
			}
			else
			{
				cultureKey = context.Request.Headers.AcceptLanguage.ToString() ?? "en";
				_cookieService.SetCookie("language", cultureKey);
			}

			// Apply culture settings if valid
			if(!string.IsNullOrEmpty(cultureKey) && CultureExists(cultureKey))
			{
				CultureInfo culture = new CultureInfo(cultureKey);
				Thread.CurrentThread.CurrentCulture = culture;
				Thread.CurrentThread.CurrentUICulture = culture;
				_cookieService.SetCookie("language", cultureKey);
			}
			else
			{
				CultureInfo defaultCulture = new CultureInfo("en");
				Thread.CurrentThread.CurrentCulture = defaultCulture;
				Thread.CurrentThread.CurrentUICulture = defaultCulture;
			}

			await next(context);
		}

		/// <summary>
		/// Checks if the provided culture exists in the system.
		/// </summary>
		/// <param name="cultureName">The culture name to check.</param>
		/// <returns>True if the culture exists, otherwise false.</returns>
		private static bool CultureExists(string cultureName)
		{
			return CultureInfo.GetCultures(CultureTypes.AllCultures)
				 .Any(culture => culture.Name.Equals(cultureName, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}